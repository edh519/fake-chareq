SELECT * FROM dbo.InitialAuthorisations
SELECT * FROM dbo.FinalAuthorisations
SELECT * FROM dbo.ProcessDeviationReasons
SELECT * FROM dbo.WorkRequestEvents

-- Script to migrate existing authorisations to the new events model.

-- Event Types
DECLARE @ClosedEventTypeId INT = (SELECT WorkRequestEventTypeId FROM dbo.WorkRequestEventTypes WHERE WorkRequestEventTypeName = 'Closing');
DECLARE @RequestingChangesEventTypeId INT = (SELECT WorkRequestEventTypeId FROM dbo.WorkRequestEventTypes WHERE WorkRequestEventTypeName = 'Requesting Changes');
DECLARE @ApprovalEventTypeId INT = (SELECT WorkRequestEventTypeId FROM dbo.WorkRequestEventTypes WHERE WorkRequestEventTypeName = 'Approving');
DECLARE @CompletingEventTypeId INT = (SELECT WorkRequestEventTypeId FROM dbo.WorkRequestEventTypes WHERE WorkRequestEventTypeName = 'Completing');

-- Status Types
DECLARE @ClosedStatusId INT = (SELECT WorkRequestStatusId FROM dbo.WorkRequestStatuses WHERE WorkRequestStatusName = 'Abandoned');
DECLARE @RequestChangesStatusId INT = (SELECT WorkRequestStatusId FROM dbo.WorkRequestStatuses WHERE WorkRequestStatusName = 'Pending Requester');
DECLARE @ChangesMadeStatusId INT = (SELECT WorkRequestStatusId FROM dbo.WorkRequestStatuses WHERE WorkRequestStatusName = 'Pending Initial Approval');
DECLARE @PendingWorkStatusId INT = (SELECT WorkRequestStatusId FROM dbo.WorkRequestStatuses WHERE WorkRequestStatusName = 'Pending Work');
DECLARE @CompletedStatusId INT = (SELECT WorkRequestStatusId FROM dbo.WorkRequestStatuses WHERE WorkRequestStatusName = 'Completed');


-- Closing
-- Closed requests will have an initial auth to explain closure. Closure cannot occur any other way.
INSERT INTO dbo.WorkRequestEvents
	(WorkRequestId, EventTypeWorkRequestEventTypeId, DurationDays, Content, CreatedById, CreatedAt)
SELECT 
	ia.WorkRequestId, 
	@ClosedEventTypeId,
	ia.EstimatedTimeImpact,
	ia.WorkRequiredDecription,
	aspUser.Id,
	ia.DecisionDateTime
FROM dbo.InitialAuthorisations ia
JOIN dbo.AspNetUsers aspUser ON aspUser.NormalizedEmail = UPPER(ia.DecisionBy)
JOIN dbo.WorkRequests wr ON wr.WorkRequestId = ia.WorkRequestId
WHERE wr.StatusWorkRequestStatusId = @ClosedStatusId;


-- Requesting Changes
-- The initial authorisation will contain the request for changes when either it is 'Pending Requester' or it is 'Pending Initial Approval' whilst existing.
INSERT INTO dbo.WorkRequestEvents
	(WorkRequestId, EventTypeWorkRequestEventTypeId, DurationDays, Content, CreatedById, CreatedAt)
SELECT 
	ia.WorkRequestId, 
	@RequestingChangesEventTypeId,
	ia.EstimatedTimeImpact,
	ia.WorkRequiredDecription,
	aspUser.Id,
	ia.DecisionDateTime
FROM dbo.InitialAuthorisations ia
JOIN dbo.AspNetUsers aspUser ON aspUser.NormalizedEmail = UPPER(ia.DecisionBy)
JOIN dbo.WorkRequests wr ON wr.WorkRequestId = ia.WorkRequestId
WHERE wr.StatusWorkRequestStatusId IN (@RequestChangesStatusId, @ChangesMadeStatusId);


-- Initial Approval
-- The initial authorisation will contain the approval when it has passed the approval already, i.e. it is approved or has been completed.
INSERT INTO dbo.WorkRequestEvents
	(WorkRequestId, EventTypeWorkRequestEventTypeId, DurationDays, Content, CreatedById, CreatedAt)
SELECT 
	ia.WorkRequestId, 
	@ApprovalEventTypeId,
	ia.EstimatedTimeImpact,
	ia.WorkRequiredDecription,
	aspUser.Id,
	ia.DecisionDateTime
FROM dbo.InitialAuthorisations ia
JOIN dbo.AspNetUsers aspUser ON aspUser.NormalizedEmail = UPPER(ia.DecisionBy)
JOIN dbo.WorkRequests wr ON wr.WorkRequestId = ia.WorkRequestId
WHERE wr.StatusWorkRequestStatusId IN (@PendingWorkStatusId, @CompletedStatusId);


-- Completed
-- Any final authorisations represent a completed message. Only complication is ensuring process deviation reasons are appended appropriately.
INSERT INTO dbo.WorkRequestEvents
	(WorkRequestId, EventTypeWorkRequestEventTypeId, DurationDays, Content, CreatedById, CreatedAt)
SELECT 
	fa.WorkRequestId, 
	@CompletingEventTypeId,
	fa.ActualTimeImpactDays,
	'Reference: ' + fa.WorkReference + CHAR(13)+CHAR(10) + fa.Comments
	aspUser.Id,
	fa.CompletedDateTime
FROM dbo.FinalAuthorisations fa
JOIN dbo.AspNetUsers aspUser ON aspUser.NormalizedEmail = UPPER(fa.CompletedBy)
LEFT JOIN dbo.ProcessDeviationReasons pdr ON pdr.ProcessDeviationReasonId = fa.ProcessDeviationReasonId;