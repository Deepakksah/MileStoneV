-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[set_failover_recorder_information]') AND type in (N'P', N'PC'))
DROP PROCEDURE [set_failover_recorder_information]
GO

CREATE PROCEDURE [set_failover_recorder_information]
	@IDFailoverServer [GuidType],
	@CertificateConfigured BIT,
	@WebServerHostName [NameType],
	@WebServerPort INT,
	@Version [NameType],
	@TimeZoneName [NameType]

AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		DECLARE @recording_server_service_id AS [GuidType]
		DECLARE @recorder_id AS [GuidType]

		-- Make sure the failover service exists and look up recording server service id
		SELECT @recording_server_service_id = fs.[IDRecordingServerService]
		FROM [FailoverServers] fs WITH(UPDLOCK)
		WHERE fs.[IDFailoverServer] = @IDFailoverServer

		IF (@recording_server_service_id IS NULL)
		BEGIN
			SET @ErrorArgs = @procedureName + N': %%IDFailoverServer=' + CAST(@IDFailoverServer AS NVARCHAR(36)) + N'%%'
			;THROW 60039, @ErrorArgs, 1;
		END

		-- Make sure that the associated recording server service exists
		IF NOT EXISTS (
			SELECT [pkid]
			FROM [RecordingServerService] WITH(UPDLOCK)
			WHERE [pkid] = @recording_server_service_id)
		BEGIN
			SET @ErrorArgs = @procedureName + N': %%IDFailoverServer=' + CAST(@IDFailoverServer AS NVARCHAR(36)) + N'%%'
			;THROW 60039, @ErrorArgs, 1;
		END

		UPDATE [RecordingServerService]
		SET	[last_modified] = @CurrentUtcTime, 
			[certificate_configured] = @CertificateConfigured,
			[webserver_hostname] = @WebServerHostName,
			[webserver_port] = @WebServerPort,
			[version] = @Version,
			[time_zone_name] = @TimeZoneName
		WHERE 
			[pkid] = @recording_server_service_id

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