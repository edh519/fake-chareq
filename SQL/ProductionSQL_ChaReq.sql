IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [ChangeRequestStatuses] (
        [ChangeRequestStatusId] int NOT NULL,
        [ChangeRequestStatusName] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_ChangeRequestStatuses] PRIMARY KEY ([ChangeRequestStatusId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [DecisionTypes] (
        [DecisionTypeId] int NOT NULL,
        [DecisionTypeName] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_DecisionTypes] PRIMARY KEY ([DecisionTypeId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [EmailTypes] (
        [EmailTypeId] int NOT NULL,
        [EmailTypeName] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_EmailTypes] PRIMARY KEY ([EmailTypeId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [ImpactTypes] (
        [ImpactTypeId] int NOT NULL,
        [ImpactTypeName] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_ImpactTypes] PRIMARY KEY ([ImpactTypeId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [NotificationTypes] (
        [NotificationTypeId] int NOT NULL,
        [NotificationTypeName] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_NotificationTypes] PRIMARY KEY ([NotificationTypeId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [ProcessDeviationReasons] (
        [ProcessDeviationReasonId] int NOT NULL IDENTITY,
        [Reason] varchar(MAX) NOT NULL,
        CONSTRAINT [PK_ProcessDeviationReasons] PRIMARY KEY ([ProcessDeviationReasonId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [RationaleTypes] (
        [RationaleTypeId] int NOT NULL,
        [RationaleTypeName] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_RationaleTypes] PRIMARY KEY ([RationaleTypeId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [RejectionReasonTypes] (
        [RejectionReasonTypeId] int NOT NULL,
        [RejectionReasonTypeName] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [CausesArchive] bit NOT NULL,
        CONSTRAINT [PK_RejectionReasonTypes] PRIMARY KEY ([RejectionReasonTypeId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [Role] (
        [RoleId] int NOT NULL IDENTITY,
        [RoleName] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Role] PRIMARY KEY ([RoleId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [SignatureUploads] (
        [SignatureUploadId] int NOT NULL IDENTITY,
        [FileName] nvarchar(max) NULL,
        [File] varbinary(max) NULL,
        [FileUploadDateTime] datetime2 NOT NULL,
        [FileHash] nvarchar(max) NULL,
        [UploadedBy] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_SignatureUploads] PRIMARY KEY ([SignatureUploadId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [Trials] (
        [TrialId] int NOT NULL IDENTITY,
        [TrialName] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Trials] PRIMARY KEY ([TrialId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(128) NOT NULL,
        [ProviderKey] nvarchar(128) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(128) NOT NULL,
        [Name] nvarchar(128) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [ChangeRequests] (
        [ChangeRequestId] int NOT NULL IDENTITY,
        [Reference] nvarchar(max) NULL,
        [StatusChangeRequestStatusId] int NULL,
        [HasActiveRejection] bit NOT NULL,
        [TrialId] int NULL,
        [TrialOther] nvarchar(max) NULL,
        [RoleId] int NULL,
        [RoleOther] nvarchar(max) NULL,
        [CreationDateTime] datetime2 NOT NULL,
        [DetailDescription] nvarchar(max) NULL,
        [ImpactTypeId] int NULL,
        [RationaleTypeId] int NULL,
        [ReasonForChange] nvarchar(max) NULL,
        [CreatedBy] nvarchar(max) NULL,
        [AdditionalComments] nvarchar(max) NULL,
        [TitleSubject] VARCHAR(255) NULL,
        CONSTRAINT [PK_ChangeRequests] PRIMARY KEY ([ChangeRequestId]),
        CONSTRAINT [FK_ChangeRequests_ChangeRequestStatuses_StatusChangeRequestStatusId] FOREIGN KEY ([StatusChangeRequestStatusId]) REFERENCES [ChangeRequestStatuses] ([ChangeRequestStatusId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_ChangeRequests_ImpactTypes_ImpactTypeId] FOREIGN KEY ([ImpactTypeId]) REFERENCES [ImpactTypes] ([ImpactTypeId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_ChangeRequests_RationaleTypes_RationaleTypeId] FOREIGN KEY ([RationaleTypeId]) REFERENCES [RationaleTypes] ([RationaleTypeId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_ChangeRequests_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Role] ([RoleId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_ChangeRequests_Trials_TrialId] FOREIGN KEY ([TrialId]) REFERENCES [Trials] ([TrialId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [ChangeRequestSignatures] (
        [ChangeRequestsChangeRequestId] int NOT NULL,
        [SignaturesSignatureUploadId] int NOT NULL,
        CONSTRAINT [PK_ChangeRequestSignatures] PRIMARY KEY ([ChangeRequestsChangeRequestId], [SignaturesSignatureUploadId]),
        CONSTRAINT [FK_ChangeRequestSignatures_ChangeRequests_ChangeRequestsChangeRequestId] FOREIGN KEY ([ChangeRequestsChangeRequestId]) REFERENCES [ChangeRequests] ([ChangeRequestId]) ON DELETE CASCADE,
        CONSTRAINT [FK_ChangeRequestSignatures_SignatureUploads_SignaturesSignatureUploadId] FOREIGN KEY ([SignaturesSignatureUploadId]) REFERENCES [SignatureUploads] ([SignatureUploadId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [DevLeadChangeAuthorisations] (
        [DevLeadChangeAuthorisationId] int NOT NULL IDENTITY,
        [EstimatedTimeImpact] float NOT NULL,
        [ChangeRequiredDecription] nvarchar(max) NULL,
        [DecisionTypeId] int NULL,
        [DecisionExplanation] nvarchar(max) NULL,
        [DecisionBy] nvarchar(max) NULL,
        [DecisionDateTime] datetime2 NOT NULL,
        [AdditionalComments] nvarchar(max) NULL,
        [ChangeRequestId] int NULL,
        CONSTRAINT [PK_DevLeadChangeAuthorisations] PRIMARY KEY ([DevLeadChangeAuthorisationId]),
        CONSTRAINT [FK_DevLeadChangeAuthorisations_ChangeRequests_ChangeRequestId] FOREIGN KEY ([ChangeRequestId]) REFERENCES [ChangeRequests] ([ChangeRequestId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_DevLeadChangeAuthorisations_DecisionTypes_DecisionTypeId] FOREIGN KEY ([DecisionTypeId]) REFERENCES [DecisionTypes] ([DecisionTypeId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [DevWorkCompleteAuthorisations] (
        [DevWorkCompleteAuthorisationId] int NOT NULL IDENTITY,
        [CommitReference] nvarchar(max) NULL,
        [ActualTimeImpactDays] float NOT NULL,
        [ChangeDescription] nvarchar(max) NULL,
        [CompletedBy] nvarchar(max) NULL,
        [CompletedDateTime] datetime2 NOT NULL,
        [AdditionalComments] nvarchar(max) NULL,
        [ProcessDeviationReasonId] int NOT NULL,
        [ChangeRequestId] int NULL,
        CONSTRAINT [PK_DevWorkCompleteAuthorisations] PRIMARY KEY ([DevWorkCompleteAuthorisationId]),
        CONSTRAINT [FK_DevWorkCompleteAuthorisations_ChangeRequests_ChangeRequestId] FOREIGN KEY ([ChangeRequestId]) REFERENCES [ChangeRequests] ([ChangeRequestId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_DevWorkCompleteAuthorisations_ProcessDeviationReasons_ProcessDeviationReasonId] FOREIGN KEY ([ProcessDeviationReasonId]) REFERENCES [ProcessDeviationReasons] ([ProcessDeviationReasonId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [DevWorkReleaseAuthorisations] (
        [DevWorkReleaseAuthorisationId] int NOT NULL IDENTITY,
        [ReleasedBy] nvarchar(max) NULL,
        [ReleasedDate] datetime2 NOT NULL,
        [AdditionalComments] nvarchar(max) NULL,
        [ProcessDeviationReasonId] int NOT NULL,
        [ChangeRequestId] int NULL,
        CONSTRAINT [PK_DevWorkReleaseAuthorisations] PRIMARY KEY ([DevWorkReleaseAuthorisationId]),
        CONSTRAINT [FK_DevWorkReleaseAuthorisations_ChangeRequests_ChangeRequestId] FOREIGN KEY ([ChangeRequestId]) REFERENCES [ChangeRequests] ([ChangeRequestId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_DevWorkReleaseAuthorisations_ProcessDeviationReasons_ProcessDeviationReasonId] FOREIGN KEY ([ProcessDeviationReasonId]) REFERENCES [ProcessDeviationReasons] ([ProcessDeviationReasonId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [DevWorkReviewAuthorisations] (
        [DevWorkReviewAuthorisationId] int NOT NULL IDENTITY,
        [ReviewedBy] nvarchar(max) NULL,
        [ReviewedDateTime] datetime2 NOT NULL,
        [AdditionalComments] nvarchar(max) NULL,
        [ProcessDeviationReasonId] int NOT NULL,
        [ChangeRequestId] int NULL,
        CONSTRAINT [PK_DevWorkReviewAuthorisations] PRIMARY KEY ([DevWorkReviewAuthorisationId]),
        CONSTRAINT [FK_DevWorkReviewAuthorisations_ChangeRequests_ChangeRequestId] FOREIGN KEY ([ChangeRequestId]) REFERENCES [ChangeRequests] ([ChangeRequestId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_DevWorkReviewAuthorisations_ProcessDeviationReasons_ProcessDeviationReasonId] FOREIGN KEY ([ProcessDeviationReasonId]) REFERENCES [ProcessDeviationReasons] ([ProcessDeviationReasonId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [FileUploads] (
        [FileUploadId] int NOT NULL IDENTITY,
        [FileName] nvarchar(max) NULL,
        [ReadableFileName] nvarchar(max) NULL,
        [File] varbinary(max) NULL,
        [FileUploadDateTime] datetime2 NOT NULL,
        [FileHash] nvarchar(max) NULL,
        [UploadedBy] nvarchar(max) NULL,
        [ChangeRequestId] int NOT NULL,
        CONSTRAINT [PK_FileUploads] PRIMARY KEY ([FileUploadId]),
        CONSTRAINT [FK_FileUploads_ChangeRequests_ChangeRequestId] FOREIGN KEY ([ChangeRequestId]) REFERENCES [ChangeRequests] ([ChangeRequestId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [Notifications] (
        [NotificationId] int NOT NULL IDENTITY,
        [NotificationTypeId] int NULL,
        [ChangeRequestId] int NULL,
        [SentDate] datetime2 NOT NULL,
        [Recipient] nvarchar(max) NULL,
        [SeenDate] datetime2 NULL,
        CONSTRAINT [PK_Notifications] PRIMARY KEY ([NotificationId]),
        CONSTRAINT [FK_Notifications_ChangeRequests_ChangeRequestId] FOREIGN KEY ([ChangeRequestId]) REFERENCES [ChangeRequests] ([ChangeRequestId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Notifications_NotificationTypes_NotificationTypeId] FOREIGN KEY ([NotificationTypeId]) REFERENCES [NotificationTypes] ([NotificationTypeId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [Rejections] (
        [RejectionId] int NOT NULL IDENTITY,
        [RejectionReasonTypeId] int NULL,
        [IsRejectionActive] bit NOT NULL,
        [RejectedBy] nvarchar(max) NULL,
        [RejectedDate] datetime2 NOT NULL,
        [Message] nvarchar(max) NULL,
        [ChangeRequestId] int NULL,
        CONSTRAINT [PK_Rejections] PRIMARY KEY ([RejectionId]),
        CONSTRAINT [FK_Rejections_ChangeRequests_ChangeRequestId] FOREIGN KEY ([ChangeRequestId]) REFERENCES [ChangeRequests] ([ChangeRequestId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Rejections_RejectionReasonTypes_RejectionReasonTypeId] FOREIGN KEY ([RejectionReasonTypeId]) REFERENCES [RejectionReasonTypes] ([RejectionReasonTypeId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [DevLeadAuthSignatures] (
        [SignaturesSignatureUploadId] int NOT NULL,
        [devLeadChangeAuthorisationsDevLeadChangeAuthorisationId] int NOT NULL,
        CONSTRAINT [PK_DevLeadAuthSignatures] PRIMARY KEY ([SignaturesSignatureUploadId], [devLeadChangeAuthorisationsDevLeadChangeAuthorisationId]),
        CONSTRAINT [FK_DevLeadAuthSignatures_DevLeadChangeAuthorisations_devLeadChangeAuthorisationsDevLeadChangeAuthorisationId] FOREIGN KEY ([devLeadChangeAuthorisationsDevLeadChangeAuthorisationId]) REFERENCES [DevLeadChangeAuthorisations] ([DevLeadChangeAuthorisationId]) ON DELETE CASCADE,
        CONSTRAINT [FK_DevLeadAuthSignatures_SignatureUploads_SignaturesSignatureUploadId] FOREIGN KEY ([SignaturesSignatureUploadId]) REFERENCES [SignatureUploads] ([SignatureUploadId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [DevWorkCompleteAuthSignatures] (
        [SignaturesSignatureUploadId] int NOT NULL,
        [devWorkCompleteAuthorisationsDevWorkCompleteAuthorisationId] int NOT NULL,
        CONSTRAINT [PK_DevWorkCompleteAuthSignatures] PRIMARY KEY ([SignaturesSignatureUploadId], [devWorkCompleteAuthorisationsDevWorkCompleteAuthorisationId]),
        CONSTRAINT [FK_DevWorkCompleteAuthSignatures_DevWorkCompleteAuthorisations_devWorkCompleteAuthorisationsDevWorkCompleteAuthorisationId] FOREIGN KEY ([devWorkCompleteAuthorisationsDevWorkCompleteAuthorisationId]) REFERENCES [DevWorkCompleteAuthorisations] ([DevWorkCompleteAuthorisationId]) ON DELETE CASCADE,
        CONSTRAINT [FK_DevWorkCompleteAuthSignatures_SignatureUploads_SignaturesSignatureUploadId] FOREIGN KEY ([SignaturesSignatureUploadId]) REFERENCES [SignatureUploads] ([SignatureUploadId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [DevWorkReleaseAuthSignatures] (
        [SignaturesSignatureUploadId] int NOT NULL,
        [devWorkReleaseAuthorisationsDevWorkReleaseAuthorisationId] int NOT NULL,
        CONSTRAINT [PK_DevWorkReleaseAuthSignatures] PRIMARY KEY ([SignaturesSignatureUploadId], [devWorkReleaseAuthorisationsDevWorkReleaseAuthorisationId]),
        CONSTRAINT [FK_DevWorkReleaseAuthSignatures_DevWorkReleaseAuthorisations_devWorkReleaseAuthorisationsDevWorkReleaseAuthorisationId] FOREIGN KEY ([devWorkReleaseAuthorisationsDevWorkReleaseAuthorisationId]) REFERENCES [DevWorkReleaseAuthorisations] ([DevWorkReleaseAuthorisationId]) ON DELETE CASCADE,
        CONSTRAINT [FK_DevWorkReleaseAuthSignatures_SignatureUploads_SignaturesSignatureUploadId] FOREIGN KEY ([SignaturesSignatureUploadId]) REFERENCES [SignatureUploads] ([SignatureUploadId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [DevWorkReviewAuthSignatures] (
        [SignaturesSignatureUploadId] int NOT NULL,
        [devWorkReviewAuthorisationsDevWorkReviewAuthorisationId] int NOT NULL,
        CONSTRAINT [PK_DevWorkReviewAuthSignatures] PRIMARY KEY ([SignaturesSignatureUploadId], [devWorkReviewAuthorisationsDevWorkReviewAuthorisationId]),
        CONSTRAINT [FK_DevWorkReviewAuthSignatures_DevWorkReviewAuthorisations_devWorkReviewAuthorisationsDevWorkReviewAuthorisationId] FOREIGN KEY ([devWorkReviewAuthorisationsDevWorkReviewAuthorisationId]) REFERENCES [DevWorkReviewAuthorisations] ([DevWorkReviewAuthorisationId]) ON DELETE CASCADE,
        CONSTRAINT [FK_DevWorkReviewAuthSignatures_SignatureUploads_SignaturesSignatureUploadId] FOREIGN KEY ([SignaturesSignatureUploadId]) REFERENCES [SignatureUploads] ([SignatureUploadId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE TABLE [SentEmails] (
        [SentEmailId] int NOT NULL IDENTITY,
        [EmailTypeId] int NULL,
        [EmailSentDateTime] datetime2 NOT NULL,
        [Recipient] nvarchar(max) NULL,
        [ReplyTo] nvarchar(max) NULL,
        [From] nvarchar(max) NULL,
        [Subject] nvarchar(max) NULL,
        [HtmlBody] nvarchar(max) NULL,
        [PlainTextBody] nvarchar(max) NULL,
        [NotificationId] int NULL,
        CONSTRAINT [PK_SentEmails] PRIMARY KEY ([SentEmailId]),
        CONSTRAINT [FK_SentEmails_EmailTypes_EmailTypeId] FOREIGN KEY ([EmailTypeId]) REFERENCES [EmailTypes] ([EmailTypeId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_SentEmails_Notifications_NotificationId] FOREIGN KEY ([NotificationId]) REFERENCES [Notifications] ([NotificationId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ChangeRequestStatusId', N'ChangeRequestStatusName', N'IsActive') AND [object_id] = OBJECT_ID(N'[ChangeRequestStatuses]'))
        SET IDENTITY_INSERT [ChangeRequestStatuses] ON;
    EXEC(N'INSERT INTO [ChangeRequestStatuses] ([ChangeRequestStatusId], [ChangeRequestStatusName], [IsActive])
    VALUES (10, N''Pending Requester'', CAST(1 AS bit)),
    (20, N''Pending Initial Approval'', CAST(1 AS bit)),
    (30, N''Pending Development Work'', CAST(1 AS bit)),
    (40, N''Pending Peer Review'', CAST(1 AS bit)),
    (50, N''Pending Release'', CAST(1 AS bit)),
    (60, N''Completed'', CAST(1 AS bit)),
    (70, N''Abandoned'', CAST(1 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ChangeRequestStatusId', N'ChangeRequestStatusName', N'IsActive') AND [object_id] = OBJECT_ID(N'[ChangeRequestStatuses]'))
        SET IDENTITY_INSERT [ChangeRequestStatuses] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'DecisionTypeId', N'DecisionTypeName', N'IsActive') AND [object_id] = OBJECT_ID(N'[DecisionTypes]'))
        SET IDENTITY_INSERT [DecisionTypes] ON;
    EXEC(N'INSERT INTO [DecisionTypes] ([DecisionTypeId], [DecisionTypeName], [IsActive])
    VALUES (10, N''Approved'', CAST(1 AS bit)),
    (20, N''Rejected with Amendments'', CAST(1 AS bit)),
    (30, N''Rejected and Abandoned'', CAST(1 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'DecisionTypeId', N'DecisionTypeName', N'IsActive') AND [object_id] = OBJECT_ID(N'[DecisionTypes]'))
        SET IDENTITY_INSERT [DecisionTypes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'EmailTypeId', N'EmailTypeName', N'IsActive') AND [object_id] = OBJECT_ID(N'[EmailTypes]'))
        SET IDENTITY_INSERT [EmailTypes] ON;
    EXEC(N'INSERT INTO [EmailTypes] ([EmailTypeId], [EmailTypeName], [IsActive])
    VALUES (301, N''ChangeRequestPeerReviewDeclinedToDev'', CAST(1 AS bit)),
    (300, N''ChangeRequestPeerReviewApprovalToDev'', CAST(1 AS bit)),
    (201, N''ChangeRequestPendingPeerReviewToDevTeam'', CAST(1 AS bit)),
    (200, N''ChangeRequestPendingDevWorkToDevTeam'', CAST(1 AS bit)),
    (101, N''ChangeRequestPendingSubsequentApprovalToDevLead'', CAST(1 AS bit)),
    (202, N''ChangeRequestPendingReleaseToDevTeam'', CAST(1 AS bit)),
    (4, N''ChangeRequestCompletedToRequester'', CAST(1 AS bit)),
    (3, N''ChangeRequestDeclinedAndAbandonedToRequester'', CAST(1 AS bit)),
    (2, N''ChangeRequestDeclinedWithAmendmentsToRequester'', CAST(1 AS bit)),
    (1, N''ChangeRequestApprovedToRequester'', CAST(1 AS bit)),
    (100, N''ChangeRequestPendingInitialApprovalToDevLead'', CAST(1 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'EmailTypeId', N'EmailTypeName', N'IsActive') AND [object_id] = OBJECT_ID(N'[EmailTypes]'))
        SET IDENTITY_INSERT [EmailTypes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ImpactTypeId', N'ImpactTypeName', N'IsActive') AND [object_id] = OBJECT_ID(N'[ImpactTypes]'))
        SET IDENTITY_INSERT [ImpactTypes] ON;
    EXEC(N'INSERT INTO [ImpactTypes] ([ImpactTypeId], [ImpactTypeName], [IsActive])
    VALUES (10, N''Critical'', CAST(1 AS bit)),
    (20, N''High'', CAST(1 AS bit)),
    (30, N''Medium'', CAST(1 AS bit)),
    (40, N''Low'', CAST(1 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'ImpactTypeId', N'ImpactTypeName', N'IsActive') AND [object_id] = OBJECT_ID(N'[ImpactTypes]'))
        SET IDENTITY_INSERT [ImpactTypes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'NotificationTypeId', N'IsActive', N'NotificationTypeName') AND [object_id] = OBJECT_ID(N'[NotificationTypes]'))
        SET IDENTITY_INSERT [NotificationTypes] ON;
    EXEC(N'INSERT INTO [NotificationTypes] ([NotificationTypeId], [IsActive], [NotificationTypeName])
    VALUES (301, CAST(1 AS bit), N''Change Request Pending Peer Review Declined''),
    (300, CAST(1 AS bit), N''Change Request Pending Peer Review''),
    (202, CAST(1 AS bit), N''Change Request Pending Release''),
    (201, CAST(1 AS bit), N''Change Request Pending Peer Review''),
    (200, CAST(1 AS bit), N''Change Request Pending Development Work''),
    (2, CAST(1 AS bit), N''Change Request Declined with Amendments''),
    (100, CAST(1 AS bit), N''Change Request Pending Initial Approval''),
    (4, CAST(1 AS bit), N''Change Request Completed''),
    (3, CAST(1 AS bit), N''Change Request Decline and Abandoned''),
    (1, CAST(1 AS bit), N''Change Request Approved''),
    (101, CAST(1 AS bit), N''Change Request Pending Subsequent Approval'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'NotificationTypeId', N'IsActive', N'NotificationTypeName') AND [object_id] = OBJECT_ID(N'[NotificationTypes]'))
        SET IDENTITY_INSERT [NotificationTypes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RationaleTypeId', N'IsActive', N'RationaleTypeName') AND [object_id] = OBJECT_ID(N'[RationaleTypes]'))
        SET IDENTITY_INSERT [RationaleTypes] ON;
    EXEC(N'INSERT INTO [RationaleTypes] ([RationaleTypeId], [IsActive], [RationaleTypeName])
    VALUES (4, CAST(1 AS bit), N''New Feature''),
    (3, CAST(1 AS bit), N''Missing Feature''),
    (1, CAST(1 AS bit), N''Change''),
    (2, CAST(1 AS bit), N''Bug'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RationaleTypeId', N'IsActive', N'RationaleTypeName') AND [object_id] = OBJECT_ID(N'[RationaleTypes]'))
        SET IDENTITY_INSERT [RationaleTypes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RejectionReasonTypeId', N'CausesArchive', N'IsActive', N'RejectionReasonTypeName') AND [object_id] = OBJECT_ID(N'[RejectionReasonTypes]'))
        SET IDENTITY_INSERT [RejectionReasonTypes] ON;
    EXEC(N'INSERT INTO [RejectionReasonTypes] ([RejectionReasonTypeId], [CausesArchive], [IsActive], [RejectionReasonTypeName])
    VALUES (1, CAST(0 AS bit), CAST(1 AS bit), N''More Detail Needed''),
    (2, CAST(0 AS bit), CAST(1 AS bit), N''Incorrect Details'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RejectionReasonTypeId', N'CausesArchive', N'IsActive', N'RejectionReasonTypeName') AND [object_id] = OBJECT_ID(N'[RejectionReasonTypes]'))
        SET IDENTITY_INSERT [RejectionReasonTypes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RejectionReasonTypeId', N'CausesArchive', N'IsActive', N'RejectionReasonTypeName') AND [object_id] = OBJECT_ID(N'[RejectionReasonTypes]'))
        SET IDENTITY_INSERT [RejectionReasonTypes] ON;
    EXEC(N'INSERT INTO [RejectionReasonTypes] ([RejectionReasonTypeId], [CausesArchive], [IsActive], [RejectionReasonTypeName])
    VALUES (100, CAST(1 AS bit), CAST(1 AS bit), N''User Error''),
    (101, CAST(1 AS bit), CAST(1 AS bit), N''Duplicate Request''),
    (102, CAST(1 AS bit), CAST(1 AS bit), N''Already Fixed'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RejectionReasonTypeId', N'CausesArchive', N'IsActive', N'RejectionReasonTypeName') AND [object_id] = OBJECT_ID(N'[RejectionReasonTypes]'))
        SET IDENTITY_INSERT [RejectionReasonTypes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'IsActive', N'RoleName') AND [object_id] = OBJECT_ID(N'[Role]'))
        SET IDENTITY_INSERT [Role] ON;
    EXEC(N'INSERT INTO [Role] ([RoleId], [IsActive], [RoleName])
    VALUES (1000, CAST(1 AS bit), N''Other''),
    (1, CAST(1 AS bit), N''Trial Coordinator''),
    (2, CAST(1 AS bit), N''Research Staff''),
    (3, CAST(1 AS bit), N''Chief Investigator''),
    (4, CAST(1 AS bit), N''Data Administrator'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'IsActive', N'RoleName') AND [object_id] = OBJECT_ID(N'[Role]'))
        SET IDENTITY_INSERT [Role] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'TrialId', N'IsActive', N'TrialName') AND [object_id] = OBJECT_ID(N'[Trials]'))
        SET IDENTITY_INSERT [Trials] ON;
    EXEC(N'INSERT INTO [Trials] ([TrialId], [IsActive], [TrialName])
    VALUES (15, CAST(1 AS bit), N''PACT''),
    (16, CAST(1 AS bit), N''ProFHER-2''),
    (17, CAST(1 AS bit), N''SHEDSSc''),
    (18, CAST(1 AS bit), N''SNAP2''),
    (23, CAST(1 AS bit), N''VenUS6''),
    (20, CAST(1 AS bit), N''_SOPManager''),
    (21, CAST(1 AS bit), N''STEPFORWARD''),
    (22, CAST(1 AS bit), N''SWHSI-II''),
    (14, CAST(1 AS bit), N''OSTRICH''),
    (19, CAST(1 AS bit), N''SOFFT''),
    (13, CAST(1 AS bit), N''MODS''),
    (7, CAST(1 AS bit), N''DIAMONDS''),
    (11, CAST(1 AS bit), N''GYY''),
    (10, CAST(1 AS bit), N''Firefli''),
    (9, CAST(1 AS bit), N''_ETMA''),
    (8, CAST(1 AS bit), N''DISC [CTIMP]''),
    (6, CAST(1 AS bit), N''_ChangeRequest''),
    (5, CAST(1 AS bit), N''BRIGHT''),
    (4, CAST(1 AS bit), N''BATH-OUT-2''),
    (3, CAST(1 AS bit), N''BASIL''),
    (2, CAST(1 AS bit), N''ASSSIST-2''),
    (1, CAST(1 AS bit), N''ACTIVE''),
    (12, CAST(1 AS bit), N''L1FE''),
    (1000, CAST(1 AS bit), N''Other'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'TrialId', N'IsActive', N'TrialName') AND [object_id] = OBJECT_ID(N'[Trials]'))
        SET IDENTITY_INSERT [Trials] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_ChangeRequests_ImpactTypeId] ON [ChangeRequests] ([ImpactTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_ChangeRequests_RationaleTypeId] ON [ChangeRequests] ([RationaleTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_ChangeRequests_RoleId] ON [ChangeRequests] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_ChangeRequests_StatusChangeRequestStatusId] ON [ChangeRequests] ([StatusChangeRequestStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_ChangeRequests_TrialId] ON [ChangeRequests] ([TrialId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_ChangeRequestSignatures_SignaturesSignatureUploadId] ON [ChangeRequestSignatures] ([SignaturesSignatureUploadId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_DevLeadAuthSignatures_devLeadChangeAuthorisationsDevLeadChangeAuthorisationId] ON [DevLeadAuthSignatures] ([devLeadChangeAuthorisationsDevLeadChangeAuthorisationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_DevLeadChangeAuthorisations_ChangeRequestId] ON [DevLeadChangeAuthorisations] ([ChangeRequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_DevLeadChangeAuthorisations_DecisionTypeId] ON [DevLeadChangeAuthorisations] ([DecisionTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_DevWorkCompleteAuthorisations_ChangeRequestId] ON [DevWorkCompleteAuthorisations] ([ChangeRequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_DevWorkCompleteAuthorisations_ProcessDeviationReasonId] ON [DevWorkCompleteAuthorisations] ([ProcessDeviationReasonId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_DevWorkCompleteAuthSignatures_devWorkCompleteAuthorisationsDevWorkCompleteAuthorisationId] ON [DevWorkCompleteAuthSignatures] ([devWorkCompleteAuthorisationsDevWorkCompleteAuthorisationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_DevWorkReleaseAuthorisations_ChangeRequestId] ON [DevWorkReleaseAuthorisations] ([ChangeRequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_DevWorkReleaseAuthorisations_ProcessDeviationReasonId] ON [DevWorkReleaseAuthorisations] ([ProcessDeviationReasonId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_DevWorkReleaseAuthSignatures_devWorkReleaseAuthorisationsDevWorkReleaseAuthorisationId] ON [DevWorkReleaseAuthSignatures] ([devWorkReleaseAuthorisationsDevWorkReleaseAuthorisationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_DevWorkReviewAuthorisations_ChangeRequestId] ON [DevWorkReviewAuthorisations] ([ChangeRequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_DevWorkReviewAuthorisations_ProcessDeviationReasonId] ON [DevWorkReviewAuthorisations] ([ProcessDeviationReasonId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_DevWorkReviewAuthSignatures_devWorkReviewAuthorisationsDevWorkReviewAuthorisationId] ON [DevWorkReviewAuthSignatures] ([devWorkReviewAuthorisationsDevWorkReviewAuthorisationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_FileUploads_ChangeRequestId] ON [FileUploads] ([ChangeRequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_Notifications_ChangeRequestId] ON [Notifications] ([ChangeRequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_Notifications_NotificationTypeId] ON [Notifications] ([NotificationTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_Rejections_ChangeRequestId] ON [Rejections] ([ChangeRequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_Rejections_RejectionReasonTypeId] ON [Rejections] ([RejectionReasonTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_SentEmails_EmailTypeId] ON [SentEmails] ([EmailTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    CREATE INDEX [IX_SentEmails_NotificationId] ON [SentEmails] ([NotificationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220112140129_Initial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220112140129_Initial', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    ALTER TABLE [FileUploads] DROP CONSTRAINT [FK_FileUploads_ChangeRequests_ChangeRequestId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    ALTER TABLE [Notifications] DROP CONSTRAINT [FK_Notifications_ChangeRequests_ChangeRequestId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    DROP TABLE [ChangeRequestSignatures];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    DROP TABLE [DevLeadAuthSignatures];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    DROP TABLE [DevWorkCompleteAuthSignatures];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    DROP TABLE [DevWorkReleaseAuthSignatures];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    DROP TABLE [DevWorkReviewAuthSignatures];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    DROP TABLE [Rejections];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    DROP TABLE [DevLeadChangeAuthorisations];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    DROP TABLE [DevWorkCompleteAuthorisations];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    DROP TABLE [DevWorkReleaseAuthorisations];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    DROP TABLE [DevWorkReviewAuthorisations];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    DROP TABLE [SignatureUploads];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    DROP TABLE [RejectionReasonTypes];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    DROP TABLE [DecisionTypes];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    DROP TABLE [ChangeRequests];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    DROP TABLE [ChangeRequestStatuses];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    DROP TABLE [ImpactTypes];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    DROP TABLE [RationaleTypes];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    DROP TABLE [Role];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC(N'DELETE FROM [EmailTypes]
    WHERE [EmailTypeId] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC(N'DELETE FROM [EmailTypes]
    WHERE [EmailTypeId] = 2;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC(N'DELETE FROM [EmailTypes]
    WHERE [EmailTypeId] = 3;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC(N'DELETE FROM [EmailTypes]
    WHERE [EmailTypeId] = 4;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC(N'DELETE FROM [EmailTypes]
    WHERE [EmailTypeId] = 100;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC(N'DELETE FROM [EmailTypes]
    WHERE [EmailTypeId] = 101;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC(N'DELETE FROM [EmailTypes]
    WHERE [EmailTypeId] = 200;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC(N'DELETE FROM [EmailTypes]
    WHERE [EmailTypeId] = 201;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC(N'DELETE FROM [EmailTypes]
    WHERE [EmailTypeId] = 202;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC(N'DELETE FROM [EmailTypes]
    WHERE [EmailTypeId] = 300;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC(N'DELETE FROM [EmailTypes]
    WHERE [EmailTypeId] = 301;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC(N'DELETE FROM [NotificationTypes]
    WHERE [NotificationTypeId] = 201;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC(N'DELETE FROM [NotificationTypes]
    WHERE [NotificationTypeId] = 202;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC(N'DELETE FROM [NotificationTypes]
    WHERE [NotificationTypeId] = 300;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC(N'DELETE FROM [NotificationTypes]
    WHERE [NotificationTypeId] = 301;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC sp_rename N'[Notifications].[ChangeRequestId]', N'WorkRequestId', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC sp_rename N'[Notifications].[IX_Notifications_ChangeRequestId]', N'IX_Notifications_WorkRequestId', N'INDEX';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC sp_rename N'[FileUploads].[ChangeRequestId]', N'WorkRequestId', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC sp_rename N'[FileUploads].[IX_FileUploads_ChangeRequestId]', N'IX_FileUploads_WorkRequestId', N'INDEX';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    CREATE TABLE [WorkRequestStatuses] (
        [WorkRequestStatusId] int NOT NULL,
        [WorkRequestStatusName] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_WorkRequestStatuses] PRIMARY KEY ([WorkRequestStatusId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
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
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
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
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
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
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC(N'UPDATE [NotificationTypes] SET [NotificationTypeName] = N''Work Request Approved''
    WHERE [NotificationTypeId] = 1;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC(N'UPDATE [NotificationTypes] SET [NotificationTypeName] = N''Request Requires Ammendments''
    WHERE [NotificationTypeId] = 2;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC(N'UPDATE [NotificationTypes] SET [NotificationTypeName] = N''Request Closed''
    WHERE [NotificationTypeId] = 3;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC(N'UPDATE [NotificationTypes] SET [NotificationTypeName] = N''Request Completed''
    WHERE [NotificationTypeId] = 4;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC(N'UPDATE [NotificationTypes] SET [NotificationTypeName] = N''Request Pending Initial Approval''
    WHERE [NotificationTypeId] = 100;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC(N'UPDATE [NotificationTypes] SET [NotificationTypeName] = N''Request Re-submitted Pending Initial Approval''
    WHERE [NotificationTypeId] = 101;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    EXEC(N'UPDATE [NotificationTypes] SET [NotificationTypeName] = N''Request Pending Work''
    WHERE [NotificationTypeId] = 200;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'WorkRequestStatusId', N'IsActive', N'WorkRequestStatusName') AND [object_id] = OBJECT_ID(N'[WorkRequestStatuses]'))
        SET IDENTITY_INSERT [WorkRequestStatuses] ON;
    EXEC(N'INSERT INTO [WorkRequestStatuses] ([WorkRequestStatusId], [IsActive], [WorkRequestStatusName])
    VALUES (10, CAST(1 AS bit), N''Pending Requester''),
    (20, CAST(1 AS bit), N''Pending Initial Approval''),
    (30, CAST(1 AS bit), N''Pending Work''),
    (100, CAST(1 AS bit), N''Completed''),
    (110, CAST(1 AS bit), N''Abandoned'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'WorkRequestStatusId', N'IsActive', N'WorkRequestStatusName') AND [object_id] = OBJECT_ID(N'[WorkRequestStatuses]'))
        SET IDENTITY_INSERT [WorkRequestStatuses] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    CREATE INDEX [IX_FinalAuthorisations_ProcessDeviationReasonId] ON [FinalAuthorisations] ([ProcessDeviationReasonId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    CREATE INDEX [IX_FinalAuthorisations_WorkRequestId] ON [FinalAuthorisations] ([WorkRequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    CREATE INDEX [IX_InitialAuthorisations_WorkRequestId] ON [InitialAuthorisations] ([WorkRequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    CREATE INDEX [IX_WorkRequests_StatusWorkRequestStatusId] ON [WorkRequests] ([StatusWorkRequestStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    CREATE INDEX [IX_WorkRequests_TrialId] ON [WorkRequests] ([TrialId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    ALTER TABLE [FileUploads] ADD CONSTRAINT [FK_FileUploads_WorkRequests_WorkRequestId] FOREIGN KEY ([WorkRequestId]) REFERENCES [WorkRequests] ([WorkRequestId]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    ALTER TABLE [Notifications] ADD CONSTRAINT [FK_Notifications_WorkRequests_WorkRequestId] FOREIGN KEY ([WorkRequestId]) REFERENCES [WorkRequests] ([WorkRequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220811134426_version2'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220811134426_version2', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220824110434_HotfixProcessDeviationReasonNullable'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FinalAuthorisations]') AND [c].[name] = N'ProcessDeviationReasonId');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [FinalAuthorisations] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [FinalAuthorisations] ALTER COLUMN [ProcessDeviationReasonId] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220824110434_HotfixProcessDeviationReasonNullable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220824110434_HotfixProcessDeviationReasonNullable', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220824132131_AddingLabelsAndAssignees'
)
BEGIN
    ALTER TABLE [AspNetUsers] ADD [WorkRequestId] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220824132131_AddingLabelsAndAssignees'
)
BEGIN
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
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220824132131_AddingLabelsAndAssignees'
)
BEGIN
    CREATE INDEX [IX_AspNetUsers_WorkRequestId] ON [AspNetUsers] ([WorkRequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220824132131_AddingLabelsAndAssignees'
)
BEGIN
    CREATE INDEX [IX_Label_WorkRequestId] ON [Label] ([WorkRequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220824132131_AddingLabelsAndAssignees'
)
BEGIN
    ALTER TABLE [AspNetUsers] ADD CONSTRAINT [FK_AspNetUsers_WorkRequests_WorkRequestId] FOREIGN KEY ([WorkRequestId]) REFERENCES [WorkRequests] ([WorkRequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20220824132131_AddingLabelsAndAssignees'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220824132131_AddingLabelsAndAssignees', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221004151939_AddApplicationUser'
)
BEGIN
    ALTER TABLE [AspNetUsers] DROP CONSTRAINT [FK_AspNetUsers_WorkRequests_WorkRequestId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221004151939_AddApplicationUser'
)
BEGIN
    DROP INDEX [IX_AspNetUsers_WorkRequestId] ON [AspNetUsers];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221004151939_AddApplicationUser'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'WorkRequestId');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [AspNetUsers] DROP COLUMN [WorkRequestId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221004151939_AddApplicationUser'
)
BEGIN
    CREATE TABLE [ApplicationUserWorkRequest] (
        [AssigneesId] nvarchar(450) NOT NULL,
        [WorkRequestsWorkRequestId] int NOT NULL,
        CONSTRAINT [PK_ApplicationUserWorkRequest] PRIMARY KEY ([AssigneesId], [WorkRequestsWorkRequestId]),
        CONSTRAINT [FK_ApplicationUserWorkRequest_AspNetUsers_AssigneesId] FOREIGN KEY ([AssigneesId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ApplicationUserWorkRequest_WorkRequests_WorkRequestsWorkRequestId] FOREIGN KEY ([WorkRequestsWorkRequestId]) REFERENCES [WorkRequests] ([WorkRequestId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221004151939_AddApplicationUser'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'NotificationTypeId', N'IsActive', N'NotificationTypeName') AND [object_id] = OBJECT_ID(N'[NotificationTypes]'))
        SET IDENTITY_INSERT [NotificationTypes] ON;
    EXEC(N'INSERT INTO [NotificationTypes] ([NotificationTypeId], [IsActive], [NotificationTypeName])
    VALUES (201, CAST(1 AS bit), N''Assigned to Request'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'NotificationTypeId', N'IsActive', N'NotificationTypeName') AND [object_id] = OBJECT_ID(N'[NotificationTypes]'))
        SET IDENTITY_INSERT [NotificationTypes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221004151939_AddApplicationUser'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'NotificationTypeId', N'IsActive', N'NotificationTypeName') AND [object_id] = OBJECT_ID(N'[NotificationTypes]'))
        SET IDENTITY_INSERT [NotificationTypes] ON;
    EXEC(N'INSERT INTO [NotificationTypes] ([NotificationTypeId], [IsActive], [NotificationTypeName])
    VALUES (202, CAST(1 AS bit), N''Unassigned from Request'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'NotificationTypeId', N'IsActive', N'NotificationTypeName') AND [object_id] = OBJECT_ID(N'[NotificationTypes]'))
        SET IDENTITY_INSERT [NotificationTypes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221004151939_AddApplicationUser'
)
BEGIN
    CREATE INDEX [IX_ApplicationUserWorkRequest_WorkRequestsWorkRequestId] ON [ApplicationUserWorkRequest] ([WorkRequestsWorkRequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221004151939_AddApplicationUser'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221004151939_AddApplicationUser', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221005090703_CorrectLabelWorkRequestRelationship'
)
BEGIN
    ALTER TABLE [Label] DROP CONSTRAINT [FK_Label_WorkRequests_WorkRequestId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221005090703_CorrectLabelWorkRequestRelationship'
)
BEGIN
    DROP INDEX [IX_Label_WorkRequestId] ON [Label];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221005090703_CorrectLabelWorkRequestRelationship'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Label]') AND [c].[name] = N'WorkRequestId');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Label] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [Label] DROP COLUMN [WorkRequestId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221005090703_CorrectLabelWorkRequestRelationship'
)
BEGIN
    CREATE TABLE [LabelWorkRequest] (
        [LabelsLabelId] int NOT NULL,
        [WorkRequestsWorkRequestId] int NOT NULL,
        CONSTRAINT [PK_LabelWorkRequest] PRIMARY KEY ([LabelsLabelId], [WorkRequestsWorkRequestId]),
        CONSTRAINT [FK_LabelWorkRequest_Label_LabelsLabelId] FOREIGN KEY ([LabelsLabelId]) REFERENCES [Label] ([LabelId]) ON DELETE CASCADE,
        CONSTRAINT [FK_LabelWorkRequest_WorkRequests_WorkRequestsWorkRequestId] FOREIGN KEY ([WorkRequestsWorkRequestId]) REFERENCES [WorkRequests] ([WorkRequestId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221005090703_CorrectLabelWorkRequestRelationship'
)
BEGIN
    CREATE INDEX [IX_LabelWorkRequest_WorkRequestsWorkRequestId] ON [LabelWorkRequest] ([WorkRequestsWorkRequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221005090703_CorrectLabelWorkRequestRelationship'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221005090703_CorrectLabelWorkRequestRelationship', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221025135055_NotificationsCreatedByField'
)
BEGIN
    ALTER TABLE [Notifications] ADD [CreatedBy] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221025135055_NotificationsCreatedByField'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221025135055_NotificationsCreatedByField', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221031150700_AddedLastEditedBy'
)
BEGIN
    ALTER TABLE [WorkRequests] ADD [LastEditedBy] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221031150700_AddedLastEditedBy'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221031150700_AddedLastEditedBy', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221031155856_GitHubRef'
)
BEGIN
    ALTER TABLE [WorkRequests] ADD [GitHubIssueNumber] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221031155856_GitHubRef'
)
BEGIN
    ALTER TABLE [Trials] ADD [GitHubRepositoryId] bigint NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221031155856_GitHubRef'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221031155856_GitHubRef', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [WorkRequestStatuses] ADD [PeriodEnd] datetime2 NOT NULL DEFAULT '9999-12-31T23:59:59.9999999';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [WorkRequestStatuses] ADD [PeriodStart] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [WorkRequests] ADD [PeriodEnd] datetime2 NOT NULL DEFAULT '9999-12-31T23:59:59.9999999';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [WorkRequests] ADD [PeriodStart] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [Trials] ADD [PeriodEnd] datetime2 NOT NULL DEFAULT '9999-12-31T23:59:59.9999999';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [Trials] ADD [PeriodStart] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [SentEmails] ADD [PeriodEnd] datetime2 NOT NULL DEFAULT '9999-12-31T23:59:59.9999999';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [SentEmails] ADD [PeriodStart] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [ProcessDeviationReasons] ADD [PeriodEnd] datetime2 NOT NULL DEFAULT '9999-12-31T23:59:59.9999999';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [ProcessDeviationReasons] ADD [PeriodStart] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [NotificationTypes] ADD [PeriodEnd] datetime2 NOT NULL DEFAULT '9999-12-31T23:59:59.9999999';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [NotificationTypes] ADD [PeriodStart] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [Notifications] ADD [PeriodEnd] datetime2 NOT NULL DEFAULT '9999-12-31T23:59:59.9999999';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [Notifications] ADD [PeriodStart] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [LabelWorkRequest] ADD [PeriodEnd] datetime2 NOT NULL DEFAULT '9999-12-31T23:59:59.9999999';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [LabelWorkRequest] ADD [PeriodStart] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [Label] ADD [PeriodEnd] datetime2 NOT NULL DEFAULT '9999-12-31T23:59:59.9999999';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [Label] ADD [PeriodStart] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [InitialAuthorisations] ADD [PeriodEnd] datetime2 NOT NULL DEFAULT '9999-12-31T23:59:59.9999999';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [InitialAuthorisations] ADD [PeriodStart] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [FinalAuthorisations] ADD [PeriodEnd] datetime2 NOT NULL DEFAULT '9999-12-31T23:59:59.9999999';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [FinalAuthorisations] ADD [PeriodStart] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [FileUploads] ADD [PeriodEnd] datetime2 NOT NULL DEFAULT '9999-12-31T23:59:59.9999999';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [FileUploads] ADD [PeriodStart] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [EmailTypes] ADD [PeriodEnd] datetime2 NOT NULL DEFAULT '9999-12-31T23:59:59.9999999';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [EmailTypes] ADD [PeriodStart] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetUserTokens] ADD [PeriodEnd] datetime2 NOT NULL DEFAULT '9999-12-31T23:59:59.9999999';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetUserTokens] ADD [PeriodStart] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetUsers] ADD [PeriodEnd] datetime2 NOT NULL DEFAULT '9999-12-31T23:59:59.9999999';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetUsers] ADD [PeriodStart] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetUserRoles] ADD [PeriodEnd] datetime2 NOT NULL DEFAULT '9999-12-31T23:59:59.9999999';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetUserRoles] ADD [PeriodStart] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetUserLogins] ADD [PeriodEnd] datetime2 NOT NULL DEFAULT '9999-12-31T23:59:59.9999999';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetUserLogins] ADD [PeriodStart] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetUserClaims] ADD [PeriodEnd] datetime2 NOT NULL DEFAULT '9999-12-31T23:59:59.9999999';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetUserClaims] ADD [PeriodStart] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetRoles] ADD [PeriodEnd] datetime2 NOT NULL DEFAULT '9999-12-31T23:59:59.9999999';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetRoles] ADD [PeriodStart] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetRoleClaims] ADD [PeriodEnd] datetime2 NOT NULL DEFAULT '9999-12-31T23:59:59.9999999';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetRoleClaims] ADD [PeriodStart] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [ApplicationUserWorkRequest] ADD [PeriodEnd] datetime2 NOT NULL DEFAULT '9999-12-31T23:59:59.9999999';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [ApplicationUserWorkRequest] ADD [PeriodStart] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    EXEC(N'ALTER TABLE [WorkRequestStatuses] ADD PERIOD FOR SYSTEM_TIME ([PeriodStart], [PeriodEnd])')
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [WorkRequestStatuses] ALTER COLUMN [PeriodStart] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [WorkRequestStatuses] ALTER COLUMN [PeriodEnd] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    EXEC(N'ALTER TABLE [WorkRequests] ADD PERIOD FOR SYSTEM_TIME ([PeriodStart], [PeriodEnd])')
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [WorkRequests] ALTER COLUMN [PeriodStart] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [WorkRequests] ALTER COLUMN [PeriodEnd] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    EXEC(N'ALTER TABLE [Trials] ADD PERIOD FOR SYSTEM_TIME ([PeriodStart], [PeriodEnd])')
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [Trials] ALTER COLUMN [PeriodStart] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [Trials] ALTER COLUMN [PeriodEnd] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    EXEC(N'ALTER TABLE [SentEmails] ADD PERIOD FOR SYSTEM_TIME ([PeriodStart], [PeriodEnd])')
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [SentEmails] ALTER COLUMN [PeriodStart] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [SentEmails] ALTER COLUMN [PeriodEnd] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    EXEC(N'ALTER TABLE [ProcessDeviationReasons] ADD PERIOD FOR SYSTEM_TIME ([PeriodStart], [PeriodEnd])')
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [ProcessDeviationReasons] ALTER COLUMN [PeriodStart] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [ProcessDeviationReasons] ALTER COLUMN [PeriodEnd] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    EXEC(N'ALTER TABLE [NotificationTypes] ADD PERIOD FOR SYSTEM_TIME ([PeriodStart], [PeriodEnd])')
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [NotificationTypes] ALTER COLUMN [PeriodStart] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [NotificationTypes] ALTER COLUMN [PeriodEnd] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    EXEC(N'ALTER TABLE [Notifications] ADD PERIOD FOR SYSTEM_TIME ([PeriodStart], [PeriodEnd])')
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [Notifications] ALTER COLUMN [PeriodStart] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [Notifications] ALTER COLUMN [PeriodEnd] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    EXEC(N'ALTER TABLE [LabelWorkRequest] ADD PERIOD FOR SYSTEM_TIME ([PeriodStart], [PeriodEnd])')
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [LabelWorkRequest] ALTER COLUMN [PeriodStart] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [LabelWorkRequest] ALTER COLUMN [PeriodEnd] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    EXEC(N'ALTER TABLE [Label] ADD PERIOD FOR SYSTEM_TIME ([PeriodStart], [PeriodEnd])')
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [Label] ALTER COLUMN [PeriodStart] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [Label] ALTER COLUMN [PeriodEnd] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    EXEC(N'ALTER TABLE [InitialAuthorisations] ADD PERIOD FOR SYSTEM_TIME ([PeriodStart], [PeriodEnd])')
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [InitialAuthorisations] ALTER COLUMN [PeriodStart] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [InitialAuthorisations] ALTER COLUMN [PeriodEnd] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    EXEC(N'ALTER TABLE [FinalAuthorisations] ADD PERIOD FOR SYSTEM_TIME ([PeriodStart], [PeriodEnd])')
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [FinalAuthorisations] ALTER COLUMN [PeriodStart] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [FinalAuthorisations] ALTER COLUMN [PeriodEnd] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    EXEC(N'ALTER TABLE [FileUploads] ADD PERIOD FOR SYSTEM_TIME ([PeriodStart], [PeriodEnd])')
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [FileUploads] ALTER COLUMN [PeriodStart] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [FileUploads] ALTER COLUMN [PeriodEnd] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    EXEC(N'ALTER TABLE [EmailTypes] ADD PERIOD FOR SYSTEM_TIME ([PeriodStart], [PeriodEnd])')
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [EmailTypes] ALTER COLUMN [PeriodStart] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [EmailTypes] ALTER COLUMN [PeriodEnd] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    EXEC(N'ALTER TABLE [AspNetUserTokens] ADD PERIOD FOR SYSTEM_TIME ([PeriodStart], [PeriodEnd])')
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetUserTokens] ALTER COLUMN [PeriodStart] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetUserTokens] ALTER COLUMN [PeriodEnd] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    EXEC(N'ALTER TABLE [AspNetUsers] ADD PERIOD FOR SYSTEM_TIME ([PeriodStart], [PeriodEnd])')
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetUsers] ALTER COLUMN [PeriodStart] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetUsers] ALTER COLUMN [PeriodEnd] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    EXEC(N'ALTER TABLE [AspNetUserRoles] ADD PERIOD FOR SYSTEM_TIME ([PeriodStart], [PeriodEnd])')
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetUserRoles] ALTER COLUMN [PeriodStart] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetUserRoles] ALTER COLUMN [PeriodEnd] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    EXEC(N'ALTER TABLE [AspNetUserLogins] ADD PERIOD FOR SYSTEM_TIME ([PeriodStart], [PeriodEnd])')
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetUserLogins] ALTER COLUMN [PeriodStart] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetUserLogins] ALTER COLUMN [PeriodEnd] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    EXEC(N'ALTER TABLE [AspNetUserClaims] ADD PERIOD FOR SYSTEM_TIME ([PeriodStart], [PeriodEnd])')
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetUserClaims] ALTER COLUMN [PeriodStart] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetUserClaims] ALTER COLUMN [PeriodEnd] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    EXEC(N'ALTER TABLE [AspNetRoles] ADD PERIOD FOR SYSTEM_TIME ([PeriodStart], [PeriodEnd])')
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetRoles] ALTER COLUMN [PeriodStart] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetRoles] ALTER COLUMN [PeriodEnd] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    EXEC(N'ALTER TABLE [AspNetRoleClaims] ADD PERIOD FOR SYSTEM_TIME ([PeriodStart], [PeriodEnd])')
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetRoleClaims] ALTER COLUMN [PeriodStart] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [AspNetRoleClaims] ALTER COLUMN [PeriodEnd] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    EXEC(N'ALTER TABLE [ApplicationUserWorkRequest] ADD PERIOD FOR SYSTEM_TIME ([PeriodStart], [PeriodEnd])')
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [ApplicationUserWorkRequest] ALTER COLUMN [PeriodStart] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    ALTER TABLE [ApplicationUserWorkRequest] ALTER COLUMN [PeriodEnd] ADD HIDDEN
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [WorkRequestStatuses] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[WorkRequestStatusesHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [WorkRequests] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[WorkRequestsHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [Trials] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[TrialsHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [SentEmails] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[SentEmailsHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [ProcessDeviationReasons] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[ProcessDeviationReasonsHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [NotificationTypes] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[NotificationTypesHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [Notifications] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[NotificationsHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [LabelWorkRequest] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[LabelWorkRequestHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [Label] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[LabelHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [InitialAuthorisations] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[InitialAuthorisationsHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [FinalAuthorisations] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[FinalAuthorisationsHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [FileUploads] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[FileUploadsHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [EmailTypes] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[EmailTypesHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [AspNetUserTokens] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[AspNetUserTokensHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [AspNetUsers] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[AspNetUsersHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [AspNetUserRoles] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[AspNetUserRolesHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [AspNetUserLogins] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[AspNetUserLoginsHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [AspNetUserClaims] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[AspNetUserClaimsHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [AspNetRoles] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[AspNetRolesHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [AspNetRoleClaims] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[AspNetRoleClaimsHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [ApplicationUserWorkRequest] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[ApplicationUserWorkRequestHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221110141833_AddingTemporalTables'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221110141833_AddingTemporalTables', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221116172859_AddTemplates'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'CREATE TABLE [Templates] (
        [TemplateId] int NOT NULL IDENTITY,
        [TemplateShort] nvarchar(max) NULL,
        [TemplateDescription] nvarchar(max) NULL,
        [TemplateLayout] nvarchar(max) NULL,
        [IsArchived] bit NOT NULL,
        [PeriodEnd] datetime2 GENERATED ALWAYS AS ROW END HIDDEN NOT NULL,
        [PeriodStart] datetime2 GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
        CONSTRAINT [PK_Templates] PRIMARY KEY ([TemplateId]),
        PERIOD FOR SYSTEM_TIME([PeriodStart], [PeriodEnd])
    ) WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + N'].[TemplatesHistory]))');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221116172859_AddTemplates'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221116172859_AddTemplates', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221129131447_ConversationalView'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'CREATE TABLE [WorkRequestEventTypes] (
        [WorkRequestEventTypeId] int NOT NULL,
        [WorkRequestEventTypeName] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [PeriodEnd] datetime2 GENERATED ALWAYS AS ROW END HIDDEN NOT NULL,
        [PeriodStart] datetime2 GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
        CONSTRAINT [PK_WorkRequestEventTypes] PRIMARY KEY ([WorkRequestEventTypeId]),
        PERIOD FOR SYSTEM_TIME([PeriodStart], [PeriodEnd])
    ) WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + N'].[WorkRequestEventTypesHistory]))');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221129131447_ConversationalView'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'CREATE TABLE [WorkRequestEvents] (
        [WorkRequestEventId] int NOT NULL IDENTITY,
        [WorkRequestId] int NULL,
        [EventTypeWorkRequestEventTypeId] int NULL,
        [DurationDays] float NULL,
        [Content] nvarchar(max) NULL,
        [CreatedById] nvarchar(450) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [PeriodEnd] datetime2 GENERATED ALWAYS AS ROW END HIDDEN NOT NULL,
        [PeriodStart] datetime2 GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
        CONSTRAINT [PK_WorkRequestEvents] PRIMARY KEY ([WorkRequestEventId]),
        CONSTRAINT [FK_WorkRequestEvents_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_WorkRequestEvents_WorkRequestEventTypes_EventTypeWorkRequestEventTypeId] FOREIGN KEY ([EventTypeWorkRequestEventTypeId]) REFERENCES [WorkRequestEventTypes] ([WorkRequestEventTypeId]),
        CONSTRAINT [FK_WorkRequestEvents_WorkRequests_WorkRequestId] FOREIGN KEY ([WorkRequestId]) REFERENCES [WorkRequests] ([WorkRequestId]),
        PERIOD FOR SYSTEM_TIME([PeriodStart], [PeriodEnd])
    ) WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + N'].[WorkRequestEventsHistory]))');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221129131447_ConversationalView'
)
BEGIN
    CREATE INDEX [IX_WorkRequestEvents_CreatedById] ON [WorkRequestEvents] ([CreatedById]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221129131447_ConversationalView'
)
BEGIN
    CREATE INDEX [IX_WorkRequestEvents_EventTypeWorkRequestEventTypeId] ON [WorkRequestEvents] ([EventTypeWorkRequestEventTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221129131447_ConversationalView'
)
BEGIN
    CREATE INDEX [IX_WorkRequestEvents_WorkRequestId] ON [WorkRequestEvents] ([WorkRequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221129131447_ConversationalView'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221129131447_ConversationalView', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221129133320_WorkRequestEventTypeEnum'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'WorkRequestEventTypeId', N'IsActive', N'WorkRequestEventTypeName') AND [object_id] = OBJECT_ID(N'[WorkRequestEventTypes]'))
        SET IDENTITY_INSERT [WorkRequestEventTypes] ON;
    EXEC(N'INSERT INTO [WorkRequestEventTypes] ([WorkRequestEventTypeId], [IsActive], [WorkRequestEventTypeName])
    VALUES (1, CAST(1 AS bit), N''Message''),
    (2, CAST(1 AS bit), N''Requesting Changes''),
    (3, CAST(1 AS bit), N''Approving''),
    (4, CAST(1 AS bit), N''Completing''),
    (5, CAST(1 AS bit), N''Closing'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'WorkRequestEventTypeId', N'IsActive', N'WorkRequestEventTypeName') AND [object_id] = OBJECT_ID(N'[WorkRequestEventTypes]'))
        SET IDENTITY_INSERT [WorkRequestEventTypes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221129133320_WorkRequestEventTypeEnum'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221129133320_WorkRequestEventTypeEnum', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221130143052_AddNotificationTypeWorkRequestReceived'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'NotificationTypeId', N'IsActive', N'NotificationTypeName') AND [object_id] = OBJECT_ID(N'[NotificationTypes]'))
        SET IDENTITY_INSERT [NotificationTypes] ON;
    EXEC(N'INSERT INTO [NotificationTypes] ([NotificationTypeId], [IsActive], [NotificationTypeName])
    VALUES (5, CAST(1 AS bit), N''Work Request Received'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'NotificationTypeId', N'IsActive', N'NotificationTypeName') AND [object_id] = OBJECT_ID(N'[NotificationTypes]'))
        SET IDENTITY_INSERT [NotificationTypes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221130143052_AddNotificationTypeWorkRequestReceived'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221130143052_AddNotificationTypeWorkRequestReceived', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221202161828_ProcessDeviationReason_WorkRequest'
)
BEGIN
    ALTER TABLE [WorkRequests] ADD [ProcessDeviationReasonId] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221202161828_ProcessDeviationReason_WorkRequest'
)
BEGIN
    CREATE INDEX [IX_WorkRequests_ProcessDeviationReasonId] ON [WorkRequests] ([ProcessDeviationReasonId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221202161828_ProcessDeviationReason_WorkRequest'
)
BEGIN
    ALTER TABLE [WorkRequests] ADD CONSTRAINT [FK_WorkRequests_ProcessDeviationReasons_ProcessDeviationReasonId] FOREIGN KEY ([ProcessDeviationReasonId]) REFERENCES [ProcessDeviationReasons] ([ProcessDeviationReasonId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221202161828_ProcessDeviationReason_WorkRequest'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221202161828_ProcessDeviationReason_WorkRequest', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221205101047_AddedLastEditedDateTime'
)
BEGIN
    ALTER TABLE [WorkRequests] ADD [LastEditedDateTime] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20221205101047_AddedLastEditedDateTime'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221205101047_AddedLastEditedDateTime', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230330135741_UpdateWorkRequestEventTypeValues'
)
BEGIN
    EXEC(N'UPDATE [WorkRequestEventTypes] SET [WorkRequestEventTypeName] = N''Request Changes''
    WHERE [WorkRequestEventTypeId] = 2;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230330135741_UpdateWorkRequestEventTypeValues'
)
BEGIN
    EXEC(N'UPDATE [WorkRequestEventTypes] SET [WorkRequestEventTypeName] = N''Approve''
    WHERE [WorkRequestEventTypeId] = 3;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230330135741_UpdateWorkRequestEventTypeValues'
)
BEGIN
    EXEC(N'UPDATE [WorkRequestEventTypes] SET [WorkRequestEventTypeName] = N''Complete''
    WHERE [WorkRequestEventTypeId] = 4;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230330135741_UpdateWorkRequestEventTypeValues'
)
BEGIN
    EXEC(N'UPDATE [WorkRequestEventTypes] SET [WorkRequestEventTypeName] = N''Close''
    WHERE [WorkRequestEventTypeId] = 5;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230330135741_UpdateWorkRequestEventTypeValues'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230330135741_UpdateWorkRequestEventTypeValues', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230403075632_AddedSubscriptions'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'CREATE TABLE [WorkRequestSubscriptions] (
        [Id] int NOT NULL IDENTITY,
        [WorkRequestId] int NULL,
        [ApplicationUserId] nvarchar(450) NULL,
        [PeriodEnd] datetime2 GENERATED ALWAYS AS ROW END HIDDEN NOT NULL,
        [PeriodStart] datetime2 GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
        CONSTRAINT [PK_WorkRequestSubscriptions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_WorkRequestSubscriptions_AspNetUsers_ApplicationUserId] FOREIGN KEY ([ApplicationUserId]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_WorkRequestSubscriptions_WorkRequests_WorkRequestId] FOREIGN KEY ([WorkRequestId]) REFERENCES [WorkRequests] ([WorkRequestId]),
        PERIOD FOR SYSTEM_TIME([PeriodStart], [PeriodEnd])
    ) WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + N'].[WorkRequestSubscriptionsHistory]))');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230403075632_AddedSubscriptions'
)
BEGIN
    CREATE INDEX [IX_WorkRequestSubscriptions_ApplicationUserId] ON [WorkRequestSubscriptions] ([ApplicationUserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230403075632_AddedSubscriptions'
)
BEGIN
    CREATE INDEX [IX_WorkRequestSubscriptions_WorkRequestId] ON [WorkRequestSubscriptions] ([WorkRequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230403075632_AddedSubscriptions'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230403075632_AddedSubscriptions', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240412111314_20240412_AddedAssignmentWorkRequestEventTypeEnum'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'WorkRequestEventTypeId', N'IsActive', N'WorkRequestEventTypeName') AND [object_id] = OBJECT_ID(N'[WorkRequestEventTypes]'))
        SET IDENTITY_INSERT [WorkRequestEventTypes] ON;
    EXEC(N'INSERT INTO [WorkRequestEventTypes] ([WorkRequestEventTypeId], [IsActive], [WorkRequestEventTypeName])
    VALUES (10, CAST(0 AS bit), N''Assignment'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'WorkRequestEventTypeId', N'IsActive', N'WorkRequestEventTypeName') AND [object_id] = OBJECT_ID(N'[WorkRequestEventTypes]'))
        SET IDENTITY_INSERT [WorkRequestEventTypes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240412111314_20240412_AddedAssignmentWorkRequestEventTypeEnum'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240412111314_20240412_AddedAssignmentWorkRequestEventTypeEnum', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240627084922_AddTriageInfo'
)
BEGIN
    ALTER TABLE [Trials] ADD [TrialAttribution] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240627084922_AddTriageInfo'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'CREATE TABLE [TriageInfoRotas] (
        [TriageInfoRotaId] int NOT NULL IDENTITY,
        [Day] int NOT NULL,
        [Morning] nvarchar(max) NULL,
        [Afternoon] nvarchar(max) NULL,
        [Reserve] nvarchar(max) NULL,
        [PeriodEnd] datetime2 GENERATED ALWAYS AS ROW END HIDDEN NOT NULL,
        [PeriodStart] datetime2 GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
        CONSTRAINT [PK_TriageInfoRotas] PRIMARY KEY ([TriageInfoRotaId]),
        PERIOD FOR SYSTEM_TIME([PeriodStart], [PeriodEnd])
    ) WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + N'].[TriageInfoRotasHistory]))');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240627084922_AddTriageInfo'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240627084922_AddTriageInfo', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240710145826_AddMessageNotificationEnum'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'NotificationTypeId', N'IsActive', N'NotificationTypeName') AND [object_id] = OBJECT_ID(N'[NotificationTypes]'))
        SET IDENTITY_INSERT [NotificationTypes] ON;
    EXEC(N'INSERT INTO [NotificationTypes] ([NotificationTypeId], [IsActive], [NotificationTypeName])
    VALUES (300, CAST(1 AS bit), N''New Message'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'NotificationTypeId', N'IsActive', N'NotificationTypeName') AND [object_id] = OBJECT_ID(N'[NotificationTypes]'))
        SET IDENTITY_INSERT [NotificationTypes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240710145826_AddMessageNotificationEnum'
)
BEGIN
    EXEC(N'UPDATE [WorkRequestStatuses] SET [WorkRequestStatusName] = N''Closed''
    WHERE [WorkRequestStatusId] = 110;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240710145826_AddMessageNotificationEnum'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240710145826_AddMessageNotificationEnum', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240711142748_AddMessageToNotification'
)
BEGIN
    ALTER TABLE [Notifications] ADD [Message] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240711142748_AddMessageToNotification'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240711142748_AddMessageToNotification', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250203122241_UpdateSentEmailModel'
)
BEGIN
    ALTER TABLE [SentEmails] DROP CONSTRAINT [FK_SentEmails_EmailTypes_EmailTypeId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250203122241_UpdateSentEmailModel'
)
BEGIN
    DROP INDEX [IX_SentEmails_EmailTypeId] ON [SentEmails];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250203122241_UpdateSentEmailModel'
)
BEGIN
    ALTER TABLE [SentEmails] SET (SYSTEM_VERSIONING = OFF)

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250203122241_UpdateSentEmailModel'
)
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SentEmails]') AND [c].[name] = N'EmailTypeId');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [SentEmails] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [SentEmails] DROP COLUMN [EmailTypeId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250203122241_UpdateSentEmailModel'
)
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SentEmailsHistory]') AND [c].[name] = N'EmailTypeId');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [SentEmailsHistory] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [SentEmailsHistory] DROP COLUMN [EmailTypeId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250203122241_UpdateSentEmailModel'
)
BEGIN
    EXEC sp_rename N'[SentEmails].[Recipient]', N'Username', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250203122241_UpdateSentEmailModel'
)
BEGIN
    EXEC sp_rename N'[SentEmailsHistory].[Recipient]', N'Username', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250203122241_UpdateSentEmailModel'
)
BEGIN
    EXEC sp_rename N'[SentEmails].[PlainTextBody]', N'SentTo', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250203122241_UpdateSentEmailModel'
)
BEGIN
    EXEC sp_rename N'[SentEmailsHistory].[PlainTextBody]', N'SentTo', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250203122241_UpdateSentEmailModel'
)
BEGIN
    EXEC sp_rename N'[SentEmails].[From]', N'SentFrom', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250203122241_UpdateSentEmailModel'
)
BEGIN
    EXEC sp_rename N'[SentEmailsHistory].[From]', N'SentFrom', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250203122241_UpdateSentEmailModel'
)
BEGIN
    EXEC sp_rename N'[SentEmails].[EmailSentDateTime]', N'SentAt', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250203122241_UpdateSentEmailModel'
)
BEGIN
    EXEC sp_rename N'[SentEmailsHistory].[EmailSentDateTime]', N'SentAt', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250203122241_UpdateSentEmailModel'
)
BEGIN
    ALTER TABLE [SentEmails] ADD [BccEmails] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250203122241_UpdateSentEmailModel'
)
BEGIN
    ALTER TABLE [SentEmailsHistory] ADD [BccEmails] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250203122241_UpdateSentEmailModel'
)
BEGIN
    ALTER TABLE [SentEmails] ADD [CcEmails] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250203122241_UpdateSentEmailModel'
)
BEGIN
    ALTER TABLE [SentEmailsHistory] ADD [CcEmails] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250203122241_UpdateSentEmailModel'
)
BEGIN
    EXEC(N'UPDATE [WorkRequestEventTypes] SET [WorkRequestEventTypeName] = N''Close as Completed''
    WHERE [WorkRequestEventTypeId] = 4;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250203122241_UpdateSentEmailModel'
)
BEGIN
    EXEC(N'UPDATE [WorkRequestEventTypes] SET [WorkRequestEventTypeName] = N''Close as Not Planned''
    WHERE [WorkRequestEventTypeId] = 5;
    SELECT @@ROWCOUNT');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250203122241_UpdateSentEmailModel'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [SentEmails] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[SentEmailsHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250203122241_UpdateSentEmailModel'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250203122241_UpdateSentEmailModel', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250204105728_RemoveNotificationReferencesWithEmails'
)
BEGIN
    ALTER TABLE [SentEmails] DROP CONSTRAINT [FK_SentEmails_Notifications_NotificationId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250204105728_RemoveNotificationReferencesWithEmails'
)
BEGIN
    DROP INDEX [IX_SentEmails_NotificationId] ON [SentEmails];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250204105728_RemoveNotificationReferencesWithEmails'
)
BEGIN
    ALTER TABLE [SentEmails] SET (SYSTEM_VERSIONING = OFF)

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250204105728_RemoveNotificationReferencesWithEmails'
)
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SentEmails]') AND [c].[name] = N'NotificationId');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [SentEmails] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [SentEmails] DROP COLUMN [NotificationId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250204105728_RemoveNotificationReferencesWithEmails'
)
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SentEmailsHistory]') AND [c].[name] = N'NotificationId');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [SentEmailsHistory] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [SentEmailsHistory] DROP COLUMN [NotificationId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250204105728_RemoveNotificationReferencesWithEmails'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [SentEmails] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[SentEmailsHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250204105728_RemoveNotificationReferencesWithEmails'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250204105728_RemoveNotificationReferencesWithEmails', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250206142156_AddTrialRepositoryInfoMultiRepoConfig'
)
BEGIN
    ALTER TABLE [Trials] SET (SYSTEM_VERSIONING = OFF)

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250206142156_AddTrialRepositoryInfoMultiRepoConfig'
)
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Trials]') AND [c].[name] = N'GitHubRepositoryId');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Trials] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [Trials] DROP COLUMN [GitHubRepositoryId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250206142156_AddTrialRepositoryInfoMultiRepoConfig'
)
BEGIN
    DECLARE @var8 sysname;
    SELECT @var8 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[TrialsHistory]') AND [c].[name] = N'GitHubRepositoryId');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [TrialsHistory] DROP CONSTRAINT [' + @var8 + '];');
    ALTER TABLE [TrialsHistory] DROP COLUMN [GitHubRepositoryId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250206142156_AddTrialRepositoryInfoMultiRepoConfig'
)
BEGIN
    ALTER TABLE [WorkRequests] ADD [AssignedTrialRepositoryId] bigint NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250206142156_AddTrialRepositoryInfoMultiRepoConfig'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'CREATE TABLE [TrialRepositoryInfos] (
        [Id] int NOT NULL IDENTITY,
        [GitHubRepositoryId] bigint NOT NULL,
        [TrialId] int NOT NULL,
        [PeriodEnd] datetime2 GENERATED ALWAYS AS ROW END HIDDEN NOT NULL,
        [PeriodStart] datetime2 GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
        CONSTRAINT [PK_TrialRepositoryInfos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TrialRepositoryInfos_Trials_TrialId] FOREIGN KEY ([TrialId]) REFERENCES [Trials] ([TrialId]) ON DELETE CASCADE,
        PERIOD FOR SYSTEM_TIME([PeriodStart], [PeriodEnd])
    ) WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + N'].[TrialRepositoryInfosHistory]))');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250206142156_AddTrialRepositoryInfoMultiRepoConfig'
)
BEGIN
    CREATE INDEX [IX_TrialRepositoryInfos_TrialId] ON [TrialRepositoryInfos] ([TrialId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250206142156_AddTrialRepositoryInfoMultiRepoConfig'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'ALTER TABLE [Trials] SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + '].[TrialsHistory]))')

END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250206142156_AddTrialRepositoryInfoMultiRepoConfig'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250206142156_AddTrialRepositoryInfoMultiRepoConfig', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250714153240_AddBccAddressesToNotifications'
)
BEGIN
    ALTER TABLE [Notifications] ADD [BccAddresses] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250714153240_AddBccAddressesToNotifications'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250714153240_AddBccAddressesToNotifications', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250801141743_AddContactUs'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'CREATE TABLE [ContactUs] (
        [ContactUsId] int NOT NULL IDENTITY,
        [Message] nvarchar(max) NULL,
        [Email] nvarchar(max) NULL,
        [Submitted] datetime2 NOT NULL,
        [PeriodEnd] datetime2 GENERATED ALWAYS AS ROW END HIDDEN NOT NULL,
        [PeriodStart] datetime2 GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
        CONSTRAINT [PK_ContactUs] PRIMARY KEY ([ContactUsId]),
        PERIOD FOR SYSTEM_TIME([PeriodStart], [PeriodEnd])
    ) WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + N'].[ContactUsHistory]))');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250801141743_AddContactUs'
)
BEGIN

                INSERT INTO AspNetRoles ([Id],[Name], [NormalizedName]) VALUES (100,'Developer', 'DEVELOPER')
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250801141743_AddContactUs'
)
BEGIN

                INSERT INTO AspNetUserRoles (UserId, RoleId)
                SELECT u.Id, r.Id
                FROM AspNetUsers u
                JOIN AspNetRoles r ON r.NormalizedName = 'DEVELOPER'
                WHERE u.Email IN (
                    'tom.pool@york.ac.uk',
                    'elise-brook.davis-hirst@york.ac.uk',
                    'anthony.wishart@york.ac.uk'
                );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250801141743_AddContactUs'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250801141743_AddContactUs', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250806140844_AddSubTaskAdditions'
)
BEGIN
    ALTER TABLE [Notifications] ADD [SubTaskId] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250806140844_AddSubTaskAdditions'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'CREATE TABLE [SubTaskEventTypes] (
        [SubTaskEventTypeId] int NOT NULL,
        [SubTaskEventTypeName] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [PeriodEnd] datetime2 GENERATED ALWAYS AS ROW END HIDDEN NOT NULL,
        [PeriodStart] datetime2 GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
        CONSTRAINT [PK_SubTaskEventTypes] PRIMARY KEY ([SubTaskEventTypeId]),
        PERIOD FOR SYSTEM_TIME([PeriodStart], [PeriodEnd])
    ) WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + N'].[SubTaskEventTypesHistory]))');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250806140844_AddSubTaskAdditions'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'CREATE TABLE [SubTaskStatuses] (
        [SubTaskStatusId] int NOT NULL,
        [SubTaskStatusName] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [PeriodEnd] datetime2 GENERATED ALWAYS AS ROW END HIDDEN NOT NULL,
        [PeriodStart] datetime2 GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
        CONSTRAINT [PK_SubTaskStatuses] PRIMARY KEY ([SubTaskStatusId]),
        PERIOD FOR SYSTEM_TIME([PeriodStart], [PeriodEnd])
    ) WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + N'].[SubTaskStatusesHistory]))');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250806140844_AddSubTaskAdditions'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'CREATE TABLE [SubTasks] (
        [SubTaskId] int NOT NULL IDENTITY,
        [SubTaskTitle] nvarchar(max) NULL,
        [WorkRequestId] int NOT NULL,
        [StatusSubTaskStatusId] int NULL,
        [AssigneeId] nvarchar(450) NULL,
        [CreatedBy] nvarchar(max) NULL,
        [LastEditedBy] nvarchar(max) NULL,
        [CreationDateTime] datetime2 NOT NULL,
        [LastEditedDateTime] datetime2 NOT NULL,
        [PeriodEnd] datetime2 GENERATED ALWAYS AS ROW END HIDDEN NOT NULL,
        [PeriodStart] datetime2 GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
        CONSTRAINT [PK_SubTasks] PRIMARY KEY ([SubTaskId]),
        CONSTRAINT [FK_SubTasks_AspNetUsers_AssigneeId] FOREIGN KEY ([AssigneeId]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_SubTasks_SubTaskStatuses_StatusSubTaskStatusId] FOREIGN KEY ([StatusSubTaskStatusId]) REFERENCES [SubTaskStatuses] ([SubTaskStatusId]),
        CONSTRAINT [FK_SubTasks_WorkRequests_WorkRequestId] FOREIGN KEY ([WorkRequestId]) REFERENCES [WorkRequests] ([WorkRequestId]) ON DELETE CASCADE,
        PERIOD FOR SYSTEM_TIME([PeriodStart], [PeriodEnd])
    ) WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + N'].[SubTasksHistory]))');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250806140844_AddSubTaskAdditions'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'CREATE TABLE [SubTaskEvents] (
        [SubTaskEventId] int NOT NULL IDENTITY,
        [SubTaskId] int NULL,
        [EventTypeSubTaskEventTypeId] int NULL,
        [Content] nvarchar(max) NULL,
        [CreatedById] nvarchar(450) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [PeriodEnd] datetime2 GENERATED ALWAYS AS ROW END HIDDEN NOT NULL,
        [PeriodStart] datetime2 GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
        CONSTRAINT [PK_SubTaskEvents] PRIMARY KEY ([SubTaskEventId]),
        CONSTRAINT [FK_SubTaskEvents_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_SubTaskEvents_SubTaskEventTypes_EventTypeSubTaskEventTypeId] FOREIGN KEY ([EventTypeSubTaskEventTypeId]) REFERENCES [SubTaskEventTypes] ([SubTaskEventTypeId]),
        CONSTRAINT [FK_SubTaskEvents_SubTasks_SubTaskId] FOREIGN KEY ([SubTaskId]) REFERENCES [SubTasks] ([SubTaskId]),
        PERIOD FOR SYSTEM_TIME([PeriodStart], [PeriodEnd])
    ) WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + N'].[SubTaskEventsHistory]))');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250806140844_AddSubTaskAdditions'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'NotificationTypeId', N'IsActive', N'NotificationTypeName') AND [object_id] = OBJECT_ID(N'[NotificationTypes]'))
        SET IDENTITY_INSERT [NotificationTypes] ON;
    EXEC(N'INSERT INTO [NotificationTypes] ([NotificationTypeId], [IsActive], [NotificationTypeName])
    VALUES (501, CAST(1 AS bit), N''Sub Task Created''),
    (502, CAST(1 AS bit), N''Sub Task Assigned''),
    (503, CAST(1 AS bit), N''Sub Task - New Message''),
    (504, CAST(1 AS bit), N''Sub Task Approved''),
    (505, CAST(1 AS bit), N''Sub Task Rejected''),
    (506, CAST(1 AS bit), N''Sub Task Closed As Not Planned'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'NotificationTypeId', N'IsActive', N'NotificationTypeName') AND [object_id] = OBJECT_ID(N'[NotificationTypes]'))
        SET IDENTITY_INSERT [NotificationTypes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250806140844_AddSubTaskAdditions'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'SubTaskEventTypeId', N'IsActive', N'SubTaskEventTypeName') AND [object_id] = OBJECT_ID(N'[SubTaskEventTypes]'))
        SET IDENTITY_INSERT [SubTaskEventTypes] ON;
    EXEC(N'INSERT INTO [SubTaskEventTypes] ([SubTaskEventTypeId], [IsActive], [SubTaskEventTypeName])
    VALUES (1, CAST(1 AS bit), N''Message''),
    (2, CAST(1 AS bit), N''Approve''),
    (3, CAST(1 AS bit), N''Reject''),
    (4, CAST(1 AS bit), N''Closed as Not Planned''),
    (5, CAST(1 AS bit), N''Assignment'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'SubTaskEventTypeId', N'IsActive', N'SubTaskEventTypeName') AND [object_id] = OBJECT_ID(N'[SubTaskEventTypes]'))
        SET IDENTITY_INSERT [SubTaskEventTypes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250806140844_AddSubTaskAdditions'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'SubTaskStatusId', N'IsActive', N'SubTaskStatusName') AND [object_id] = OBJECT_ID(N'[SubTaskStatuses]'))
        SET IDENTITY_INSERT [SubTaskStatuses] ON;
    EXEC(N'INSERT INTO [SubTaskStatuses] ([SubTaskStatusId], [IsActive], [SubTaskStatusName])
    VALUES (510, CAST(1 AS bit), N''Open''),
    (520, CAST(1 AS bit), N''Approved''),
    (600, CAST(1 AS bit), N''Rejected''),
    (610, CAST(1 AS bit), N''Closed as not planned'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'SubTaskStatusId', N'IsActive', N'SubTaskStatusName') AND [object_id] = OBJECT_ID(N'[SubTaskStatuses]'))
        SET IDENTITY_INSERT [SubTaskStatuses] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250806140844_AddSubTaskAdditions'
)
BEGIN
    CREATE INDEX [IX_Notifications_SubTaskId] ON [Notifications] ([SubTaskId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250806140844_AddSubTaskAdditions'
)
BEGIN
    CREATE INDEX [IX_SubTaskEvents_CreatedById] ON [SubTaskEvents] ([CreatedById]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250806140844_AddSubTaskAdditions'
)
BEGIN
    CREATE INDEX [IX_SubTaskEvents_EventTypeSubTaskEventTypeId] ON [SubTaskEvents] ([EventTypeSubTaskEventTypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250806140844_AddSubTaskAdditions'
)
BEGIN
    CREATE INDEX [IX_SubTaskEvents_SubTaskId] ON [SubTaskEvents] ([SubTaskId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250806140844_AddSubTaskAdditions'
)
BEGIN
    CREATE INDEX [IX_SubTasks_AssigneeId] ON [SubTasks] ([AssigneeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250806140844_AddSubTaskAdditions'
)
BEGIN
    CREATE INDEX [IX_SubTasks_StatusSubTaskStatusId] ON [SubTasks] ([StatusSubTaskStatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250806140844_AddSubTaskAdditions'
)
BEGIN
    CREATE INDEX [IX_SubTasks_WorkRequestId] ON [SubTasks] ([WorkRequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250806140844_AddSubTaskAdditions'
)
BEGIN
    ALTER TABLE [Notifications] ADD CONSTRAINT [FK_Notifications_SubTasks_SubTaskId] FOREIGN KEY ([SubTaskId]) REFERENCES [SubTasks] ([SubTaskId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250806140844_AddSubTaskAdditions'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250806140844_AddSubTaskAdditions', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250813134601_AddContactUsSubmissionPage'
)
BEGIN
    DECLARE @var9 sysname;
    SELECT @var9 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ContactUs]') AND [c].[name] = N'Message');
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [ContactUs] DROP CONSTRAINT [' + @var9 + '];');
    EXEC(N'UPDATE [ContactUs] SET [Message] = '''' WHERE [Message] IS NULL');
    ALTER TABLE [ContactUs] ALTER COLUMN [Message] varchar(max) NOT NULL;
    ALTER TABLE [ContactUs] ADD DEFAULT '' FOR [Message];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250813134601_AddContactUsSubmissionPage'
)
BEGIN
    DECLARE @var10 sysname;
    SELECT @var10 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ContactUs]') AND [c].[name] = N'Email');
    IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [ContactUs] DROP CONSTRAINT [' + @var10 + '];');
    EXEC(N'UPDATE [ContactUs] SET [Email] = '''' WHERE [Email] IS NULL');
    ALTER TABLE [ContactUs] ALTER COLUMN [Email] varchar(255) NOT NULL;
    ALTER TABLE [ContactUs] ADD DEFAULT '' FOR [Email];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250813134601_AddContactUsSubmissionPage'
)
BEGIN
    ALTER TABLE [ContactUs] ADD [Actioned] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250813134601_AddContactUsSubmissionPage'
)
BEGIN
    ALTER TABLE [ContactUs] ADD [ActionedAt] datetime2 NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250813134601_AddContactUsSubmissionPage'
)
BEGIN
    ALTER TABLE [ContactUs] ADD [ActionedBy] varchar(255) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250813134601_AddContactUsSubmissionPage'
)
BEGIN
    ALTER TABLE [ContactUs] ADD [Notes] varchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250813134601_AddContactUsSubmissionPage'
)
BEGIN
    ALTER TABLE [ContactUs] ADD [UpdatedBy] varchar(255) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250813134601_AddContactUsSubmissionPage'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250813134601_AddContactUsSubmissionPage', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251007104522_AddTrialOwnerToTrial'
)
BEGIN
    ALTER TABLE [Trials] ADD [TrialOwner] int NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251007104522_AddTrialOwnerToTrial'
)
BEGIN

                INSERT INTO [dbo].[AspNetUsers] 
                (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
                VALUES 
                (NEWID(), 'system', 'SYSTEM', 'system@york.ac.uk', 'SYSTEM@YORK.AC.UK', 1, NULL, NEWID(), NEWID(), NULL, 0, 0, NULL, 1, 0);
            
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251007104522_AddTrialOwnerToTrial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251007104522_AddTrialOwnerToTrial', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251009102831_AddSystemAccountColumn'
)
BEGIN
    ALTER TABLE [AspNetUsers] ADD [isSystemAccount] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251009102831_AddSystemAccountColumn'
)
BEGIN

                    UPDATE [dbo].[AspNetUsers]
                    SET [IsSystemAccount] = 1
                    WHERE [Email] = 'system@york.ac.uk';
                
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251009102831_AddSystemAccountColumn'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251009102831_AddSystemAccountColumn', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251009121026_AddTrialEmailToTrial'
)
BEGIN
    ALTER TABLE [Trials] ADD [TrialEmailId] nvarchar(450) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251009121026_AddTrialEmailToTrial'
)
BEGIN
    CREATE INDEX [IX_Trials_TrialEmailId] ON [Trials] ([TrialEmailId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251009121026_AddTrialEmailToTrial'
)
BEGIN
    ALTER TABLE [Trials] ADD CONSTRAINT [FK_Trials_AspNetUsers_TrialEmailId] FOREIGN KEY ([TrialEmailId]) REFERENCES [AspNetUsers] ([Id]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251009121026_AddTrialEmailToTrial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251009121026_AddTrialEmailToTrial', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251031093351_AddNewWorkRequestEventTypes'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'WorkRequestEventTypeId', N'IsActive', N'WorkRequestEventTypeName') AND [object_id] = OBJECT_ID(N'[WorkRequestEventTypes]'))
        SET IDENTITY_INSERT [WorkRequestEventTypes] ON;
    EXEC(N'INSERT INTO [WorkRequestEventTypes] ([WorkRequestEventTypeId], [IsActive], [WorkRequestEventTypeName])
    VALUES (11, CAST(0 AS bit), N''Label''),
    (12, CAST(0 AS bit), N''GitHub Issue Attachment'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'WorkRequestEventTypeId', N'IsActive', N'WorkRequestEventTypeName') AND [object_id] = OBJECT_ID(N'[WorkRequestEventTypes]'))
        SET IDENTITY_INSERT [WorkRequestEventTypes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251031093351_AddNewWorkRequestEventTypes'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251031093351_AddNewWorkRequestEventTypes', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251106133447_AddDataExportLogsTable'
)
BEGIN
    CREATE TABLE [dbo].[DataExportLog] (
        [Id] int NOT NULL IDENTITY,
        [WorkRequestId] int NULL,
        [DownloadName] nvarchar(max) NULL,
        [DataExportBy] nvarchar(max) NULL,
        [DataExportDate] datetime2 NOT NULL,
        [DataExportSuccessful] bit NOT NULL,
        [ExceptionText] nvarchar(max) NULL,
        [PeriodEnd] datetime2 GENERATED ALWAYS AS ROW END HIDDEN NOT NULL,
        [PeriodStart] datetime2 GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
        CONSTRAINT [PK_DataExportLog] PRIMARY KEY ([Id]),
        PERIOD FOR SYSTEM_TIME([PeriodStart], [PeriodEnd])
    ) WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[DataExportLogHistory]));
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251106133447_AddDataExportLogsTable'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'WorkRequestEventTypeId', N'IsActive', N'WorkRequestEventTypeName') AND [object_id] = OBJECT_ID(N'[WorkRequestEventTypes]'))
        SET IDENTITY_INSERT [WorkRequestEventTypes] ON;
    EXEC(N'INSERT INTO [WorkRequestEventTypes] ([WorkRequestEventTypeId], [IsActive], [WorkRequestEventTypeName])
    VALUES (13, CAST(0 AS bit), N''Export work request'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'WorkRequestEventTypeId', N'IsActive', N'WorkRequestEventTypeName') AND [object_id] = OBJECT_ID(N'[WorkRequestEventTypes]'))
        SET IDENTITY_INSERT [WorkRequestEventTypes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251106133447_AddDataExportLogsTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251106133447_AddDataExportLogsTable', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251120130549_RemoveWorkerRole'
)
BEGIN
    UPDATE dbo.AspNetUserRoles SET RoleId = 71 WHERE RoleId = 53
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251120130549_RemoveWorkerRole'
)
BEGIN
    DELETE FROM dbo.AspNetRoles WHERE Id = 53
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251120130549_RemoveWorkerRole'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251120130549_RemoveWorkerRole', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251201155838_AddNewFileManageWREventType'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'WorkRequestEventTypeId', N'IsActive', N'WorkRequestEventTypeName') AND [object_id] = OBJECT_ID(N'[WorkRequestEventTypes]'))
        SET IDENTITY_INSERT [WorkRequestEventTypes] ON;
    EXEC(N'INSERT INTO [WorkRequestEventTypes] ([WorkRequestEventTypeId], [IsActive], [WorkRequestEventTypeName])
    VALUES (20, CAST(0 AS bit), N''File Management'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'WorkRequestEventTypeId', N'IsActive', N'WorkRequestEventTypeName') AND [object_id] = OBJECT_ID(N'[WorkRequestEventTypes]'))
        SET IDENTITY_INSERT [WorkRequestEventTypes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251201155838_AddNewFileManageWREventType'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251201155838_AddNewFileManageWREventType', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251205093152_AddSubscribeEnum'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'WorkRequestEventTypeId', N'IsActive', N'WorkRequestEventTypeName') AND [object_id] = OBJECT_ID(N'[WorkRequestEventTypes]'))
        SET IDENTITY_INSERT [WorkRequestEventTypes] ON;
    EXEC(N'INSERT INTO [WorkRequestEventTypes] ([WorkRequestEventTypeId], [IsActive], [WorkRequestEventTypeName])
    VALUES (24, CAST(0 AS bit), N''Subscribe'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'WorkRequestEventTypeId', N'IsActive', N'WorkRequestEventTypeName') AND [object_id] = OBJECT_ID(N'[WorkRequestEventTypes]'))
        SET IDENTITY_INSERT [WorkRequestEventTypes] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251205093152_AddSubscribeEnum'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251205093152_AddSubscribeEnum', N'8.0.22');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219114902_AddDataExportJobsAndStatusEnum'
)
BEGIN
    ALTER TABLE [WorkRequestEvents] DROP CONSTRAINT [FK_WorkRequestEvents_WorkRequests_WorkRequestId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219114902_AddDataExportJobsAndStatusEnum'
)
BEGIN
    DROP INDEX [IX_WorkRequestEvents_WorkRequestId] ON [WorkRequestEvents];
    DECLARE @var11 sysname;
    SELECT @var11 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[WorkRequestEvents]') AND [c].[name] = N'WorkRequestId');
    IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [WorkRequestEvents] DROP CONSTRAINT [' + @var11 + '];');
    EXEC(N'UPDATE [WorkRequestEvents] SET [WorkRequestId] = 0 WHERE [WorkRequestId] IS NULL');
    ALTER TABLE [WorkRequestEvents] ALTER COLUMN [WorkRequestId] int NOT NULL;
    ALTER TABLE [WorkRequestEvents] ADD DEFAULT 0 FOR [WorkRequestId];
    CREATE INDEX [IX_WorkRequestEvents_WorkRequestId] ON [WorkRequestEvents] ([WorkRequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219114902_AddDataExportJobsAndStatusEnum'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'CREATE TABLE [DataExportStatuses] (
        [DataExportStatusId] int NOT NULL,
        [DataExportStatusName] nvarchar(max) NULL,
        [IsActive] bit NOT NULL,
        [PeriodEnd] datetime2 GENERATED ALWAYS AS ROW END HIDDEN NOT NULL,
        [PeriodStart] datetime2 GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
        CONSTRAINT [PK_DataExportStatuses] PRIMARY KEY ([DataExportStatusId]),
        PERIOD FOR SYSTEM_TIME([PeriodStart], [PeriodEnd])
    ) WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + N'].[DataExportStatusesHistory]))');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219114902_AddDataExportJobsAndStatusEnum'
)
BEGIN
    DECLARE @historyTableSchema sysname = SCHEMA_NAME()
    EXEC(N'CREATE TABLE [DataExportJobs] (
        [Id] int NOT NULL IDENTITY,
        [TrialId] int NOT NULL,
        [RequestedById] nvarchar(450) NULL,
        [RequestedAt] datetime2 NOT NULL,
        [StatusId] int NOT NULL,
        [FilePath] nvarchar(max) NULL,
        [CompletedAt] datetime2 NULL,
        [NotificationSent] bit NOT NULL,
        [PeriodEnd] datetime2 GENERATED ALWAYS AS ROW END HIDDEN NOT NULL,
        [PeriodStart] datetime2 GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
        CONSTRAINT [PK_DataExportJobs] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_DataExportJobs_AspNetUsers_RequestedById] FOREIGN KEY ([RequestedById]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_DataExportJobs_DataExportStatuses_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [DataExportStatuses] ([DataExportStatusId]) ON DELETE CASCADE,
        CONSTRAINT [FK_DataExportJobs_Trials_TrialId] FOREIGN KEY ([TrialId]) REFERENCES [Trials] ([TrialId]) ON DELETE CASCADE,
        PERIOD FOR SYSTEM_TIME([PeriodStart], [PeriodEnd])
    ) WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @historyTableSchema + N'].[DataExportJobsHistory]))');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219114902_AddDataExportJobsAndStatusEnum'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'DataExportStatusId', N'DataExportStatusName', N'IsActive') AND [object_id] = OBJECT_ID(N'[DataExportStatuses]'))
        SET IDENTITY_INSERT [DataExportStatuses] ON;
    EXEC(N'INSERT INTO [DataExportStatuses] ([DataExportStatusId], [DataExportStatusName], [IsActive])
    VALUES (0, N''Queued'', CAST(1 AS bit)),
    (1, N''Processing'', CAST(1 AS bit)),
    (2, N''Completed'', CAST(1 AS bit)),
    (3, N''Failed'', CAST(1 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'DataExportStatusId', N'DataExportStatusName', N'IsActive') AND [object_id] = OBJECT_ID(N'[DataExportStatuses]'))
        SET IDENTITY_INSERT [DataExportStatuses] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219114902_AddDataExportJobsAndStatusEnum'
)
BEGIN
    CREATE INDEX [IX_DataExportJobs_RequestedById] ON [DataExportJobs] ([RequestedById]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219114902_AddDataExportJobsAndStatusEnum'
)
BEGIN
    CREATE INDEX [IX_DataExportJobs_StatusId] ON [DataExportJobs] ([StatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219114902_AddDataExportJobsAndStatusEnum'
)
BEGIN
    CREATE INDEX [IX_DataExportJobs_TrialId] ON [DataExportJobs] ([TrialId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219114902_AddDataExportJobsAndStatusEnum'
)
BEGIN
    ALTER TABLE [WorkRequestEvents] ADD CONSTRAINT [FK_WorkRequestEvents_WorkRequests_WorkRequestId] FOREIGN KEY ([WorkRequestId]) REFERENCES [WorkRequests] ([WorkRequestId]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251219114902_AddDataExportJobsAndStatusEnum'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251219114902_AddDataExportJobsAndStatusEnum', N'8.0.22');
END;
GO

COMMIT;
GO

