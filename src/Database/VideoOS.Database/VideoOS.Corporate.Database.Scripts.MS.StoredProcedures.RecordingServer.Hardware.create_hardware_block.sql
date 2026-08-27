-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_hardware_block]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_hardware_block]
GO

CREATE PROCEDURE [create_hardware_block]
	@hardware_id [GuidType],
	@expires DATETIME,
	@owner NVARCHAR(256),
	@data NVARCHAR(4000)
AS
BEGIN
	SET NOCOUNT ON

		-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @pkid INT
		DECLARE @rowCount INT
		DECLARE @dataSetAfter XML
		DECLARE @recorder_service_id [GuidType]

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		BEGIN TRAN
	
		-- Make sure that hardware/recording server exists
		DECLARE @hardware_exists BIT
		SELECT @hardware_exists = 1
		FROM [Hardware] h WITH(UPDLOCK)
		WHERE h.[IDHardware] = @hardware_id
	
		SELECT @rowCount = @@ROWCOUNT
		IF(@rowCount = 0)
		BEGIN
			;THROW 60245, @procedureName, 1;
		END

		INSERT [HardwareBlock] ([hardware_id], [expires], [owner], [data]) VALUES (@hardware_id, @expires, @owner, @data)

		SET @pkid = SCOPE_IDENTITY()
	
		-- Get DataSet after changes
		SET @dataSetAfter = dbo.hardwareBlockAsDataSet(@pkid)

		-- Get recording server service ids for task creation
		DECLARE recorder_cursor CURSOR FOR
			SELECT rssr.[recording_server_service_id]
			FROM [RecordingServerServiceRecorder] rssr INNER JOIN [Hardware] hw ON hw.[IDRecorder] = rssr.recorder_id
			WHERE hw.[IDHardware] = @hardware_id AND rssr.[type] IN (0, 2) -- RECORDER or HOT_STANDBY

		OPEN recorder_cursor
		BEGIN TRY
			FETCH NEXT FROM recorder_cursor
			INTO @recorder_service_id

			WHILE(@@FETCH_STATUS = 0)
			BEGIN
				-- Call the create task procedure to create diffgram update task
				EXEC [create_task_transaction]
					@type = 'diffgram_insert',
					@target = @recorder_service_id,
					@data_a = @dataSetAfter,
					@current_time = @CurrentUtcTime

				FETCH NEXT FROM recorder_cursor
				INTO @recorder_service_id
			END
		END TRY
		BEGIN CATCH
			CLOSE recorder_cursor
			DEALLOCATE recorder_cursor
			;THROW
		END CATCH

		CLOSE recorder_cursor
		DEALLOCATE recorder_cursor

		COMMIT TRAN

		SELECT @pkid
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