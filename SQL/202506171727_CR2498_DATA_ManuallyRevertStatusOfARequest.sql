-- manually enter @WorkRequestId, from searching WorkRequests table
DECLARE @WorkRequestId INT = 2497;

-- dynamically get matching @WorkRequestEventId to remove last entry
DECLARE @WorkRequestEventId INT;
SELECT TOP 1	@WorkRequestEventId	= WorkRequestEventId
FROM			dbo.WorkRequestEvents
WHERE			WorkRequestId		= @WorkRequestId
ORDER BY		CreatedAt DESC

-- dynamically get matching @ProcessDeviationReasonId to remove
DECLARE @ProcessDeviationReasonId INT;
SELECT	@ProcessDeviationReasonId	= ProcessDeviationReasonId
FROM	dbo.WorkRequests
WHERE	WorkRequestId				= @WorkRequestId

-- set status of request back to "Pending Work"  
UPDATE	dbo.WorkRequests
SET		StatusWorkRequestStatusId	= (
										SELECT	StatusWorkRequestStatusId
										FROM	dbo.WorkRequestStatuses
										WHERE	WorkRequestStatusName = 'Pending Work'
									)
WHERE	WorkRequestId				= @WorkRequestId;

-- delete "Completed" event record for request
DELETE FROM dbo.WorkRequestEvents
WHERE		WorkRequestEventId = @WorkRequestEventId;

-- remove link to process deviation record
UPDATE	dbo.WorkRequests
SET		ProcessDeviationReasonId	= NULL
WHERE	WorkRequestId				= @WorkRequestId;

-- delete process deviation record
DELETE FROM dbo.ProcessDeviationReasons
WHERE		ProcessDeviationReasonId = @ProcessDeviationReasonId;