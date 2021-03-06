
GO
/****** Object:  StoredProcedure [dbo].[DCO_ValidateSession]    Script Date: 05/30/2014 18:25:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DCO_ValidateSession]
	@SessionID	VARCHAR(100)
AS
BEGIN
	SELECT 
		SessionID,
		SessionData
	FROM
		DCO_Session
	WHERE 
		SessionID = @SessionID
		AND GETDATE() - LastUpdateDate < 1
END