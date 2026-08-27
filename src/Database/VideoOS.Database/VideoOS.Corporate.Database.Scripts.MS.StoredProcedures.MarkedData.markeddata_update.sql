-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[markeddata_update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [markeddata_update]
GO

CREATE PROCEDURE [markeddata_update]
	@IDMarkedData UNIQUEIDENTIFIER,
	@IDDevices [tvp_StringTableGuidType] READONLY,
	@Reference [NameType],
	@Header [LongNameType],
	@Description NVARCHAR(MAX),
	@StartTime BIGINT,
	@TagTime BIGINT,
	@EndTime BIGINT,
	@RetentionType INT,
	@RetentionUnits INT,
	@RetentionExpire BIGINT,
	@ProtectionSize BIGINT,
	@StickerSizes [tvp_StickerSizeTable] READONLY,
	@session_id [GuidType]
AS	
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @MarkedDataType_EvidenceLock INT = 2
		DECLARE @marked_data_type INT
		DECLARE @affectedRecorders TABLE ([recorder_id] [GuidType] NOT NULL);

		DECLARE @evidenceLocksBefore TABLE (
			[device_id] [GuidType] NOT NULL,
			[recorder_id] [GuidType] NOT NULL,
			[diffgram] XML NOT NULL
			);
		DECLARE @evidenceLocksAfter TABLE (
			[device_id] [GuidType] NOT NULL,
			[recorder_id] [GuidType] NOT NULL,
			[diffgram] XML NOT NULL
			);

		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		-- Get values for existing marked data and check for exists
		SELECT	@marked_data_type = [MarkDataType]
		FROM	[MarkedData] WITH(UPDLOCK)
		WHERE	[IDMarkedData] = @IDMarkedData


		-- Check that the marked data exist
		IF (@marked_data_type IS NULL)
		BEGIN
			;THROW 60163, @procedureName, 1;
		END

		DECLARE @noSizeValue INT
		IF (@marked_data_type = @MarkedDataType_EvidenceLock)
		BEGIN
			SELECT @noSizeValue = -1
		END
		ELSE
		BEGIN
			SELECT @noSizeValue = 0
		END

		-- Capture before diffgrams for evidence locks
		IF (@marked_data_type = @MarkedDataType_EvidenceLock)
		BEGIN
			INSERT @evidenceLocksBefore ([device_id], [recorder_id], [diffgram])
			SELECT  dtLock.[IDDevice], dtLock.[IDRecorder], dbo.[deviceTrackEvidenceLockAsDataSet](dtLock.[IDRecorder], NULL, dtLock.[IDDevice], @IDMarkedData)
			FROM	(SELECT DISTINCT [IDDevice], [IDRecorder] -- Make the evidence lock track diffgrams for a device in one diffgram
					FROM [DeviceTrackEvidenceLock] 
					WHERE [IDMarkedData] = @IDMarkedData) dtLock
		END

		UPDATE	[MarkedData]
		SET		[LastModified] = @CurrentUtcTime,
				[Reference] = @Reference,
				[Header] = @Header,
				[Description] = @Description,
				[StartTime] = @StartTime,
				[TagTime] = @TagTime,
				[EndTime] = @EndTime,
				[RetentionType] = @RetentionType,
				[RetentionUnits] = @RetentionUnits,
				[RetentionExpire] = @RetentionExpire,
				[ProtectionSize] = @ProtectionSize
		WHERE	[IDMarkedData] = @IDMarkedData
	
		DELETE FROM [MarkedDataDevices]
		WHERE [IDMarkedData] = @IDMarkedData

		INSERT	[MarkedDataDevices] ([IDMarkedData], [IDDevice], [Size])
		SELECT	@IDMarkedData, ids.[ID], ISNULL(ss.Size, @noSizeValue)
		FROM	@IDDevices ids
				LEFT JOIN @StickerSizes AS ss ON ss.[DeviceId] = ids.[ID]

		-- Capture after diffgrams for evidence locks
		IF (@marked_data_type = @MarkedDataType_EvidenceLock)
		BEGIN
			DECLARE @recorder_id AS [GuidType]
			DECLARE @diffgram_before AS XML
			DECLARE @diffgram_after AS XML

			INSERT @evidenceLocksAfter ([device_id], [recorder_id], [diffgram])
			SELECT  dtLock.[IDDevice], dtLock.[IDRecorder], dbo.[deviceTrackEvidenceLockAsDataSet](dtLock.[IDRecorder], NULL, dtLock.[IDDevice], @IDMarkedData)
			FROM	(SELECT DISTINCT [IDDevice], [IDRecorder] -- Make the evidence lock track diffgrams for a device in one diffgram
					FROM [DeviceTrackEvidenceLock] 
					WHERE [IDMarkedData] = @IDMarkedData) dtLock
			
			-- Create diffgram(s)
			DECLARE device_evidence_lock_cursor CURSOR FORWARD_ONLY READ_ONLY FOR
			SELECT	COALESCE([before].[recorder_id], [after].[recorder_id]) AS [recorder_id],
					[before].[diffgram] AS [diffgram_before],
					[after].[diffgram] AS [diffgram_after]
			FROM	@evidenceLocksBefore [before]
					FULL OUTER JOIN @evidenceLocksAfter [after] ON [before].[device_id] = [after].[device_id] AND [before].[recorder_id] = [after].[recorder_id]
	
			OPEN device_evidence_lock_cursor
			BEGIN TRY
				WHILE (1 = 1)
				BEGIN
					FETCH device_evidence_lock_cursor
					INTO @recorder_id, @diffgram_before, @diffgram_after

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
							@current_time = @CurrentUtcTime,
							@recorder_id = @recorder_id
					END
					ELSE IF (@diffgram_before IS NOT NULL)
					BEGIN
						EXEC [create_task_recorder_services_transaction]
							@type = 'diffgram_delete',
							@data_a = @diffgram_before,
							@current_time = @CurrentUtcTime,
							@recorder_id = @recorder_id
					END
					ELSE IF (@diffgram_after IS NOT NULL)
					BEGIN
						EXEC [create_task_recorder_services_transaction]
							@type = 'diffgram_insert',
							@data_a = @diffgram_after,
							@current_time = @CurrentUtcTime,
							@recorder_id = @recorder_id
					END

					INSERT INTO @affectedRecorders ([recorder_id]) VALUES (@recorder_id)
				END
			END TRY
			BEGIN CATCH
				CLOSE device_evidence_lock_cursor
				DEALLOCATE device_evidence_lock_cursor
				;THROW
			END CATCH

			CLOSE device_evidence_lock_cursor
			DEALLOCATE device_evidence_lock_cursor
		END

		COMMIT TRAN

		-- Log: The marked data (with evidence locks or bookmarks) '{Header}' was updated.
		EXEC create_pending_log @session_id, 22013, @IDMarkedData, @Header

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
	END CATCH;
END
GO