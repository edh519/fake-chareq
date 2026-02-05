-- 20241114 jw3677: CR1702 Delete unused and never used user 'EMAILEDIT@YORK.AC.UK'.

DECLARE @UserId VARCHAR (450) = (SELECT Id FROM dbo.AspNetUsers WHERE NormalizedEmail = 'EMAILEDIT@YORK.AC.UK');

DELETE FROM dbo.AspNetUserClaims WHERE UserId = @UserId;
DELETE FROM dbo.AspNetUsers WHERE Id = @UserId;