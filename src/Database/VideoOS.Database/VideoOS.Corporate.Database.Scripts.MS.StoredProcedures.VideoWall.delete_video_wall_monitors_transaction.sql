-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_video_wall_monitors_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_video_wall_monitors_transaction]
GO

CREATE PROCEDURE [delete_video_wall_monitors_transaction]
	@videowall_id [GuidType],
	@videowallName [NameType],
	@session_id [GuidType]
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

	-- Delete monitor presets
	EXEC [delete_video_wall_monitor_presets_transaction]
		@videowall_id = @videowall_id

	-- Delete custom properties
	INSERT [TempCustomPropertyDelete]
	SELECT @@SPID, [pkid]
	FROM [VideoWallMonitor]
	WHERE [videowall_id] = @videowall_id
	
	EXEC [delete_custom_properties_transaction]
	
	-- Extract info for logging
	DECLARE @monitorName [NameType]
	DECLARE @monitor_id [GuidType]
	
	DECLARE monitor_cursor CURSOR FOR 
	SELECT [pkid], [name] 
	FROM [VideoWallMonitor]
	WHERE [videowall_id] = @videowall_id
	
	OPEN monitor_cursor
	BEGIN TRY
		FETCH NEXT FROM monitor_cursor INTO @monitor_id, @monitorName

		WHILE @@FETCH_STATUS = 0
		BEGIN
			EXEC create_pending_log @session_id, 10102, @monitor_id, @monitorName, @videowall_id, @videoWallName
			FETCH NEXT FROM monitor_cursor INTO @monitor_id, @monitorName
		END
	END TRY
	BEGIN CATCH
		CLOSE monitor_cursor
		DEALLOCATE monitor_cursor
		;THROW
	END CATCH

	CLOSE monitor_cursor
	DEALLOCATE monitor_cursor
	
	-- Delete the monitor
	DELETE FROM [VideoWallMonitor]
	WHERE [videowall_id] = @videowall_id

	-- Delete views
	EXEC [delete_video_wall_view_transaction]
		@videowall_id = @videowall_id
END
GO