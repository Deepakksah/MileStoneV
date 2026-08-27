-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[recording_server_service_configurable_update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [recording_server_service_configurable_update]
GO

CREATE PROCEDURE [recording_server_service_configurable_update]
	@IDRecorder [GuidType],
	@EnableAccess BIT,
	@WebAddress [NameType],
	@WebPort INT,
	@ShutdownOnStorageFailure BIT,
	@MulticastServerAddress [NameType]
 AS
BEGIN
	SET NOCOUNT ON;

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @dataSetBefore XML
		DECLARE @dataSetAfter XML
		DECLARE @IDRecordingServerService [GuidType]

		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		SET @IDRecordingServerService = (SELECT [recording_server_service_id] FROM [RecordingServerServiceRecorder] WHERE @IDRecorder = [recorder_id] AND [type] = 0)
	
		-- Make sure the recording server configurable exists
		IF NOT EXISTS (SELECT [PKID] FROM [RecordingServerServiceConfigurable] WITH(UPDLOCK) WHERE [PKID] = @IDRecordingServerService)
		BEGIN
			;THROW 60047, @procedureName, 1;
		END

		IF(@EnableAccess IS NULL AND @WebAddress IS NULL AND @WebPort IS NULL AND @ShutdownOnStorageFailure IS NULL AND @MulticastServerAddress IS NULL)
		BEGIN
			ROLLBACK TRAN
			RETURN 0
		END

		SET @dataSetBefore = dbo.recordingServerServiceConfigurableAsDataSet(@IDRecordingServerService)

		UPDATE [RecordingServerServiceConfigurable]
		SET	[PublicAccessEnabled] = COALESCE(@EnableAccess, [PublicAccessEnabled]),
			[PublicWebServerHostname] = COALESCE(@WebAddress, [PublicWebServerHostname]),
			[PublicWebServerPort] = COALESCE(@WebPort, [PublicWebServerPort]),
			[ShutdownOnStorageFailure] = COALESCE(@ShutdownOnStorageFailure, [ShutdownOnStorageFailure]),
			[MulticastServerAddress] = COALESCE(@MulticastServerAddress, [MulticastServerAddress]),
			[LastModified] = GETUTCDATE()
		WHERE [PKID] = @IDRecordingServerService

		SET @dataSetAfter = dbo.recordingServerServiceConfigurableAsDataSet(@IDRecordingServerService)

		EXEC [create_task_server_services_transaction]
			@type = 'diffgram_update',
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