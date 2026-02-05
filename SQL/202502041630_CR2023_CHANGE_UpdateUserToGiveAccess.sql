-- 20250204 edh519:	Created script to update name and/or email of a user by their username.

-- Change the following 4 variables to their expected values.
DECLARE @Username	VARCHAR(MAX) = 'rmc538';				-- case insensitive, e.g. 'abc123'
DECLARE @Firstname	VARCHAR(MAX) = 'Rachel';					-- e.g. 'Firstname'
DECLARE @FullName	VARCHAR(MAX) = 'Rachel Bottomley-Wise';			-- e.g. 'Firstname Lastname'
DECLARE @Email		VARCHAR(MAX) = 'rachel.bottomley-wise@york.ac.uk';	-- e.g. 'firstname.lastname@york.ac.uk'

---
-- Select the UserId and the existing email in system for safely updating where needed only.
DECLARE @UserId VARCHAR(MAX) = (SELECT Id FROM dbo.AspNetUsers WHERE NormalizedUserName = UPPER(@Username));
DECLARE @OldEmail VARCHAR(MAX) = (SELECT Email FROM dbo.AspNetUsers WHERE NormalizedUserName = UPPER(@Username));

-- Update all instances of @OldEmail with new @Email where they are different. NB: This is required only for tables still using email as a FK.
UPDATE dbo.FileUploads
SET		UploadedBy	= @Email
WHERE	UploadedBy	=	@OldEmail
	AND UploadedBy	!=	@Email;
UPDATE dbo.FinalAuthorisations
SET		CompletedBy	= @Email
WHERE	CompletedBy	=	@OldEmail
	AND CompletedBy	!=	@Email;
UPDATE dbo.InitialAuthorisations
SET		DecisionBy	= @Email
WHERE	DecisionBy	=	@OldEmail
	AND DecisionBy	!=	@Email;
UPDATE dbo.Notifications
SET		Recipient	= @Email
WHERE	Recipient	=	@OldEmail
	AND Recipient	!=	@Email;
UPDATE dbo.Notifications
SET		CreatedBy	= @Email
WHERE	CreatedBy	=	@OldEmail
	AND CreatedBy	!=	@Email;
UPDATE dbo.WorkRequests
SET		CreatedBy	= @Email
WHERE	CreatedBy	=	@OldEmail
	AND CreatedBy	!=	@Email;
UPDATE dbo.WorkRequests
SET		LastEditedBy	= @Email
WHERE	LastEditedBy	=	@OldEmail
	AND LastEditedBy	!=	@Email;


-- Update the AspNetUsers and AspNetUserClaims tables.
-- Update the main record's email address, if either has changed.
UPDATE dbo.AspNetUsers
SET		Email			= @Email,
		NormalizedEmail = UPPER(@Email)
WHERE	Id = @UserId
	AND Email != @Email 
	AND NormalizedEmail != UPPER(@Email);

-- Update the names associated with the above main record, if either has changed.
UPDATE dbo.AspNetUserClaims
SET		ClaimValue = @Firstname
WHERE	UserId		= @UserId
	AND ClaimType	= 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname'
	AND ClaimValue  != @Firstname;
UPDATE dbo.AspNetUserClaims
SET		ClaimValue = @FullName
WHERE	UserId		= @UserId
	AND ClaimType	= 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'
	AND ClaimValue  != @FullName;


---
-- Confirm changes completed as expected. Output the now changed records for review.
SELECT * FROM dbo.AspNetUsers WHERE	Id = @UserId;
SELECT * FROM dbo.AspNetUserClaims WHERE UserId = @UserId;