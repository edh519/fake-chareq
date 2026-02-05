BEGIN TRANSACTION;
GO

ALTER TABLE [FileUploads] DROP CONSTRAINT [FK_FileUploads_ChangeRequests_ChangeRequestId];
GO

ALTER TABLE [Notifications] DROP CONSTRAINT [FK_Notifications_ChangeRequests_ChangeRequestId];
GO

DROP TABLE [ChangeRequestSignatures];
GO

DROP TABLE [DevLeadAuthSignatures];
GO

DROP TABLE [DevWorkCompleteAuthSignatures];
GO

DROP TABLE [DevWorkReleaseAuthSignatures];
GO

DROP TABLE [DevWorkReviewAuthSignatures];
GO

DROP TABLE [Rejections];
GO

DROP TABLE [DevLeadChangeAuthorisations];
GO

DROP TABLE [DevWorkCompleteAuthorisations];
GO

DROP TABLE [DevWorkReleaseAuthorisations];
GO

DROP TABLE [DevWorkReviewAuthorisations];
GO

DROP TABLE [SignatureUploads];
GO

DROP TABLE [RejectionReasonTypes];
GO

DROP TABLE [DecisionTypes];
GO

DROP TABLE [ChangeRequests];
GO

DROP TABLE [ChangeRequestStatuses];
GO

DROP TABLE [ImpactTypes];
GO

DROP TABLE [RationaleTypes];
GO

DROP TABLE [Role];
GO

DELETE FROM [EmailTypes]
WHERE [EmailTypeId] = 1;
SELECT @@ROWCOUNT;

GO

DELETE FROM [EmailTypes]
WHERE [EmailTypeId] = 2;
SELECT @@ROWCOUNT;

GO

DELETE FROM [EmailTypes]
WHERE [EmailTypeId] = 3;
SELECT @@ROWCOUNT;

GO

DELETE FROM [EmailTypes]
WHERE [EmailTypeId] = 4;
SELECT @@ROWCOUNT;

GO

DELETE FROM [EmailTypes]
WHERE [EmailTypeId] = 100;
SELECT @@ROWCOUNT;

GO

DELETE FROM [EmailTypes]
WHERE [EmailTypeId] = 101;
SELECT @@ROWCOUNT;

GO

DELETE FROM [EmailTypes]
WHERE [EmailTypeId] = 200;
SELECT @@ROWCOUNT;

GO

DELETE FROM [EmailTypes]
WHERE [EmailTypeId] = 201;
SELECT @@ROWCOUNT;

GO

DELETE FROM [EmailTypes]
WHERE [EmailTypeId] = 202;
SELECT @@ROWCOUNT;

GO

DELETE FROM [EmailTypes]
WHERE [EmailTypeId] = 300;
SELECT @@ROWCOUNT;

GO

DELETE FROM [EmailTypes]
WHERE [EmailTypeId] = 301;
SELECT @@ROWCOUNT;

GO

DELETE FROM [NotificationTypes]
WHERE [NotificationTypeId] = 201;
SELECT @@ROWCOUNT;

GO

DELETE FROM [NotificationTypes]
WHERE [NotificationTypeId] = 202;
SELECT @@ROWCOUNT;

GO

DELETE FROM [NotificationTypes]
WHERE [NotificationTypeId] = 300;
SELECT @@ROWCOUNT;

GO

DELETE FROM [NotificationTypes]
WHERE [NotificationTypeId] = 301;
SELECT @@ROWCOUNT;

GO

EXEC sp_rename N'[Notifications].[ChangeRequestId]', N'WorkRequestId', N'COLUMN';
GO

EXEC sp_rename N'[Notifications].[IX_Notifications_ChangeRequestId]', N'IX_Notifications_WorkRequestId', N'INDEX';
GO

EXEC sp_rename N'[FileUploads].[ChangeRequestId]', N'WorkRequestId', N'COLUMN';
GO

EXEC sp_rename N'[FileUploads].[IX_FileUploads_ChangeRequestId]', N'IX_FileUploads_WorkRequestId', N'INDEX';
GO

CREATE TABLE [WorkRequestStatuses] (
    [WorkRequestStatusId] int NOT NULL,
    [WorkRequestStatusName] nvarchar(max) NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_WorkRequestStatuses] PRIMARY KEY ([WorkRequestStatusId])
);
GO

CREATE TABLE [WorkRequests] (
    [WorkRequestId] int NOT NULL IDENTITY,
    [Reference] nvarchar(max) NULL,
    [StatusWorkRequestStatusId] int NULL,
    [TrialId] int NULL,
    [TrialOther] nvarchar(max) NULL,
    [CreationDateTime] datetime2 NOT NULL,
    [DetailDescription] nvarchar(max) NULL,
    [CreatedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_WorkRequests] PRIMARY KEY ([WorkRequestId]),
    CONSTRAINT [FK_WorkRequests_Trials_TrialId] FOREIGN KEY ([TrialId]) REFERENCES [Trials] ([TrialId]),
    CONSTRAINT [FK_WorkRequests_WorkRequestStatuses_StatusWorkRequestStatusId] FOREIGN KEY ([StatusWorkRequestStatusId]) REFERENCES [WorkRequestStatuses] ([WorkRequestStatusId])
);
GO

