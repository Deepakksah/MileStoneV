-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_hardware_events]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_hardware_events]
GO

CREATE PROCEDURE [dbo].[update_hardware_events]
	@hardware_id [GuidType],
	@last_modified DATETIME,
	@events_xml XML,
	@session_id [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @rowCount INT
		DECLARE @dataSetBefore XML
		DECLARE @dataSetAfter XML
	
		DECLARE @hardware_name [NameType]
		DECLARE @recordingserver_id [GuidType]
		DECLARE @recordingserver_name [NameType]

		DECLARE @last_modified_current DATETIME

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		BEGIN TRAN

		-- Make sure that device exists, and get properties for logging
		SELECT	@recordingserver_id = h.[IDRecorder],
				@recordingserver_name = r.[Name],
				@hardware_name = h.[Name],
				@last_modified_current = h.[LastModified]
		FROM [Hardware] h WITH(UPDLOCK)
		INNER JOIN [Recorders] r ON r.IDRecorder = h.[IDRecorder] 
		WHERE h.[IDHardware] = @hardware_id
	
		SELECT @rowCount = @@ROWCOUNT
		IF(@rowCount = 0)
		BEGIN
			;THROW 60082, @procedureName, 1;
		END

		-- Check that last modified doesn't prevent user from updating object
		IF (@last_modified_current <> @last_modified)
		BEGIN
			;THROW 60338, @procedureName, 1;
		END

		-- Get DataSet before changes (for DiffGram)
		SET @dataSetBefore = dbo.hardwareAsDataSet(@hardware_id)

		-- Update table 'Devices' with new settings (and last modified)
		UPDATE [Hardware]
		SET [LastModified] = @CurrentUtcTime,
			[EventSettings] = @events_xml
		WHERE [IDHardware] = @hardware_id

		-- HardwareSettingsChanged: 3615; Log an entry similar to "Settings changed on '{hardware name}'."
		EXEC create_pending_log @session_id, 3615, @hardware_id, @hardware_name, @recordingserver_id, @recordingserver_name

		-- Get DataSet after changes
		SET @dataSetAfter = dbo.hardwareAsDataSet(@hardware_id)

		-- Register diffgrams for management server
		EXEC [create_task_transaction]
			@type = 'diffgram_update',
			@target = '909749AE-88C4-4780-A381-383D691E98FA',
			@data_a = @dataSetBefore,
			@data_b = @dataSetAfter,
			@current_time = @CurrentUtcTime
		COMMIT TRAN
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