-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_roles_by_claim]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_roles_by_claim]
GO

CREATE PROCEDURE [security_query_roles_by_claim]
	@LoginProviderId	UNIQUEIDENTIFIER,
	@ClaimName			NVARCHAR(128),
	@ClaimValue			NVARCHAR(512)
AS
BEGIN
	SET NOCOUNT ON

	SELECT  [IDRole]
	FROM    SecurityRoleClaim rc
	INNER JOIN SecurityClaim c ON c.IDClaim = rc.IDClaim
	AND (
		-- Do an 'Ordinal' string compare
	(c.CaseSensitive = 1 AND rc.ClaimValue COLLATE Latin1_General_100_BIN = @ClaimValue COLLATE Latin1_General_100_BIN)
	OR
	-- Do an 'OrdinalIgnoreCase' string compare
	(c.CaseSensitive = 0 AND UPPER(rc.ClaimValue) COLLATE Latin1_General_100_BIN = UPPER(@ClaimValue) COLLATE Latin1_General_100_BIN)
	)
	WHERE (c.LoginProviderId = @LoginProviderId AND c.ClaimName = @ClaimName COLLATE Latin1_General_100_BIN)
END
GO