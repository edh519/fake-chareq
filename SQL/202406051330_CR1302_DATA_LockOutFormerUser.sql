/* 20240605 aw1203: Lock out user gap527 as they have left */

UPDATE dbo.AspNetUsers
SET	LockoutEnd = '9999-12-31 23:59:59 +00:00'
WHERE UserName = 'gap527'