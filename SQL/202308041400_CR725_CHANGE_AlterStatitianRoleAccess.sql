/* 20230804 aw1203: Alter statistician (from provided dohs-she-group@york.ac.uk email list) user role to allow them to approve requests, assign and be assigned */
/* ChaReq user roles: a user is a User, someone who can authorise is a Worker, admin is Authoriser */


DECLARE @WorkerUserRoleId INT
SELECT	@WorkerUserRoleId = Id
FROM	dbo.AspNetRoles
WHERE	Name = 'Worker';

WITH CTE_StatisticianUserIds AS (
	SELECT	ANUR.UserId,
			ANUR.RoleId,
			ANU.Email,
			ANR.Name
	FROM	dbo.AspNetUserRoles ANUR
	JOIN	dbo.AspNetUsers ANU
	ON		ANU.Id = ANUR.UserId
	JOIN	dbo.AspNetRoles ANR
	ON		ANR.Id = ANUR.RoleId
	WHERE	ANU.Email IN (
		'abbie.cowling@york.ac.uk',
		'ada.keding@york.ac.uk',
		'alex.mitchell@york.ac.uk',
		'caroline.fairhurst@york.ac.uk',
		'catherine.hewitt@york.ac.uk',
		'charlie.peck@york.ac.uk',
		'charlie.welch@york.ac.uk',
		'emma.brooks@york.ac.uk',
		'fraser.wiggins@york.ac.uk',
		'gill.parkinson@york.ac.uk',
		'han-i.wang@york.ac.uk',
		'izzy.coleman@york.ac.uk',
		'jinshuo.li@york.ac.uk',
		'jobie.kirkwood@york.ac.uk',
		'kalpita.baird@york.ac.uk',
		'laura.mandefield@york.ac.uk',
		'luke.strachan@york.ac.uk',
		'matthew.bailey@york.ac.uk',
		'nassos.gkekas@york.ac.uk',
		'qi.wu@york.ac.uk',
		'qian.zhao@york.ac.uk',
		'sally.baker@york.ac.uk',
		'sarah.gardner@york.ac.uk',
		'sfh514@york.ac.uk',
		'steve.parrott@york.ac.uk',
		'tanya.pawson@york.ac.uk',
		'val.wadsworth@york.ac.uk'
	)
	AND ANR.Name = 'User'
)
UPDATE	CTE_StatisticianUserIds
SET		RoleId = @WorkerUserRoleId