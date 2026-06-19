CREATE TABLE [dbo].[REF_Metier]
(
    [Id]   INT            NOT NULL IDENTITY (1, 1) PRIMARY KEY,
    [Name] NVARCHAR(MAX)  NOT NULL,
    [Code] NVARCHAR(MAX)  NULL
)
