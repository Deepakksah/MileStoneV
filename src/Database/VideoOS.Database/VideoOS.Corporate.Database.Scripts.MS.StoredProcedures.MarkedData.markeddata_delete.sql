-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[markeddata_delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [markeddata_delete]
GO

CREATE PROCEDURE [markeddata_delete]
	@IDMarkedData UNIQUEIDENTIFIER,
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

		DECLARE @header [LongNameType]
		DECLARE @marked_data_type INT

		DECLARE @affectedRecorders TABLE ([recorder_id] [GuidType] NOT NULL);

		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		-- Get values for logging and check for exists
		SELECT	@header = [Header],
				@marked_data_type = [MarkDataType]
		FROM	[MarkedData] WITH(UPDLOCK)
		WHERE	[IDMarkedData] = @IDMarkedData


		-- Check that the marked data exist
		IF (@header IS NULL)
		BEGIN
			;THROW 60163, @procedureName, 1;
		END

		-- Create diffgrams for evidence locks
		IF (@marked_data_type = @MarkedDataType_EvidenceLock)
		BEGIN
			DECLARE @device_id [GuidType]
			DECLARE @recorder_id [GuidType]
			DECLARE @dataSetBefore XML

			DECLARE device_evidence_lock_cursor CURSOR FORWARD_ONLY READ_ONLY FOR
				SELECT DISTINCT [IDRecorder], [IDDevice] -- Make the evidence lock track diffgrams for a device in one diffgram
				FROM	[DeviceTrackEvidenceLock]
				WHERE	[IDMarkedData] = @IDMarkedData
	
			OPEN device_evidence_lock_cursor

			BEGIN TRY
				WHILE (1 = 1)
				BEGIN
					FETCH device_evidence_lock_cursor
					INTO @recorder_id, @device_id

					IF(@@FETCH_STATUS <> 0)
					BEGIN
						BREAK
					END

					SET @dataSetBefore = dbo.[deviceTrackEvidenceLockAsDataSet](@recorder_id, null, @device_id, @IDMarkedData)

					-- Create diffgram task for all services for recorder
					EXEC [create_task_recorder_services_transaction]
						@type = 'diffgram_delete',
						@data_a = @dataSetBefore,
						@current_time = @CurrentUtcTime,
						@recorder_id = @recorder_id

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

		-- Start deleting MarkedData
		DELETE FROM [MarkedDataDevices]
		WHERE [IDMarkedData] = @IDMarkedData

		DELETE [MarkedData]
		WHERE [IDMarkedData] = @IDMarkedData

		COMMIT TRAN

		-- Log: Marked data (with evidence locks or bookmarks) '{header}' was deleted.
		EXEC create_pending_log @session_id, 22014, @IDMarkedData, @header

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