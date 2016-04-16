SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DCO_CreateSession] (
	@SessionID		VARCHAR(100),
	@SessionData	VARBINARY(MAX)
)
AS

INSERT INTO DCO_Session
	(	SessionID, CreatedDate, LastUpdateDate, SessionData )  
VALUES
	(	@SessionID, GETDATE(), GETDATE(), @SessionData )

RETURN