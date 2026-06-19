CREATE TABLE [dbo].[ClienteEvent] (
    [Id]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [NewStatusId]      INT            NOT NULL,
    [PreviousStatusId] INT            NULL,
    [AspNetUsersId]    NVARCHAR (450) NULL,
    [ClienteCode]      NVARCHAR (255) NULL,
    [DateCreation]     DATETIME       NOT NULL,
    CONSTRAINT [PK_ClienteEvent] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ClienteEvent_ToAspNetUsers] FOREIGN KEY ([AspNetUsersId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_ClienteEvent_ToREF_ClienteStatutNewStatus] FOREIGN KEY ([NewStatusId]) REFERENCES [dbo].[REF_ClienteStatut] ([Id]),
    CONSTRAINT [FK_ClienteEvent_ToREF_ClienteStatutPreviousStatus] FOREIGN KEY ([PreviousStatusId]) REFERENCES [dbo].[REF_ClienteStatut] ([Id])
);
