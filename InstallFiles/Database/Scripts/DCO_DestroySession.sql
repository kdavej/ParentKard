GO
/****** Object:  StoredProcedure [dbo].[WEB_DestroySession]    Script Date: 05/30/2014 18:18:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DCO_DestroySession]
	@SessionID	VARCHAR(100)
AS
BEGIN
	DELETE FROM DCO_Session WHERE SessionID = @SessionID;	
END