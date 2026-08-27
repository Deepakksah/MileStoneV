-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @rowCount INT

DELETE  rc
FROM    [SecurityRoleClaim] rc
		INNER JOIN [SecurityClaim] sc ON sc.[IDClaim] = rc.[IDClaim]
WHERE   TRY_CONVERT(UNIQUEIDENTIFIER, [LoginProviderId]) IS NULL

SET @rowCount = @@ROWCOUNT
IF (@rowCount <> 0)
BEGIN
	DELETE	[SecurityUserUpdatedCache]
END

DELETE	[SecurityClaim]
WHERE	TRY_CONVERT(UNIQUEIDENTIFIER, [LoginProviderId]) IS NULL
GO
