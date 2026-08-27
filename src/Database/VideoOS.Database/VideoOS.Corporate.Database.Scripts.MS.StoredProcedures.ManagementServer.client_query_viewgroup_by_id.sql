-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[client_query_viewgroup_by_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [client_query_viewgroup_by_id]
GO

CREATE PROCEDURE [client_query_viewgroup_by_id]
	@IDViewGroup [GuidType],
	@IDViewGroupType [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT 
		vg.[IDViewGroup],
		vg.[Name], 
		vg.[Type], 
		vg.[OwnerSID], 
		COALESCE(vgd.[ModifiedDate], GETUTCDATE()) AS [LastModified],
		COALESCE(vgd.[ModifiedUser], '') AS [LastModifiedUser]
	FROM [ViewGroups] vg
	LEFT JOIN [ViewGroupData] vgd ON vg.[IDViewGroup] = vgd.[IDViewGroup]
		AND vgd.[IDViewGroupType] = @IDViewGroupType
	WHERE vg.[IDViewGroup] = @IDViewGroup
END
GO