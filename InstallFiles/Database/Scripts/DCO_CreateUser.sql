
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE dbo.DCO_CreateUser 
	@Username			VARCHAR(100),
	@UserPassword		VARCHAR(255),
	@FirstName			VARCHAR(100),
	@LastName			VARCHAR(100),
	@IsActive			BIT,
	@IsHidden			BIT,
	@ChangePassword		BIT
AS
BEGIN
	DECLARE @NOW DATETIME = GetDate()
	
	INSERT INTO DCO_User
	(UserName,UserPassword,FirstName,LastName,IsActive,IsHidden,ChangePassword,CreatedDate,LastUpdateDate)
	VALUES
	(@Username,dbo.fnEncryptString(@UserPassword),@FirstName,@LastName,@IsActive,@IsHidden,@ChangePassword,@NOW,@NOW)
	
	SELECT
		UserID,
		UserName,
		UserPassword,
		FirstName,
		LastName,
		IsActive,
		IsHidden,
		ChangePassword,
		CreatedDate,
		LastUpdateDate
	FROM
		DCO_User 
	WHERE
		UserID = SCOPE_IDENTITY()
	
END
GO
