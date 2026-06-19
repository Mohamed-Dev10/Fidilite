CREATE TABLE [dbo].[MAP_Cliente_AppFile]
(
    [ClienteId] BIGINT NOT NULL,
    [AppFileId] INT    NOT NULL,
    CONSTRAINT [PK_MAP_Cliente_AppFile] PRIMARY KEY ([ClienteId], [AppFileId]),
    CONSTRAINT [FK_MAP_Cliente_AppFile_Cliente] FOREIGN KEY ([ClienteId]) REFERENCES [dbo].[Cliente] ([Id]),
    CONSTRAINT [FK_MAP_Cliente_AppFile_AppFile] FOREIGN KEY ([AppFileId]) REFERENCES [dbo].[AppFile] ([Id])
)
