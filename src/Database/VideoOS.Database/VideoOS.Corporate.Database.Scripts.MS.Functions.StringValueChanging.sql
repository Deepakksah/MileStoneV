-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[StringValueChanging]') AND type in (N'FN'))
DROP FUNCTION [StringValueChanging]
GO

-- Function to use in a WHERE clause if only wanting to update a row if one or more specified values are changed
--
-- E.g.
-- UPDATE [table] 
-- SET col1 = @col1
-- WHERE dbo.StringValueChanging(col1, @col1) = 1

CREATE FUNCTION [StringValueChanging]
(
	@value1 AS NVARCHAR(MAX),
	@value2 AS NVARCHAR(MAX)
)
RETURNS BIT
AS BEGIN
	DECLARE @result BIT
	DECLARE @temp1 NVARCHAR(4000)
	DECLARE @temp2 NVARCHAR(4000)
	DECLARE @index INT

	IF (@value1 IS NULL AND @value2 IS NULL)			-- Both are NULL
	BEGIN
		SET @result = 0
	END
	ELSE IF (@value1 IS NULL OR @value2 IS NULL)		-- Only one of them is NULL
	BEGIN
		SET @result = 1
	END
	ELSE												-- None of them are NULL
	BEGIN
		IF (LEN(@value1) <> LEN(@value2))
			SET @result = 1
		ELSE
		BEGIN
			SET @index = 0
			WHILE @index < LEN(@value1) -- We already know that @value1 and @value2 have same length
			BEGIN
				SET @temp1 = SUBSTRING(@value1, @index, @index + 4000)
				SET @temp2 = SUBSTRING(@value2, @index, @index + 4000)

				IF ((@temp1 <> @temp2) OR HASHBYTES('SHA1', @temp1) <> HASHBYTES('SHA1', @temp2))
				BEGIN
					SET @result = 1
					BREAK
				END

				SET @result = 0
				SET @index = @index + 4000
			END
		END
	END

	RETURN @result
END
GO