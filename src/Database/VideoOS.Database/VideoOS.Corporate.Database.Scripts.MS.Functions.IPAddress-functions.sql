-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ConvertIPToInteger]') AND [type] in (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [ConvertIPToInteger]
GO

CREATE FUNCTION ConvertIPToInteger(@IPAddress AS VARCHAR(15))
RETURNS BIGINT
AS
BEGIN
	RETURN (
		CONVERT(BIGINT, PARSENAME(@IPAddress, 1)) +
		CONVERT(BIGINT, PARSENAME(@IPAddress, 2)) * 256 +
		CONVERT(BIGINT, PARSENAME(@IPAddress, 3)) * 65536 +
		CONVERT(BIGINT, PARSENAME(@IPAddress, 4)) * CAST(16777216 AS BIGINT))
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ConvertIntegerToIP]') AND [type] in (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [ConvertIntegerToIP]
GO

CREATE FUNCTION ConvertIntegerToIP(@IPAddress AS BIGINT)
RETURNS VARCHAR(15)
AS
BEGIN
	DECLARE @Octet1 TINYINT
	DECLARE @Octet2 TINYINT
	DECLARE @Octet3 TINYINT
	DECLARE @Octet4 TINYINT

	DECLARE @RestOfIP BIGINT

	SET @Octet1 = @IPAddress / CAST(16777216 AS BIGINT)
	SET @RestOfIP = @IPAddress - (@Octet1 * CAST(16777216 AS BIGINT))

	SET @Octet2 = @RestOfIP / 65536
	SET @RestOfIP = @RestOfIP - (@Octet2 * 65536)

	SET @Octet3 = @RestOfIP / 256
	SET @Octet4 = @RestOfIP - (@Octet3 * 256)

	RETURN(
		CONVERT(VARCHAR, @Octet1) + '.' +
		CONVERT(VARCHAR, @Octet2) + '.' +
		CONVERT(VARCHAR, @Octet3) + '.' +
		CONVERT(VARCHAR, @Octet4))
END
GO