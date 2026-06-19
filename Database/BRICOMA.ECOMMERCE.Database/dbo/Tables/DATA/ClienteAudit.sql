CREATE TABLE [dbo].[ClienteAudit]
(
    [Id]            BIGINT         NOT NULL IDENTITY (1, 1) PRIMARY KEY,
    [Action]        NVARCHAR(MAX)  NULL,
    [ClienteCode]   NVARCHAR(255)  NULL,
    [AspNetUsersId] NVARCHAR(450)  NULL,
    [DateCreation]  DATETIME       NOT NULL,
    CONSTRAINT [FK_ClienteAudit_ToAspNetUsers] FOREIGN KEY ([AspNetUsersId]) REFERENCES [dbo].[AspNetUsers] ([Id])
)
