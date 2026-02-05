/* 20231018 aw1203: Remove all instances of au633 from ChaReq */

UPDATE	dbo.AspNetUsers
SET		LockoutEnd						= '9999-12-31 23:59:59.9999999'
WHERE	UserName						= 'au633'