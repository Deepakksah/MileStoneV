-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[register_registered_server]') AND type in (N'P', N'PC'))
DROP PROCEDURE [register_registered_server]
GO

CREATE PROCEDURE [register_registered_server]
	@ServiceId [UNIQUEIDENTIFIER],
	@ServiceType [UNIQUEIDENTIFIER],
	@Addresses NVARCHAR(MAX),
	@Name [NameType],
	@Description [DescriptionType],
	@CustomData NVARCHAR(MAX),
	@EndpointDefinition NVARCHAR(MAX)
 
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)
	
	BEGIN TRY		
		BEGIN TRAN
	
		IF NOT EXISTS(SELECT * FROM [RegisteredServers] WHERE IDServer = @ServiceId)
		BEGIN
			INSERT INTO [RegisteredServers]
					([LastModified]
					,[IDServer]
					,[Type]
					,[Name]
					,[Description]
					,[Addresses]
					,[Trusted]
					,[CustomData]
					,[Enabled]
					,[EndpointDefinitions]
					,[DataCollectorId])
			VALUES
					( GETUTCDATE()
					, @ServiceId
					, @ServiceType
					, @Name
					, @Description
					, @Addresses
					, 1
					, @CustomData
					, 1
					, @EndpointDefinition
					, 0
					)
		END
		ELSE
		BEGIN 
			DECLARE @newInternalUrl VARCHAR(MAX) = (SELECT CAST(@Addresses AS XML).value('(/Addresses/Address[@type="Internal"]/@url)[1]', 'nvarchar(max)'))
			DECLARE @newExternalUrl VARCHAR(MAX) = (SELECT CAST(@Addresses AS XML).value('(/Addresses/Address[@type="External"]/@url)[1]', 'nvarchar(max)'))
			DECLARE @adrXml XML  = CAST((SELECT [Addresses] FROM [RegisteredServers] WHERE IDServer = @ServiceId) AS XML)

			IF (@newInternalUrl IS NOT NULL AND @newInternalUrl != '')
			BEGIN
				SET @adrXml.modify('replace value of (/Addresses/Address[@type="Internal"]/@url)[1] with sql:variable("@newInternalUrl")');
			END
		
			IF (@newExternalUrl IS NOT NULL AND @newExternalUrl != '')
			BEGIN
				SET @adrXml.modify('replace value of (/Addresses/Address[@type="External"]/@url)[1] with sql:variable("@newExternalUrl")');
			END

			UPDATE [RegisteredServers] SET [Addresses] = CAST (@adrXml as nvarchar(MAX)), [Name] = @Name, [Description] = @Description, [CustomData] = @CustomData, [EndpointDefinitions] = @EndpointDefinition WHERE IDServer = @ServiceId
		END

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