CREATE TABLE [FinalAuthorisations] (
    [FinalAuthorisationId] int NOT NULL IDENTITY,
    [WorkReference] nvarchar(max) NULL,
    [ActualTimeImpactDays] float NOT NULL,
    [CompletedBy] nvarchar(max) NULL,
    [CompletedDateTime] datetime2 NOT NULL,
    [ProcessDeviationReasonId] int NOT NULL,
    [Comments] nvarchar(max) NULL,
    [WorkRequestId] int NULL,
    CONSTRAINT [PK_FinalAuthorisations] PRIMARY KEY ([FinalAuthorisationId]),
    CONSTRAINT [FK_FinalAuthorisations_ProcessDeviationReasons_ProcessDeviationReasonId] FOREIGN KEY ([ProcessDeviationReasonId]) REFERENCES [ProcessDeviationReasons] ([ProcessDeviationReasonId]) ON DELETE CASCADE,
    CONSTRAINT [FK_FinalAuthorisations_WorkRequests_WorkRequestId] FOREIGN KEY ([WorkRequestId]) REFERENCES [WorkRequests] ([WorkRequestId])
);
GO

CREATE TABLE [InitialAuthorisations] (
    [InitialAuthorisationId] int NOT NULL IDENTITY,
    [EstimatedTimeImpact] float NOT NULL,
    [WorkRequiredDecription] nvarchar(max) NULL,
    [DecisionBy] nvarchar(max) NULL,
    [DecisionDateTime] datetime2 NOT NULL,
    [WorkRequestId] int NULL,
    CONSTRAINT [PK_InitialAuthorisations] PRIMARY KEY ([InitialAuthorisationId]),
    CONSTRAINT [FK_InitialAuthorisations_WorkRequests_WorkRequestId] FOREIGN KEY ([WorkRequestId]) REFERENCES [WorkRequests] ([WorkRequestId])
);
GO

UPDATE [NotificationTypes] SET [NotificationTypeName] = N'Work Request Approved'
WHERE [NotificationTypeId] = 1;
SELECT @@ROWCOUNT;

GO

UPDATE [NotificationTypes] SET [NotificationTypeName] = N'Request Requires Ammendments'
WHERE [NotificationTypeId] = 2;
SELECT @@ROWCOUNT;

GO

UPDATE [NotificationTypes] SET [NotificationTypeName] = N'Request Closed'
WHERE [NotificationTypeId] = 3;
SELECT @@ROWCOUNT;

GO

UPDATE [NotificationTypes] SET [NotificationTypeName] = N'Request Completed'
WHERE [NotificationTypeId] = 4;
SELECT @@ROWCOUNT;

GO

UPDATE [NotificationTypes] SET [NotificationTypeName] = N'Request Pending Initial Approval'
WHERE [NotificationTypeId] = 100;
SELECT @@ROWCOUNT;

GO

UPDATE [NotificationTypes] SET [NotificationTypeName] = N'Request Re-submitted Pending Initial Approval'
WHERE [NotificationTypeId] = 101;
SELECT @@ROWCOUNT;

GO

UPDATE [NotificationTypes] SET [NotificationTypeName] = N'Request Pending Work'
WHERE [NotificationTypeId] = 200;
SELECT @@ROWCOUNT;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'WorkRequestStatusId', N'IsActive', N'WorkRequestStatusName') AND [object_id] = OBJECT_ID(N'[WorkRequestStatuses]'))
    SET IDENTITY_INSERT [WorkRequestStatuses] ON;
INSERT INTO [WorkRequestStatuses] ([WorkRequestStatusId], [IsActive], [WorkRequestStatusName])
VALUES (10, CAST(1 AS bit), N'Pending Requester'),
(20, CAST(1 AS bit), N'Pending Initial Approval'),
(30, CAST(1 AS bit), N'Pending Work'),
(100, CAST(1 AS bit), N'Completed'),
(110, CAST(1 AS bit), N'Abandoned');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'WorkRequestStatusId', N'IsActive', N'WorkRequestStatusName') AND [object_id] = OBJECT_ID(N'[WorkRequestStatuses]'))
    SET IDENTITY_INSERT [WorkRequestStatuses] OFF;
GO

CREATE INDEX [IX_FinalAuthorisations_ProcessDeviationReasonId] ON [FinalAuthorisations] ([ProcessDeviationReasonId]);
GO

CREATE INDEX [IX_FinalAuthorisations_WorkRequestId] ON [FinalAuthorisations] ([WorkRequestId]);
GO

CREATE INDEX [IX_InitialAuthorisations_WorkRequestId] ON [InitialAuthorisations] ([WorkRequestId]);
GO

CREATE INDEX [IX_WorkRequests_StatusWorkRequestStatusId] ON [WorkRequests] ([StatusWorkRequestStatusId]);
GO

CREATE INDEX [IX_WorkRequests_TrialId] ON [WorkRequests] ([TrialId]);
GO

ALTER TABLE [FileUploads] ADD CONSTRAINT [FK_FileUploads_WorkRequests_WorkRequestId] FOREIGN KEY ([WorkRequestId]) REFERENCES [WorkRequests] ([WorkRequestId]) ON DELETE CASCADE;
GO

ALTER TABLE [Notifications] ADD CONSTRAINT [FK_Notifications_WorkRequests_WorkRequestId] FOREIGN KEY ([WorkRequestId]) REFERENCES [WorkRequests] ([WorkRequestId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220811134426_version2', N'6.0.3');
GO

COMMIT;
GO

