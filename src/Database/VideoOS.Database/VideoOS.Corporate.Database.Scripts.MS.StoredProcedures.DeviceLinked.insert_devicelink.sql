-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[insert_devicelink]') AND type in (N'P', N'PC'))
DROP PROCEDURE [insert_devicelink]
GO

CREATE PROCEDURE [insert_devicelink]
	@IDDevice [GuidType],
	@IDDeviceLinked [GuidType],
	@Priority INT
AS
BEGIN
	SET NOCOUNT ON
	
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @dataSetAfter XML

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		BEGIN TRAN

		INSERT [DeviceLinks] ([LastModified], [IDDevice], [IDDeviceLinked], [Priority])
		VALUES (@CurrentUtcTime, @IDDevice, @IDDeviceLinked, @Priority)

		-- Get DataSet after changes
		SET @dataSetAfter = dbo.deviceLinksAsDataSet(@IDDevice, @IDDeviceLinked)

		-- Register diffgrams for management server
		EXEC [create_task_server_services_transaction]
			@type = 'diffgram_insert',
			@data_a = @dataSetAfter,
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