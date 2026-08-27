-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- QueryPlugins - gets all the MIP plugins
IF OBJECT_ID ( '[Mip].[QueryPlugins]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Mip].[QueryPlugins];
GO

CREATE PROCEDURE [Mip].[QueryPlugins]
AS
BEGIN
	SET NOCOUNT ON

	SELECT DISTINCT PluginId, PluginName
	FROM [Mip].[Kinds] 
END
GO