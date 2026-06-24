/* ============================================================================
   BRICOMA.FIDELITE — Suppression des colonnes en double (alignement sur Profil)
   ----------------------------------------------------------------------------
   On retire les colonnes ajoutées en double, devenues inutiles :
     - AspNetUsers : Nom, Prenom, RefMagasinId  (→ désormais dans la table Profil)
     - AspNetRoles : Description, CreatedAt      (non utilisées)

   IDEMPOTENT / GUARDÉ (IF EXISTS) : ne fait rien si la colonne n'existe pas.
   → Sûr à exécuter sur staging (BRICOMA.FIDELITE) ET sur BRICOMA.ECOMMERCE
     (où ces colonnes n'existent probablement pas = no-op).

   ⚠️ À exécuter APRÈS le backfill (002) et APRÈS validation du refactor.
   ============================================================================ */

SET NOCOUNT ON;

DECLARE @sql NVARCHAR(MAX);

/* Helper : supprime la contrainte DEFAULT éventuelle puis la colonne, si elle existe. */

-- ---- AspNetUsers.Nom ----
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.AspNetUsers') AND name = 'Nom')
BEGIN
    SELECT @sql = 'ALTER TABLE dbo.AspNetUsers DROP CONSTRAINT ' + dc.name
    FROM sys.default_constraints dc
    JOIN sys.columns c ON c.object_id = dc.parent_object_id AND c.column_id = dc.parent_column_id
    WHERE dc.parent_object_id = OBJECT_ID('dbo.AspNetUsers') AND c.name = 'Nom';
    IF @sql IS NOT NULL EXEC sp_executesql @sql;
    ALTER TABLE dbo.AspNetUsers DROP COLUMN Nom;
    PRINT 'AspNetUsers.Nom supprimée.';
END

-- ---- AspNetUsers.Prenom ----
SET @sql = NULL;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.AspNetUsers') AND name = 'Prenom')
BEGIN
    SELECT @sql = 'ALTER TABLE dbo.AspNetUsers DROP CONSTRAINT ' + dc.name
    FROM sys.default_constraints dc
    JOIN sys.columns c ON c.object_id = dc.parent_object_id AND c.column_id = dc.parent_column_id
    WHERE dc.parent_object_id = OBJECT_ID('dbo.AspNetUsers') AND c.name = 'Prenom';
    IF @sql IS NOT NULL EXEC sp_executesql @sql;
    ALTER TABLE dbo.AspNetUsers DROP COLUMN Prenom;
    PRINT 'AspNetUsers.Prenom supprimée.';
END

-- ---- AspNetUsers.RefMagasinId ----
SET @sql = NULL;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.AspNetUsers') AND name = 'RefMagasinId')
BEGIN
    SELECT @sql = 'ALTER TABLE dbo.AspNetUsers DROP CONSTRAINT ' + dc.name
    FROM sys.default_constraints dc
    JOIN sys.columns c ON c.object_id = dc.parent_object_id AND c.column_id = dc.parent_column_id
    WHERE dc.parent_object_id = OBJECT_ID('dbo.AspNetUsers') AND c.name = 'RefMagasinId';
    IF @sql IS NOT NULL EXEC sp_executesql @sql;
    ALTER TABLE dbo.AspNetUsers DROP COLUMN RefMagasinId;
    PRINT 'AspNetUsers.RefMagasinId supprimée.';
END

-- ---- AspNetRoles.Description ----
SET @sql = NULL;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.AspNetRoles') AND name = 'Description')
BEGIN
    SELECT @sql = 'ALTER TABLE dbo.AspNetRoles DROP CONSTRAINT ' + dc.name
    FROM sys.default_constraints dc
    JOIN sys.columns c ON c.object_id = dc.parent_object_id AND c.column_id = dc.parent_column_id
    WHERE dc.parent_object_id = OBJECT_ID('dbo.AspNetRoles') AND c.name = 'Description';
    IF @sql IS NOT NULL EXEC sp_executesql @sql;
    ALTER TABLE dbo.AspNetRoles DROP COLUMN Description;
    PRINT 'AspNetRoles.Description supprimée.';
END

-- ---- AspNetRoles.CreatedAt ----
SET @sql = NULL;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.AspNetRoles') AND name = 'CreatedAt')
BEGIN
    SELECT @sql = 'ALTER TABLE dbo.AspNetRoles DROP CONSTRAINT ' + dc.name
    FROM sys.default_constraints dc
    JOIN sys.columns c ON c.object_id = dc.parent_object_id AND c.column_id = dc.parent_column_id
    WHERE dc.parent_object_id = OBJECT_ID('dbo.AspNetRoles') AND c.name = 'CreatedAt';
    IF @sql IS NOT NULL EXEC sp_executesql @sql;
    ALTER TABLE dbo.AspNetRoles DROP COLUMN CreatedAt;
    PRINT 'AspNetRoles.CreatedAt supprimée.';
END
GO
