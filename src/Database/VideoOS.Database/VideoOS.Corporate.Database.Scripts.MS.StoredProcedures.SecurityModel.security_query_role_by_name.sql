-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_role_by_name]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_role_by_name]
GO

CREATE PROCEDURE [security_query_role_by_name]
	@Name [NameType]
AS
BEGIN
	SET NOCOUNT ON;

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
	FROM [SecurityRoles]
	WHERE [Name] = @Name
END
GO