-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[client_get_viewgroupdata]') AND type in (N'P', N'PC'))
DROP PROCEDURE [client_get_viewgroupdata]
GO

CREATE PROCEDURE [client_get_viewgroupdata]
	@IDViewGroup [GuidType],
	@IDViewGroupType [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT 
		vg.[IDViewGroup],
		COALESCE(vgd.[DataVersion], 0) AS [DataVersion],
		COALESCE(vgd.[XmlVersion], 0) AS [XmlVersion],
		COALESCE(vgd.[GroupDataXml], '') AS [Data]
	FROM [ViewGroups] vg
	LEFT JOIN [ViewGroupData] vgd ON vg.[IDViewGroup] = vgd.[IDViewGroup]
		AND vgd.[IDViewGroupType] = @IDViewGroupType
	WHERE vg.[IDViewGroup] = @IDViewGroup
END
GO