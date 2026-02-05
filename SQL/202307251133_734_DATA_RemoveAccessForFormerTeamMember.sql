-- 20230725 jw3677:	ChaReq 734 - Create a script to remove access to ChaReq for a former team-member.

UPDATE dbo.AspNetUsers
SET LockoutEnabled = 1,
	LockoutEnd = '9999-12-31' -- approx. max date to be locked out indefintely.
WHERE NormalizedUserName = UPPER('au633');