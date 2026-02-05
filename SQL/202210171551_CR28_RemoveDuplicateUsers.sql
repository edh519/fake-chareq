--ChaReq 20221017 aw1203: Removing users added twice
SELECT * 
FROM dbo.AspNetUsers U
WHERE U.UserName IN (
	'caroline.fairhurst@york.ac.uk',
	'grace.ocarroll@york.ac.uk',
	'stephen.brealey@york.ac.uk'
)