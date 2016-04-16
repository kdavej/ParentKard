
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fnInitRc4] (
	@password VARCHAR(256)
)

RETURNS @box TABLE (i TINYINT, v TINYINT)

AS

BEGIN
	DECLARE	@Key TABLE (
		i TINYINT, 
		v TINYINT
		)

	DECLARE
		@index			SMALLINT,
		@passwordLen	TINYINT,
		@t				TINYINT,
		@b				SMALLINT		

	SELECT	@index = 0, @passwordLen = LEN(@password)

	WHILE @index <= 255
		BEGIN
			INSERT	@Key ( i, v )
			VALUES	( @index,  ASCII(SUBSTRING(@password, @index % @passwordLen + 1, 1)) )

			INSERT	@box ( i, v )
			VALUES	( @index, @index )

			SELECT	@index = @index + 1
		END

	SELECT	@index = 0, @b = 0

	WHILE @index <= 255
		BEGIN
			SELECT @b = (@b + b.v + k.v) % 256
			FROM @box AS b
			JOIN @Key AS k ON k.i = b.i
			WHERE b.i = @index

			SELECT	@t = v FROM @box WHERE i = @index

			UPDATE @box
			SET	v = (SELECT b2.v FROM @box b2 WHERE b2.i = @b)
			FROM @box b1
			WHERE b1.i = @index

			UPDATE @box
			SET v = @t
			WHERE i = @b

			SELECT @index = @index + 1
		END

	RETURN
END