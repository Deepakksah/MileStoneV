-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[restricted_media_live_update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [restricted_media_live_update]
GO

CREATE PROCEDURE [restricted_media_live_update]
	@IDDevices		[tvp_StringTableGuidType] READONLY,
	@StartTime		DATETIME2,
	@LastModified	DATETIME2,
	@session_id		[GuidType]
AS	
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @restricted_media_header [LongNameType]
		DECLARE @AuditRestrictedMediaLiveUpdated INT = 7125

		DECLARE @deviceRestrictedMediaBefore TABLE (
							[device_id]		[GuidType] NOT NULL,
							[device_name]	NVARCHAR(256) NOT NULL,
							[device_type]	VARCHAR(256) NOT NULL,
							[recorder_id]	[GuidType] NOT NULL,
							[diffgram]		XML NOT NULL
							);
		DECLARE @deviceRestrictedMediaAfter TABLE (
							[device_id]		[GuidType] NOT NULL,
							[recorder_id]	[GuidType] NOT NULL,
							[diffgram]		XML NOT NULL
							);

		BEGIN TRAN

		-- Check the modified restricted media live device ids exits in the restricted media live table
		Declare @deviceIdCount INT
		SET @deviceIdCount = (SELECT COUNT(*) FROM [RestrictedMediaLive] WITH(UPDLOCK) WHERE [IDDevice] IN (SELECT ids.[ID] FROM @IDDevices ids))
		IF (@deviceIdCount <> (SELECT COUNT(*) FROM @IDDevices))
		BEGIN
			;THROW 60380, @procedureName, 1; -- Message like: Cannot update the live media restriction. The live media restriction does not exist
		END

		-- Capture before diffgrams
		INSERT @deviceRestrictedMediaBefore ([device_id], [device_name], [device_type], [recorder_id], [diffgram])
			SELECT		drm.[IDDevice], d.[Name], d.[DeviceType], drm.[IDRecorder], dbo.[deviceRestrictedMediaAsDataSet](drm.[IDDevice], drm.[IDDevice])
			FROM		[DeviceRestrictedMedia] drm
			INNER JOIN	[Devices] d ON drm.[IDRestrictedMedia] = d.[IDDevice]
			WHERE		drm.[IDRestrictedMedia] IN (SELECT ids.[ID] FROM @IDDevices ids)

		UPDATE	[RestrictedMediaLive]
		SET		[StartTime] = @StartTime,
				[LastModified] = @LastModified
		WHERE	[IDDevice] IN (SELECT ids.[ID] FROM @IDDevices ids)
	
		-- Capture after diffgrams
		DECLARE @device_id			[GuidType]
		DECLARE @device_name		NVARCHAR(256)
		DECLARE @device_type		VARCHAR(100)
		DECLARE @recorder_id		[GuidType]
		DECLARE @diffgram_before	XML
		DECLARE @diffgram_after		XML

		DECLARE @AuditMediaRestrictionUpdated INT = 7121

		INSERT @deviceRestrictedMediaAfter ([device_id], [recorder_id], [diffgram])
			SELECT	[IDDevice], [IDRecorder], dbo.[deviceRestrictedMediaAsDataSet]([IDDevice], [IDDevice])
			FROM	[DeviceRestrictedMedia]
			WHERE	[IDRestrictedMedia] IN (SELECT ids.[ID] FROM @IDDevices ids)

		-- Create diffgram(s)
		DECLARE device_restricted_media_cursor CURSOR FORWARD_ONLY READ_ONLY FOR
			SELECT	[before].[device_id],
					d.[Name],
					d.[DeviceType],
					[before].[recorder_id],
					[before].[diffgram] AS [diffgram_before],
					[after].[diffgram] AS [diffgram_after]
			FROM	@deviceRestrictedMediaBefore [before]
					INNER JOIN @deviceRestrictedMediaAfter [after] ON [before].[device_id] = [after].[device_id]
					INNER JOIN [Devices] d ON d.[IDDevice] = [before].[device_id]
	
		OPEN device_restricted_media_cursor
		BEGIN TRY
			WHILE (1 = 1)
			BEGIN
				FETCH device_restricted_media_cursor
				INTO @device_id, @device_name, @device_type, @recorder_id, @diffgram_before, @diffgram_after

				IF (@@FETCH_STATUS <> 0)
				BEGIN
					BREAK
				END

				-- Create diffgram task for all services for recorder
				DECLARE @CurrentUtcTime DATETIME = GETUTCDATE()
		
				EXEC [create_task_recorder_services_transaction]
					@type = 'diffgram_update',
					@data_a = @diffgram_before,
					@data_b = @diffgram_after,
					@current_time = @CurrentUtcTime,
					@recorder_id = @recorder_id
		
				EXEC create_pending_log @session_id, @AuditRestrictedMediaLiveUpdated, @device_id, @device_name, @device_type

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

		SELECT DISTINCT([recorder_id]) FROM @deviceRestrictedMediaBefore
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