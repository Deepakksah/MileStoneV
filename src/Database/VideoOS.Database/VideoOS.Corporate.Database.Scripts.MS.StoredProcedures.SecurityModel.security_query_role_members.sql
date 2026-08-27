-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_role_members]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_role_members]
GO

CREATE PROCEDURE [security_query_role_members]
	@IDRole								GuidType,
	@IncludeDynamicallyAddedIdentities	BIT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	sri.[SID]
	FROM	[SecurityRolesIdentity] sri
	INNER JOIN SecurityIdentity si ON sri.SID = si.SID
	WHERE	[IDRole] = @IDRole 
			AND (@IncludeDynamicallyAddedIdentities = 1 OR [IsStatic] = 1)
			AND si.[Deleted] = 0
END
GO