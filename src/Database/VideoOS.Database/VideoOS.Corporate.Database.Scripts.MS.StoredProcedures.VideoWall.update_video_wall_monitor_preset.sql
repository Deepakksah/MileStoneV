-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_video_wall_monitor_preset]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_video_wall_monitor_preset]
GO

CREATE PROCEDURE [update_video_wall_monitor_preset]
	@pkid [GuidType],
	@definition_xml XML,
	@session_id [GuidType],
	@custom_properties XML = NULL
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()
	
		-- Extract info for logging
		DECLARE @videoWallMonitorName [NameType]
		DECLARE @videowallMonitor_id [GuidType]
		DECLARE @preset_id [GuidType]
		DECLARE @presetName [NameType]

		SELECT @videowallMonitor_id = vwm.[pkid], @videoWallMonitorName = vwm.[name]
		FROM [VideoWallMonitor] vwm WITH(UPDLOCK)
		INNER JOIN [VideoWallMonitorpreset] vwmp ON vwmp.[monitor_id] = vwm.[pkid]
		WHERE vwmp.[pkid] = @pkid
	
		SELECT @preset_id = vwp.[pkid], @presetName = vwp.[name]
		FROM [VideoWallPreset] vwp WITH(UPDLOCK)
		INNER JOIN [VideoWallMonitorpreset] vwmp ON vwmp.[videowallpreset_id] = vwp.[pkid]
		WHERE vwmp.[pkid] = @pkid

		-- Make sure the video wall preset exists
		IF NOT EXISTS (
			SELECT [pkid]
			FROM [VideoWallMonitorPreset] WITH(UPDLOCK)
			WHERE [pkid] = @pkid)
		BEGIN
			;THROW 60220, @procedureName, 1;
		END

		UPDATE [VideoWallMonitorPreset]
		SET [definition_xml] = @definition_xml,
			[last_modified] = @CurrentUtcTime
		WHERE [pkid] = @pkid

		-- Update custom properties
		EXEC [update_custom_properties_transaction]
			@object_id = @pkid,
			@custom_properties = @custom_properties

		COMMIT TRAN
	
		EXEC create_pending_log @session_id, 10202, @preset_id, @presetName, @videowallMonitor_id, @videoWallMonitorName
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