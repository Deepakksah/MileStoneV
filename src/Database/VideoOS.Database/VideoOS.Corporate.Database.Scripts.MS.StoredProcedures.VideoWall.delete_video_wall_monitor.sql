-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_video_wall_monitor]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_video_wall_monitor]
GO

CREATE PROCEDURE [delete_video_wall_monitor]
	@pkid [GuidType],
	@session_id [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @monitor_id [GuidType]
		DECLARE @view_id [GuidType]

		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()
	
		-- Extract info for logging
		DECLARE @videoWallName [NameType]
		DECLARE @videowall_Id[GuidType]

		SELECT @videowall_Id = vw.[pkid], @videoWallName = vw.[name]
		FROM [VideoWall] vw WITH(UPDLOCK)
		INNER JOIN [VideoWallMonitor] vwm ON vwm.[videowall_id] = vw.[pkid]
		WHERE vwm.[pkid] = @pkid
	
		DECLARE @monitorName [NameType]
		SELECT @monitor_id = [pkid], @view_id = [view_id], @monitorName = [name]
		FROM [VideoWallMonitor] WITH(UPDLOCK)
		WHERE [pkid] = @pkid

		-- Does the monitor exist?
		IF(@monitor_id IS NULL)
		BEGIN
			;THROW 60213, @procedureName, 1;
		END
	
		-- Delete custom properties
		EXEC [delete_custom_properties_transaction]
			@object_id = @pkid

		-- Delete monitor presets
		EXEC [delete_video_wall_monitor_presets_transaction]
			@monitor_id = @pkid
	
		-- Delete the monitor
		DELETE FROM [VideoWallMonitor]
		WHERE [pkid] = @pkid
	
		-- Delete view
		EXEC [delete_video_wall_view_transaction]
			@view_id = @view_id

		COMMIT TRAN
	
		EXEC create_pending_log @session_id, 10102, @pkid, @monitorName, @videowall_id, @videoWallName
	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		DECLARE @shouldReThrowError BIT = dbo.[ShouldReThrowError](ERROR_NUMBER(), ERROR_SEVERITY())
		IF(@shouldReThrowError = 1)
			THROW;
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE())
		;THROW 60000, @ErrorArgs, 1;
	END CATCH;
END
GO