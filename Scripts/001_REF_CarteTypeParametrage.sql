/* ============================================================================
   BRICOMA.FIDELITE — Paramétrage des types de carte
   ----------------------------------------------------------------------------
   Crée la table REF_CarteTypeParametrage (message de réception, image-modèle,
   position X/Y et taille du code-barres) + sa colonne BarcodeScale.

   IDEMPOTENT : peut être rejoué sans erreur (vérifie l'existence avant chaque
   action). À exécuter sur la base BRICOMA.FIDELITE après un clone/merge du code.

   Base   : BRICOMA.FIDELITE
   Auteur : équipe back-office
   ============================================================================ */

SET NOCOUNT ON;

/* 1) Table REF_CarteTypeParametrage ---------------------------------------- */
IF OBJECT_ID(N'dbo.REF_CarteTypeParametrage', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.REF_CarteTypeParametrage
    (
        Id                INT IDENTITY(1,1) NOT NULL
            CONSTRAINT PK_REF_CarteTypeParametrage PRIMARY KEY,
        REF_CarteType_Id  INT            NOT NULL,
        MessageReception  NVARCHAR(1000) NULL,
        ImagePath         NVARCHAR(500)  NULL,
        BarcodeX          INT            NOT NULL CONSTRAINT DF_RCTP_BarcodeX     DEFAULT (50),
        BarcodeY          INT            NOT NULL CONSTRAINT DF_RCTP_BarcodeY     DEFAULT (50),
        BarcodeScale      INT            NOT NULL CONSTRAINT DF_RCTP_BarcodeScale DEFAULT (100),
        CONSTRAINT FK_RCTP_ToREF_CarteType
            FOREIGN KEY (REF_CarteType_Id) REFERENCES dbo.REF_CarteType (Id),
        CONSTRAINT UQ_RCTP_CarteType UNIQUE (REF_CarteType_Id)
    );

    PRINT 'Table dbo.REF_CarteTypeParametrage créée.';
END
ELSE
    PRINT 'Table dbo.REF_CarteTypeParametrage déjà présente — ignorée.';
GO

/* 2) Colonne BarcodeScale (si table créée par une version antérieure) ------- */
IF OBJECT_ID(N'dbo.REF_CarteTypeParametrage', N'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.columns
                   WHERE object_id = OBJECT_ID(N'dbo.REF_CarteTypeParametrage')
                     AND name = N'BarcodeScale')
BEGIN
    ALTER TABLE dbo.REF_CarteTypeParametrage
        ADD BarcodeScale INT NOT NULL CONSTRAINT DF_RCTP_BarcodeScale DEFAULT (100);

    PRINT 'Colonne BarcodeScale ajoutée.';
END
ELSE
    PRINT 'Colonne BarcodeScale déjà présente — ignorée.';
GO
