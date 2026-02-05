  -- 20230831 jw3677:   Updates status of singular work request (ID 806) to Pending Initial Approval/Awaiting Decision.
  
  UPDATE dbo.WorkRequests 
  SET StatusWorkRequestStatusId = (SELECT WorkRequestStatusId FROM WorkRequestStatuses WHERE WorkRequestStatusName = 'Pending Initial Approval')
  WHERE WorkRequestId = 806;