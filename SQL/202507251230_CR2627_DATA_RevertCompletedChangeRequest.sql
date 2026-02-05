-- tp1045: Revert Completed Change Request see chareq https://ytu.york.ac.uk/ChaReq/WorkRequest/WorkRequestDetails?workRequestId=2627

DECLARE @WorkRequestId INT = 2483;

-- remove last entry from workrequestevents
DECLARE @WorkRequestEventId INT;
SELECT TOP 1	@WorkRequestEventId	= WorkRequestEventId
FROM			dbo.WorkRequestEvents
WHERE			WorkRequestId		= @WorkRequestId
ORDER BY		CreatedAt DESC


-- set status of request back to "Pending Work"  
UPDATE	dbo.WorkRequests
SET		StatusWorkRequestStatusId	= (
										SELECT	WorkRequestStatusId
										FROM	dbo.WorkRequestStatuses
										WHERE	WorkRequestStatusName = 'Pending Work'
									)
WHERE	WorkRequestId				= @WorkRequestId;

-- delete "Completed" event 
DELETE FROM dbo.WorkRequestEvents
WHERE		WorkRequestEventId = @WorkRequestEventId;