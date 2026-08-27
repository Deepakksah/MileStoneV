-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[register_data_collector_new_application_id]') AND type = 'P')
DROP PROCEDURE [register_data_collector_new_application_id]
GO

CREATE PROCEDURE [register_data_collector_new_application_id]
	@application_id UNIQUEIDENTIFIER,
	@old_application_id UNIQUEIDENTIFIER,
	@host_name NVARCHAR(100), 
	@version INT
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)
  
	BEGIN TRY
		DECLARE @data_collector_id BIGINT
		DECLARE @existing_collector_id BIGINT
		DECLARE @existing_host_name NVARCHAR(200)

		BEGIN TRANSACTION

		EXEC @data_collector_id = [register_data_collector_transaction] @application_id, @host_name, @version
	
		-- Update registered services using the old data collector link to the old application id
		SELECT @existing_collector_id = [pkid]
		FROM [DataCollector]
		WHERE [application_id] = @old_application_id

		UPDATE [RegisteredServers]
		SET [DataCollectorId] = @data_collector_id
		WHERE [DataCollectorId] = @existing_collector_id
	
		COMMIT TRANSACTION

		SELECT CAST(@data_collector_id AS INT)
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