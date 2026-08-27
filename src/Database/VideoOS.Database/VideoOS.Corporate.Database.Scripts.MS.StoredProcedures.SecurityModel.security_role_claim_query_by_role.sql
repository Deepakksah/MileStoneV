-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_role_claim_query_by_role]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [security_role_claim_query_by_role]
GO

CREATE PROCEDURE [security_role_claim_query_by_role]
	@IDRole		[GuidType]
AS
BEGIN
	SET NOCOUNT ON
 
	SELECT	sc.[LoginProviderId],
			sc.[ClaimName],
			rc.[ClaimValue]
	FROM    [SecurityRoleClaim] rc
			INNER JOIN [SecurityClaim] sc ON sc.[IDClaim] = rc.[IDClaim]
	WHERE	rc.[IDRole] = @IDRole
END
GO