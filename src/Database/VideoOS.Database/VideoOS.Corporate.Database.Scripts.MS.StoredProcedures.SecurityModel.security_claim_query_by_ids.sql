-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_claim_query_by_ids]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [security_claim_query_by_ids]
GO

CREATE PROCEDURE [security_claim_query_by_ids]
	@ids tvp_IdTableInt READONLY
AS
BEGIN
	SET NOCOUNT ON
 
	SELECT	sc.[IDClaim],
			sc.[LoginProviderId],
			sc.[ClaimName],
			sc.[CaseSensitive],
			sc.[DisplayName]
	FROM	[SecurityClaim] sc
	INNER JOIN @ids ids ON sc.[IDClaim] = ids.[ID]
END
GO