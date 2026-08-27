-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_video_wall]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_video_wall]
GO

CREATE PROCEDURE [delete_video_wall]
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
		DECLARE @videowall_id [GuidType]

		-- Extract info for logging
		DECLARE @videoWallName [NameType]

		SELECT @videowall_id = [pkid], @videoWallName = [name]
		FROM [VideoWall] WITH(UPDLOCK)
		WHERE [pkid] = @pkid

		-- Does the video wall exist?
		IF(@videowall_id IS NULL)
		BEGIN
			ROLLBACK TRAN
			;THROW 60203, @procedureName, 1;
		END
	
		-- Delete all video wall presets
		EXEC [delete_video_wall_presets_transaction]
			@videowall_id = @pkid,
			@videowallName = @videowallName,
			@session_id = @session_id

		-- Delete all video wall monitors
		EXEC [delete_video_wall_monitors_transaction]
			@videowall_id = @pkid,
			@videowallName = @videowallName,
			@session_id = @session_id

		-- Delete custom properties
		EXEC [delete_custom_properties_transaction]
			@object_id = @pkid

		-- Delete the video wall
		DELETE FROM [VideoWall]
		WHERE [pkid] = @pkid
	
		-- Unregister the security object
		EXEC [security_unregister_object_transaction]
			@namespace_name = 'SmartWall',
			@object_id = @pkid,
			@CurrentUtcTime = @CurrentUtcTime

		COMMIT TRAN

		EXEC create_pending_log @session_id, 10002, @pkid, @videoWallName
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