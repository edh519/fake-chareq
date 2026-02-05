BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FinalAuthorisations]') AND [c].[name] = N'ProcessDeviationReasonId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [FinalAuthorisations] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [FinalAuthorisations] ALTER COLUMN [ProcessDeviationReasonId] int NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220824110434_HotfixProcessDeviationReasonNullable', N'6.0.6');
GO

COMMIT;
GO

