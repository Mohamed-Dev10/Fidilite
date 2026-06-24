/* ============================================================================
   BRICOMA.FIDELITE — Backfill Profil depuis AspNetUsers
   ----------------------------------------------------------------------------
   On bascule les infos utilisateur (Nom, Prenom, magasin) de AspNetUsers vers
   la table Profil (source officielle). Ce script recopie ces données pour les
   utilisateurs qui n'ont pas encore de ligne Profil, afin de ne perdre le
   magasin (et donc le scoping) d'AUCUN utilisateur.

   IDEMPOTENT : n'insère que les Profil manquants. Ne touche pas aux Profil
   existants (ils sont considérés comme la source de vérité).

   À exécuter AVANT de supprimer les colonnes de AspNetUsers (lot C).
   Base : BRICOMA.FIDELITE (staging) — adapter si autre base.
   ============================================================================ */

SET NOCOUNT ON;

INSERT INTO dbo.Profil (Id, Prenom, Nom, REF_Magasin_Id, DateCreation)
SELECT
    u.Id,
    ISNULL(u.Prenom, '') AS Prenom,
    ISNULL(u.Nom, '')    AS Nom,
    u.RefMagasinId,
    GETDATE()
FROM dbo.AspNetUsers u
WHERE NOT EXISTS (SELECT 1 FROM dbo.Profil p WHERE p.Id = u.Id);

PRINT CONCAT(@@ROWCOUNT, ' ligne(s) Profil créée(s) depuis AspNetUsers.');
GO

/* Contrôle (lecture) : vérifier qu'aucun user n'a un magasin sans Profil correspondant */
SELECT u.Id, u.Email, u.RefMagasinId AS Magasin_AspNetUsers, p.REF_Magasin_Id AS Magasin_Profil
FROM dbo.AspNetUsers u
LEFT JOIN dbo.Profil p ON p.Id = u.Id
WHERE u.RefMagasinId IS NOT NULL AND (p.Id IS NULL OR p.REF_Magasin_Id IS NULL);
GO
