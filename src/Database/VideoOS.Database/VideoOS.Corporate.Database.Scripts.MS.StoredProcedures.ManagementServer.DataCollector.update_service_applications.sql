-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_service_applications]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_service_applications]
GO

CREATE PROCEDURE [update_service_applications]
	@data_collector_id INT,
	@service_applications XML
AS
BEGIN
	SET NOCOUNT ON;

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()
	
	BEGIN TRY
		BEGIN TRANSACTION

		IF(@service_applications IS NULL)
		BEGIN
			ROLLBACK TRANSACTION
			RETURN 0
		END

		-- Delete existing service applications to update
		DELETE [DataCollectorInstalledServices]
		WHERE [data_collector_id] = @data_collector_id


		-- Insert new server types
		INSERT [DataCollectorInstalledServices] ([data_collector_id], [service_name])
		SELECT @data_collector_id, st.[name]
		FROM @service_applications.nodes('ServiceApplications/ServiceApplication') AS T(property)
		INNER JOIN [ServerType] st ON st.[name] = T.property.value('@Name', 'NameType')
	
		COMMIT TRANSACTION
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
