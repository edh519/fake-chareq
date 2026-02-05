/* 20240130 aw1203: User dgh509 does not have email properly associated with ASP user tokens */

/* Test on LocalDB since there is no Dev version of ChaReq on TRAILSDEV */
/* To recreate the conditions in LIVE you'll need to add the following: */

/*
INSERT INTO	dbo.AspNetUsers (Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount)
VALUES	('c9968f2d-5007-49c9-97d4-abe5833896f1', 'dgh509', 'DGH509', 'debbie.sykes', 'DEBBIE.SYKES', 0, NULL, '6HO6NXNJKQ3ZCQSVSPEWEK6ZRF6DQR5I', '1f9e1f23-1d69-467e-acea-6d5c584ec0fc', NULL, 0, 0, NULL, 1, 0)

INSERT INTO	dbo.AspNetUserClaims (UserId, ClaimType, ClaimValue,SysStartTime, SysEndTime)
VALUES	('c9968f2d-5007-49c9-97d4-abe5833896f1', 'Username', 'dgh509', '2023-11-08 13:26:03', '9999-12-31 23:59:59')
*/

/* NB: Dev version of ChaReq does not have SysStartTime OR SysEndTime columns and these will need to be removed from the INSERT statement before execution */

DECLARE	@dgh509GUID NVARCHAR(450)	/* c9968f2d-5007-49c9-97d4-abe5833896f1 */
SELECT	@dgh509GUID					= Id
FROM	dbo.AspNetUsers
WHERE	UserName					= 'dgh509'

DECLARE	@dgh509StartTime DATETIME	/* 2023-11-08 13:26:03 */
SELECT	@dgh509GUID					= SysStartTime
FROM	dbo.AspNetUserClaims
WHERE	ClaimValue					= 'dgh509'

UPDATE	dbo.AspNetUsers
SET		Email						= 'debbie.sykes@york.ac.uk',
		NormalizedEmail				= 'DEBBIE.SYKES@YORK.AC.UK'
WHERE	UserName					= 'dgh509'

INSERT INTO	dbo.AspNetUserClaims (UserId, ClaimType, ClaimValue, SysStartTime,SysEndTime)
VALUES	(@dgh509GUID, 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname', 'Debbie', @dgh509StartTime, '9999-12-31 23:59:59'),
		(@dgh509GUID, 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name', 'Debbie Sykes', @dgh509StartTime, '9999-12-31 23:59:59')