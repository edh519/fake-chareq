/* 20240710 aw1203: Ensure all dummy group emails are locked out */


UPDATE	dbo.AspNetUsers
SET		LockoutEnd			= '9999-12-31 23:59:59'
WHERE	Email IN (
		'ytu-redcap-group@york.ac.uk',
		'ytu-datavalidation-group@york.ac.uk',
		'ytu-developers-group@york.ac.uk'
)