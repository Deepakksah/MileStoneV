-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- QueryKindsByPluginId - gets the MIP kinds belonging to a specific plugin
IF OBJECT_ID ( '[Mip].[QueryKindsByPluginId]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Mip].[QueryKindsByPluginId];
GO

CREATE PROCEDURE [Mip].[QueryKindsByPluginId]
	@PluginId UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON

	SELECT *
	FROM [Mip].[Kinds] 
	WHERE PluginId = @PluginId
END
GO