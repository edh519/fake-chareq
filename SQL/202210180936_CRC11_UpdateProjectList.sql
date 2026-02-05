--20221018 aw1203: Script to update trials from 'other' to specific.

DECLARE @CALMTrialId AS INT
SELECT @CALMTrialId = TrialId
FROM dbo.Trials
WHERE TrialName = 'CALM'

DECLARE @REDUCETrialId AS INT
SELECT @REDUCETrialId = TrialId
FROM dbo.Trials
WHERE TrialName = 'REDUCE'

DECLARE @DatReqTrialId AS INT
SELECT @DatReqTrialId = TrialId
FROM dbo.Trials
WHERE TrialName = 'DatReq'

UPDATE dbo.WorkRequests
SET TrialId = @CALMTrialId, 
	TrialOther = NULL
WHERE TrialOther LIKE 'CALM%'

UPDATE dbo.WorkRequests
SET TrialId = @REDUCETrialId, 
	TrialOther = NULL
WHERE TrialOther LIKE 'REDUCE%'

UPDATE dbo.WorkRequests
SET TrialId = @DatReqTrialId, 
	TrialOther = NULL
WHERE TrialOther LIKE 'DatReq%'