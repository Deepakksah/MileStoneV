-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_recording_storage_promote_new_password]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_recording_storage_promote_new_password]
GO

CREATE PROCEDURE [update_recording_storage_promote_new_password]
	@IDRecordingStorage [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

		-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
	DECLARE @rowCount INT
	DECLARE @newPassword NVARCHAR(MAX)
	DECLARE @IDRecordingServer [GuidType]
	DECLARE @dataSetBefore XML
	DECLARE @dataSetAfter XML
	DECLARE @recorder_service_id [GuidType]

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()

	BEGIN TRAN

		-- Get the new password that should be promoted
		SELECT @IDRecordingServer = [recordingserver_id], @newPassword = [encryption_password_new] 
		FROM [RecordingStorage] WITH(UPDLOCK)
		WHERE [pkid] = @IDRecordingStorage

		SELECT @rowCount = @@ROWCOUNT
		IF(@rowCount = 0)
		BEGIN
			;THROW 60027, @procedureName, 1;
		END

		-- Get service id for task
		SELECT @recorder_service_id = rssr.[recording_server_service_id]
		FROM [RecordingServerServiceRecorder] rssr
		WHERE rssr.[recorder_id] = @IDRecordingServer AND rssr.[type] = 0

		IF(@recorder_service_id IS NULL)
		BEGIN
			;THROW 60000, @procedureName, 1;
		END

		-- Get DataSet before changes
		set @dataSetBefore = dbo.recordingStorageAsDataSet(@IDRecordingStorage)

		UPDATE [RecordingStorage]
		SET 
			[encryption_password] = @newPassword,
			[encryption_password_new] = '',
			[last_modified] = @CurrentUtcTime
		WHERE [pkid] = @IDRecordingStorage
	
		-- Get DataSet after changes
		SET @dataSetAfter = dbo.recordingStorageAsDataSet(@IDRecordingStorage)

		-- Call the create task procedure to create diffgram update task
		EXEC [create_task_transaction]
								'diffgram_update',
								@recorder_service_id,
								@dataSetBefore,
								@dataSetAfter,
								@CurrentUtcTime

		COMMIT TRAN

		RETURN 0

	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		THROW; -- Used from ServerService, where Error 60000 is NOT expected
    END CATCH;
END
GO
