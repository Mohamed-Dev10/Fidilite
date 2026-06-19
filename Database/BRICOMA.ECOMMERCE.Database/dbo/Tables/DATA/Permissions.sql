CREATE TABLE [dbo].[Permissions]
(
    [Id]       INT            NOT NULL IDENTITY (1, 1) PRIMARY KEY,
    [Code]     NVARCHAR(255)  NOT NULL,
    [Label]    NVARCHAR(MAX)  NOT NULL,
    [Category] NVARCHAR(255)  NOT NULL
)
