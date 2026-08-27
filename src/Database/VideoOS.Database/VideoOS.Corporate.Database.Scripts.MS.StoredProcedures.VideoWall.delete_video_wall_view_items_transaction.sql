-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_video_wall_view_items_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_video_wall_view_items_transaction]
GO

CREATE PROCEDURE [delete_video_wall_view_items_transaction]
	@view_id [GuidType] = NULL,
	@videowall_id [GuidType] = NULL
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END
	
	IF(@view_id IS NOT NULL)
	BEGIN
		-- Delete custom properties
		INSERT [TempCustomPropertyDelete]
		SELECT @@SPID, [pkid]
		FROM [VideoWallViewItem]
		WHERE [view_id] = @view_id	
			
		EXEC [delete_custom_properties_transaction]

		DELETE [VideoWallViewItem]
		WHERE [view_id] = @view_id
	END
	ELSE IF(@videowall_id IS NOT NULL)
	BEGIN
		-- Delete custom properties
		INSERT [TempCustomPropertyDelete]
			SELECT @@SPID, [pkid]
			FROM [VideoWallViewItem]
			WHERE [view_id] IN (SELECT [view_id] 
								FROM VideoWallMonitor 
								WHERE [videowall_id] = @videowall_id)
	
		EXEC [delete_custom_properties_transaction]
			
		DELETE [VideoWallViewItem]
		WHERE [view_id] IN (
			SELECT [view_id] 
			FROM VideoWallMonitor 
			WHERE [videowall_id] = @videowall_id
		)
	END
END
GO