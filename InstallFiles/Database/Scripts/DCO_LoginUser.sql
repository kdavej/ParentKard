SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DCO_LoginUser]
		@Username	VARCHAR(100),
		@Password	VARCHAR(255)
AS
BEGIN

	DECLARE @UserID	INT

	SELECT @UserID = du.UserID
	FROM DCO_User du
	WHERE UserName = @Username AND UserPassword COLLATE Latin1_General_CS_AS = dbo.fnEncryptString(@Password) AND IsActive = 1

	SELECT 
		du.UserID,
		du.UserName,
		du.FirstName,
		du.LastName,
		du.IsActive,
		du.IsHidden,				
		du.CreatedDate,		
		du.LastUpdateDate,
		du.ChangePassword
	FROM DCO_User du	
	WHERE UserID = @UserID

END