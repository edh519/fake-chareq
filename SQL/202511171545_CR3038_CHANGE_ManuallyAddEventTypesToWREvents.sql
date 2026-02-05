/* 20251117 aw1203: Manually add EventTypeWorkRequestEventTypeId's to Events that have a null event type Id */

UPDATE	dbo.WorkRequestEvents 
SET		EventTypeWorkRequestEventTypeId = 11
WHERE	EventTypeWorkRequestEventTypeId IS NULL