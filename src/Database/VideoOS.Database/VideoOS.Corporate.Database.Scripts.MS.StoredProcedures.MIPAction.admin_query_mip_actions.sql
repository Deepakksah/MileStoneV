-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_mip_actions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_mip_actions]
GO

CREATE PROCEDURE [admin_query_mip_actions]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		m.[IDMIPAction],
		m.[Name],
		m.[DefiningPluginId],
		m.[DefiningPluginName],
		m.[SelectionText],
		m.[DescriptionText],
		m.[PickerText],
		m.[MIPKinds],
		m.[PickerText2],
		m.[MIPKinds2],
		m.[LastModified],
		m.[CreatedDate]
	FROM 
		[MIPAction] m
END
GO