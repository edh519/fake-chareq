BEGIN TRANSACTION;
GO

ALTER TABLE [AspNetUsers] ADD [WorkRequestId] int NULL;
GO

CREATE TABLE [Label] (
    [LabelId] int NOT NULL IDENTITY,
    [LabelShort] nvarchar(max) NULL,
    [LabelDescription] nvarchar(max) NULL,
    [HexColor] nvarchar(max) NULL,
    [IsArchived] bit NOT NULL,
    [WorkRequestId] int NULL,
    CONSTRAINT [PK_Label] PRIMARY KEY ([LabelId]),
    CONSTRAINT [FK_Label_WorkRequests_WorkRequestId] FOREIGN KEY ([WorkRequestId]) REFERENCES [WorkRequests] ([WorkRequestId])
);
GO

CREATE INDEX [IX_AspNetUsers_WorkRequestId] ON [AspNetUsers] ([WorkRequestId]);
GO

CREATE INDEX [IX_Label_WorkRequestId] ON [Label] ([WorkRequestId]);
GO

ALTER TABLE [AspNetUsers] ADD CONSTRAINT [FK_AspNetUsers_WorkRequests_WorkRequestId] FOREIGN KEY ([WorkRequestId]) REFERENCES [WorkRequests] ([WorkRequestId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220824132131_AddingLabelsAndAssignees', N'6.0.6');
GO

COMMIT;
GO

