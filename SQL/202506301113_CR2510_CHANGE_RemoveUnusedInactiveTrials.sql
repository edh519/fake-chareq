-- 202506301111 edh519: Remove duplicate, inactive trials with no associated work requests

DECLARE	@HamletInactiveTrialID INT
SELECT	@HamletInactiveTrialID		= TrialId
FROM	dbo.Trials
WHERE	TrialName			= 'HAMLET'
AND IsActive = 0

DECLARE	@RedConInactiveTrialID INT
SELECT	@RedConInactiveTrialID		= TrialId
FROM	dbo.Trials
WHERE	TrialName			= 'REDCon'
AND IsActive = 0

DELETE FROM [dbo].[Trials]
WHERE [TrialId] IN (@HamletInactiveTrialID, @RedConInactiveTrialID);
