/* 20240327 aw1203: YTU-Redcap group has two roles; cannot remove superfluous role via frontend */

DECLARE	@YTURedcapUserId NVARCHAR(450)
SELECT	@YTURedcapUserId				= Id
FROM	dbo.AspNetUsers
WHERE	UserName						= 'ytu-redcap-group'

DECLARE	@WorkerRoleId NVARCHAR(450)
SELECT	@WorkerRoleId					= Id
FROM	dbo.AspNetRoles
WHERE	[Name]							= 'Worker'

/* 
SELECT	UserId,
        RoleId,
        SysStartTime,
        SysEndTime,
        PeriodEnd,
        PeriodStart
*/
DELETE
FROM	dbo.AspNetUserRoles
WHERE	UserId							= @YTURedcapUserId
AND		RoleId							= @WorkerRoleId