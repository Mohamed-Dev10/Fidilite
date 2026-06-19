CREATE TABLE [dbo].[REF_AnniversaireCarte]
(
    [Id]         INT            NOT NULL IDENTITY (1, 1) PRIMARY KEY,
    [Name]       NVARCHAR(MAX)  NOT NULL,
    [Montant]    DECIMAL        NULL,
    [EmailText]  NVARCHAR(MAX)  NULL,
    [ImageUrl]   NVARCHAR(MAX)  NULL
)
