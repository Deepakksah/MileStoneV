-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_roles_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_roles_by_ids]
GO

CREATE PROCEDURE [security_query_roles_by_ids]
	@roleIds tvp_StringTableGuidType READONLY
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		[IDRole],
		[Name],
		[Description],
		[Type],
		[DualRequired],
		[PtzReservedAsAnonymous],
		[AllowMobileClientLogOn],
		[AllowSmartClientLogOn],
		[AllowWebClientLogOn],
		[LastModified]
	FROM [SecurityRoles] sr
	INNER JOIN @roleIds ids ON sr.[IDRole] = ids.[ID]
END
GO