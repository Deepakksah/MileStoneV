-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_claim_query_ids_by_provider]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [security_claim_query_ids_by_provider]
GO

CREATE PROCEDURE [security_claim_query_ids_by_provider]
	@loginProviderId	UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON
 
	SELECT	[IDClaim]
	FROM	[SecurityClaim]
	WHERE	[LoginProviderId] = @loginProviderId
END
GO