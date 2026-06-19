CREATE TABLE [dbo].[REF_ClienteType]
(
    [Id]       INT            NOT NULL IDENTITY (1, 1) PRIMARY KEY,
    [Name]     NVARCHAR(MAX)  NOT NULL,
    [Montant]  DECIMAL        NULL,
    [ImageUrl] NVARCHAR(MAX)  NULL
)
