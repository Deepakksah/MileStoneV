-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_mip_object_by_id]') AND type = 'P')
DROP PROCEDURE [admin_query_mip_object_by_id]
GO

CREATE PROCEDURE [admin_query_mip_object_by_id]
	@IDMIPObject [UNIQUEIDENTIFIER]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		m.[IDMIPObject],
		m.[Name],
		m.[Kind],
		m.[SubKind],
		m.[PluginId],
		m.[CustomData],
		m.[LastModified],
		m.[CreatedDate],
		COALESCE(sg.[RelationIds], sgm.IDServerGroup) AS [RelationIds]
	FROM
		[MIPObject] m
		LEFT JOIN [ServerGroupMembership] sgm ON sgm.IDObject = m.IDMIPObject
		LEFT JOIN [ServerGroup] sg ON sg.IDServerGroup = sgm.IDServerGroup
	WHERE
		m.[IDMIPObject] = @IDMIPObject
END
GO