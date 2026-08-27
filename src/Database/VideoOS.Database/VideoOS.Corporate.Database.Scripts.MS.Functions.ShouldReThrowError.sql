-- This header is used by the dbupdate7 tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ShouldReThrowError]') AND type in (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [ShouldReThrowError]
GO

CREATE FUNCTION [ShouldReThrowError](@ERROR_NUMBER INT, @ERROR_SEVERITY INT)
RETURNS BIT
AS
BEGIN
	IF (@ERROR_NUMBER >= 60000)
		RETURN 1

	IF(@ERROR_NUMBER = 1225 AND @ERROR_SEVERITY = 20)
		RETURN 1

	IF (@ERROR_NUMBER = 1205
		OR @ERROR_NUMBER = 8628
		OR @ERROR_NUMBER = 8645
		OR @ERROR_NUMBER = 2801
		OR @ERROR_NUMBER = 233
		OR @ERROR_NUMBER = 10053
		OR @ERROR_NUMBER = 10054
		OR @ERROR_NUMBER = -2
		OR @ERROR_NUMBER = 10060
		OR @ERROR_NUMBER = 983
		OR @ERROR_NUMBER = 976
	)
		RETURN 1

	RETURN 0
END
GO