SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fnEncryptString] (
	@password	VARCHAR(256)
)

RETURNS	VARCHAR(8000)

AS

BEGIN
	DECLARE	@Box TABLE (i TINYINT, v TINYINT)
	
	DECLARE	
		@index		SMALLINT,
		@i			SMALLINT,
		@j			SMALLINT,
		@t			TINYINT,
		@k			SMALLINT,
  		@cipherBy	TINYINT,
  		@cipher		VARCHAR(8000)	

	INSERT	@Box
	( i, v )
		SELECT 
			i,
			v
		FROM dbo.fnInitRc4(@password)

	SELECT @index = 1, @i = 0, @j = 0, @cipher = ''

	WHILE @index <= DATALENGTH(@password)
		BEGIN
			SELECT	@i = (@i + 1) % 256

			SELECT	@j = (@j + b.v) % 256 FROM @Box b WHERE b.i = @i

			SELECT @t = v FROM @Box WHERE i = @i

			UPDATE @Box
			SET	v = (SELECT w.v FROM @Box w WHERE w.i = @j)
			FROM @Box b
			WHERE b.i = @i

			UPDATE	@Box SET v = @t WHERE i = @j

			SELECT @k = v FROM @Box WHERE i = @i

			SELECT @k = (@k + v) % 256 FROM @Box WHERE i = @j

			SELECT @k = v FROM @Box WHERE i = @k

			SELECT
				@cipherBy = ASCII(SUBSTRING(@password, @index, 1)) ^ @k,
				@cipher = @cipher + CHAR(@cipherBy)

			SELECT @index = @index  +1
      	END

	RETURN	@cipher
END