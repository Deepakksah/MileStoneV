-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[restricted_media_update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [restricted_media_update]
GO

CREATE PROCEDURE [restricted_media_update]
	@IDRestrictedMedia	UNIQUEIDENTIFIER,
	@IDDevices          [tvp_StringTableGuidType] READONLY,
	@Header             [LongNameType],
	@Description        [DescriptionType2],
	@StartTime          DATETIME2,
	@EndTime            DATETIME2,
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
		DECLARE @restricted_media_header [LongNameType]
		DECLARE @affectedRecorders TABLE ([recorder_id] [GuidType] NOT NULL);
		DECLARE @deviceRestrictedMediaBefore TABLE (
			[device_id] [GuidType] NOT NULL,
			[recorder_id] [GuidType] NOT NULL,
			[diffgram] XML NOT NULL
			);
		DECLARE @deviceRestrictedMediaAfter TABLE (
			[device_id] [GuidType] NOT NULL,
			[recorder_id] [GuidType] NOT NULL,
			[diffgram] XML NOT NULL
			);

		BEGIN TRAN

		-- Get values for existing restricted media and check for exists
		SELECT	@restricted_media_header = [Header]
		FROM	[RestrictedMedia] WITH(UPDLOCK)
		WHERE	[IDRestrictedMedia] = @IDRestrictedMedia

		-- Check that the marked data exist
		IF (@restricted_media_header IS NULL)
		BEGIN
			;THROW 60375, @procedureName, 1; -- Message like: Cannot update the media restriction. The media restriction does not exist.
		END

		-- Capture before diffgrams
		INSERT @deviceRestrictedMediaBefore ([device_id], [recorder_id], [diffgram])
			SELECT	[IDDevice], [IDRecorder], dbo.[deviceRestrictedMediaAsDataSet](@IDRestrictedMedia, [IDDevice])
			FROM	[DeviceRestrictedMedia]
			WHERE   [IDRestrictedMedia] = @IDRestrictedMedia
	
		UPDATE	[RestrictedMedia]
		SET		[Header] = @Header,
				[Description] = @Description,
				[StartTime] = @StartTime,
				[EndTime] = @EndTime,
				[LastModified] = @LastModified
		WHERE	[IDRestrictedMedia] = @IDRestrictedMedia
	
		DELETE FROM [RestrictedMediaDevice]
		WHERE [IDRestrictedMedia] = @IDRestrictedMedia

		INSERT	[RestrictedMediaDevice] ([IDRestrictedMedia], [IDDevice])
			SELECT	@IDRestrictedMedia, ids.[Id]
			FROM	@IDDevices ids

		-- Capture after diffgrams
		DECLARE @device_id AS [GuidType]
		DECLARE @recorder_id AS [GuidType]
		DECLARE @diffgram_before AS XML
		DECLARE @diffgram_after AS XML

		INSERT @deviceRestrictedMediaAfter ([device_id], [recorder_id], [diffgram])
			SELECT	[IDDevice], [IDRecorder], dbo.[deviceRestrictedMediaAsDataSet](@IDRestrictedMedia, [IDdevice])
			FROM	[DeviceRestrictedMedia]
			WHERE   [IDRestrictedMedia] = @IDRestrictedMedia

		-- Create diffgram(s)
		DECLARE device_restricted_media_cursor CURSOR FORWARD_ONLY READ_ONLY FOR
			SELECT	COALESCE([before].[device_id], [after].[device_id]) AS [device_id],
					COALESCE([before].[recorder_id], [after].[recorder_id]) AS [recorder_id],
					[before].[diffgram] AS [diffgram_before],
					[after].[diffgram] AS [diffgram_after]
			FROM	@deviceRestrictedMediaBefore [before]
			FULL OUTER JOIN @deviceRestrictedMediaAfter [after] ON [before].[device_id] = [after].[device_id] AND [before].[recorder_id] = [after].[recorder_id]
	
		OPEN device_restricted_media_cursor
		BEGIN TRY
			WHILE (1 = 1)
			BEGIN
				FETCH device_restricted_media_cursor
				INTO @device_id, @recorder_id, @diffgram_before, @diffgram_after

				IF (@@FETCH_STATUS <> 0)
				BEGIN
					BREAK
				END

				-- Create diffgram task for all services for recorder
				IF (@diffgram_before IS NOT NULL AND @diffgram_after IS NOT NULL)
				BEGIN
					EXEC [create_task_recorder_services_transaction]
						@type = 'diffgram_update',
						@data_a = @diffgram_before,
						@data_b = @diffgram_after,
						@current_time = @LastModified,
						@recorder_id = @recorder_id
				END
				ELSE IF (@diffgram_before IS NOT NULL)
				BEGIN
					EXEC [create_task_recorder_services_transaction]
						@type = 'diffgram_delete',
						@data_a = @diffgram_before,
						@current_time = @LastModified,
						@recorder_id = @recorder_id
				END
				ELSE IF (@diffgram_after IS NOT NULL)
				BEGIN
					EXEC [create_task_recorder_services_transaction]
						@type = 'diffgram_insert',
						@data_a = @diffgram_after,
						@current_time = @LastModified,
						@recorder_id = @recorder_id
				END
		
				INSERT INTO @affectedRecorders ([recorder_id]) VALUES (@recorder_id)
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

		DECLARE @AuditMediaRestrictionUpdated INT = 7121 -- Log: The restricted media '{Header}' was updated.
		EXEC create_pending_log @session_id, @AuditMediaRestrictionUpdated, @IDRestrictedMedia, @header

		SELECT DISTINCT([recorder_id]) FROM @affectedRecorders
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