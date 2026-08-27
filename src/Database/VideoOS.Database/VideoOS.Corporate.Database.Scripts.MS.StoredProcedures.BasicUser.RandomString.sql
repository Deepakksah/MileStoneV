-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RandomString]') AND type in (N'P', N'PC'))
DROP PROCEDURE [RandomString]
GO

CREATE PROCEDURE [RandomString]
	@length INT,
	@randomString NVARCHAR(MAX) OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @ValidChars varchar(255)
	SET @ValidChars = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-=+&$!?'
	
	DECLARE @retVal PASSWORDTYPE
	SET @retVal = ''

	DECLARE @counter INT
	
	DECLARE @randNumber TINYINT
	
	SET @counter = 0

	WHILE @counter < @length
	BEGIN
		SET @counter = @counter + 1
		SET @randNumber = Convert(TINYINT, ((LEN(@ValidChars)) * RAND() + 1))
		SET @retVal = @retVal + SUBSTRING(@ValidChars, @randNumber, 1)
	END
	
	SET @randomString = @retVal
END
GO