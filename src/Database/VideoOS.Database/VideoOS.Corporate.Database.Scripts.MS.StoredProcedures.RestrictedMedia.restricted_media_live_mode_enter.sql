-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[restricted_media_live_mode_enter]') AND [type] IN (N'P', N'PC'))
DROP PROCEDURE [restricted_media_live_mode_enter]
GO

CREATE PROCEDURE [dbo].[restricted_media_live_mode_enter]
	@IDDevices          [tvp_StringTableGuidType] READONLY,
	@UserName           [NameType],
	@StartTime          DATETIME2,
	@LastModified       DATETIME2,
	@session_id         [GuidType]
AS	
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		BEGIN TRY
			INSERT	[RestrictedMediaLive] ([LastModified], [IDDevice], [UserName],[StartTime])
			SELECT @LastModified, ids.[Id], @UserName, @StartTime
			FROM	@IDDevices ids
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2627)
			BEGIN
				;THROW 60378, @procedureName, 1;--error message like 'Cannot enter restricted live mode. One or more of the devices are already in restricted live mode.'.
			END
			IF(ERROR_NUMBER() = 547)
			BEGIN
				;THROW 60379, @procedureName, 1;--error message like 'Cannot enter restricted live mode. One or more of the devices does not exist'.
			END
			;THROW
		END CATCH

		-- Create diffgrams
		DECLARE @device_id		[GuidType]
		DECLARE @device_name	NVARCHAR(256)
		DECLARE @device_type	VARCHAR(100)
		DECLARE @recorder_id	[GuidType]
		DECLARE @dataSetAfter	XML

		DECLARE @deviceRestrictedMediaLiveAfter TABLE (
			[device_id] [GuidType] NOT NULL,
			[device_name] NVARCHAR(256) NOT NULL,
			[device_type] VARCHAR(100) NOT NULL,
			[recorder_id] [GuidType] NOT NULL,
			[diffgram] XML NOT NULL
			);

		DECLARE @AuditRestrictedMediaLiveModeEntered INT = 7123

		-- Capture after diffgrams
		INSERT @deviceRestrictedMediaLiveAfter ([device_id], [device_name], [device_type], [recorder_id], [diffgram])
			SELECT		drm.[IDDevice], d.[Name], d.[DeviceType], drm.[IDRecorder], dbo.[deviceRestrictedMediaAsDataSet](drm.[IDDevice], drm.[IDDevice])
			FROM		[DeviceRestrictedMedia] drm
			INNER JOIN	[Devices] d ON d.[IDDevice] = drm.[IDDevice]
			WHERE		drm.[IDRestrictedMedia] IN (SELECT ids.[ID] from @IDDevices ids)

		DECLARE device_restricted_media_cursor CURSOR FORWARD_ONLY READ_ONLY FOR
			SELECT	[recorder_id], [device_id], [device_name], [device_type], [diffgram] AS [diffgram_after]
			FROM	@deviceRestrictedMediaLiveAfter
	
		OPEN device_restricted_media_cursor
		BEGIN TRY
			WHILE (1 = 1)
			BEGIN
				FETCH device_restricted_media_cursor
				INTO @recorder_id, @device_id, @device_name, @device_type, @dataSetAfter

				IF (@@FETCH_STATUS <> 0)
				BEGIN
					BREAK
				END

				-- Create diffgram task for all services for recorder
				EXEC [create_task_recorder_services_transaction]
					@type = 'diffgram_insert',
					@data_a = @dataSetAfter,
					@current_time = @LastModified,
					@recorder_id = @recorder_id
		
				EXEC create_pending_log @session_id, @AuditRestrictedMediaLiveModeEntered, @device_id, @device_name, @device_type
			END
		END TRY
		BEGIN CATCH
			CLOSE device_restricted_media_cursor
			DEALLOCATE device_restricted_media_cursor
			;THROW
		END CATCH

		CLOSE device_restricted_media_cursor
		DEALLOCATE device_restricted_media_cursor

		COMMIT TRAN

		SELECT DISTINCT([recorder_id]) FROM @deviceRestrictedMediaLiveAfter
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
	END CATCH
END
GO