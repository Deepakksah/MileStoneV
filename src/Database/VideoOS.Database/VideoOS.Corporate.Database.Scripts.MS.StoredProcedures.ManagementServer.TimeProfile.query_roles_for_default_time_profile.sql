-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_roles_for_default_time_profile]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_roles_for_default_time_profile]
GO

CREATE PROCEDURE [query_roles_for_default_time_profile]
	@IDTimeProfile GuidType
AS
BEGIN
	SET NOCOUNT ON

	SELECT [IDRole]
	FROM [TimeProfileDefaultRoleRelation]
	WHERE IDTimeProfile = @IDTimeProfile
END
GO