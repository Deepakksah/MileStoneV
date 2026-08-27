-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_server_group_id_by_member_ship_server_group_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_server_group_id_by_member_ship_server_group_id]
GO

CREATE PROCEDURE [query_server_group_id_by_member_ship_server_group_id]
	@IDServerGroup [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	BEGIN
		SELECT
			[IDObject]
		FROM [ServerGroupMembership]
		WHERE [IDServerGroup] = @IDServerGroup
	END
END
GO
