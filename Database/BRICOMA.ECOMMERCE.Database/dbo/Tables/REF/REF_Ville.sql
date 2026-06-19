CREATE TABLE [dbo].[REF_Ville]
(
    [Id]   INT            NOT NULL IDENTITY (1, 1) PRIMARY KEY,
    [Name] NVARCHAR(MAX)  NOT NULL,
    [Code] NVARCHAR(MAX)  NULL
)
