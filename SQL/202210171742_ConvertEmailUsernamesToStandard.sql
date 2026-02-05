--20221017 cha519: script to update email usernames to uni usernames

--RUN ON LIVE INSTANCE ONLY
UPDATE	u
SET		Username = o.Username,
		NormalizedUsername = UPPER(o.Username)
FROM		[ChaReq].[dbo].AspNetUsers u
INNER JOIN	[TRIALSDEVDB].[YTULib].[dbo].[Operative] o
ON			u.Username = o.Email
WHERE		u.Username LIKE '%@york.ac.uk'; --38rows (live)


-- check it all looks right
SELECT *
FROM [ChaReq].[dbo].AspNetUsers u
