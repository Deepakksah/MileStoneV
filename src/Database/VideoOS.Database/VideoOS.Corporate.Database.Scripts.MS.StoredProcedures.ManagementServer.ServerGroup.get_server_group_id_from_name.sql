-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[get_server_group_id_from_name]') AND type in (N'P', N'PC'))
DROP PROCEDURE [get_server_group_id_from_name]
GO

CREATE PROCEDURE [get_server_group_id_from_name]
	@parent_id [GuidType],
	@name [NameType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT sg.[IDServerGroup] AS [pkid]
	FROM [ServerGroup] sg
	WHERE sg.[IDParentGroup] = @parent_id
		AND sg.[Name] = @name
END
GO