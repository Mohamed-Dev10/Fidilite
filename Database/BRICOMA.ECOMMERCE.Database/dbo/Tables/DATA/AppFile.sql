CREATE TABLE [dbo].[AppFile]
(
    [Id]               INT            NOT NULL IDENTITY (1, 1) PRIMARY KEY,
    [Profil_Id]        NVARCHAR (450) NULL,
    [NomPrenom]        NVARCHAR (255) NULL,
    [URL]              NVARCHAR (500) NULL,
    [DateCreation]     DATETIME       NULL,
    [UpdatedBy]        NVARCHAR(450)  NULL,
    [DateModification] DATETIME       NULL,
    CONSTRAINT [FK_AppFile_ToProfil] FOREIGN KEY ([Profil_Id]) REFERENCES [dbo].[Profil] ([Id]),
    CONSTRAINT [FK_AppFile_ToUpdatedProfil] FOREIGN KEY ([UpdatedBy]) REFERENCES [dbo].[Profil] ([Id])
)
