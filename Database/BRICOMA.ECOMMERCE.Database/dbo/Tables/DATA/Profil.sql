CREATE TABLE [dbo].[Profil]
(
    [Id]             NVARCHAR(450) NOT NULL PRIMARY KEY,
    [Prenom]         NVARCHAR(MAX) NOT NULL,
    [Nom]            NVARCHAR(MAX) NOT NULL,
    [REF_Magasin_Id] INT           NULL,
    [DateCreation]   DATETIME      NOT NULL,
    CONSTRAINT [FK_Profil_ToAspNetUsers] FOREIGN KEY ([Id]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Profil_ToREF_Magasin] FOREIGN KEY ([REF_Magasin_Id]) REFERENCES [dbo].[REF_Magasin] ([Id])
)
