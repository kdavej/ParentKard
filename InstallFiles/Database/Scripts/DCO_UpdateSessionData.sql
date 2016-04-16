SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DCO_UpdateSessionData] 
	@SessionID		VARCHAR(100),
	@SessionData	VARBINARY(MAX)
AS
BEGIN
	UPDATE DCO_Session SET SessionData = @SessionData, LastUpdateDate = GETDATE()WHERE SessionID = @SessionID
END