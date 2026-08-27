-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_video_wall_preset]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_video_wall_preset]
GO

CREATE PROCEDURE [delete_video_wall_preset]
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
		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		-- Extract info for logging
		DECLARE @videoWallName [NameType]
		DECLARE @videowall_Id[GuidType]
		DECLARE @preset_id [GuidType]
		DECLARE @presetName [NameType]

		SELECT @videowall_Id = vw.[pkid], @videoWallName = vw.[name]
		FROM [VideoWall] vw WITH(UPDLOCK)
		INNER JOIN [VideoWallpreset] vwm ON vwm.[videowall_id] = vw.[pkid]
		WHERE vwm.[pkid] = @pkid	
	
		SELECT @preset_id = [pkid], @presetName = [name]
		FROM [VideoWallpreset] WITH(UPDLOCK)
		WHERE [pkid] = @pkid
	
		-- Does the video wall preset exist?
		IF(@preset_id IS NULL)
		BEGIN
			;THROW 60207, @procedureName, 1;
		END
	
		-- Delete custom properties
		EXEC [delete_custom_properties_transaction]
			@object_id = @pkid

		-- Delete monitor presets
		EXEC [delete_video_wall_monitor_presets_transaction]
			@videowallpreset_id = @pkid
	
		-- Delete the video wall preset
		DELETE FROM [VideoWallPreset]
		WHERE [pkid] = @pkid

		COMMIT TRAN
	
		EXEC create_pending_log @session_id, 10203, @pkid, @presetName, @videowall_id, @videoWallName
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