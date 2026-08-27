-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_failover_server]') AND type = 'P')
DROP PROCEDURE [create_failover_server]
GO

CREATE PROCEDURE [create_failover_server]
	@pkid [GuidType],
	@ServiceId [GuidType],
	@parent_id [GuidType],
	@Name [NameType],
	@Description [DescriptionType],
	@HostName NameType,
	@GroupSequence SMALLINT,
	@UdpPort INT,
	@DatabaseLocation [DescriptionType],
	@CertificateConfigured BIT,
	@WebServerHostName NameType,
	@WebServerPort INT
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
	
		DECLARE @tmpId [GuidType]
	
		-- Verify that the server group exists
		SELECT @tmpId = [IDServerGroup]
		FROM [ServerGroup] WITH(UPDLOCK)
		WHERE [IDServerGroup] = @parent_id

		IF(@tmpId IS NULL)
		BEGIN
			;THROW 61012, @procedureName, 1;
		END

		DECLARE @numberOfServers int
		SET @numberOfServers = (
			SELECT COUNT(fs.[GroupSequence]) 
			FROM [FailoverServers] fs, [ServerGroupMembership] sgm
			WHERE sgm.[IDServerGroup] = @parent_id
			AND sgm.[IDObject] = fs.[IDFailoverServer])

		BEGIN TRY
			INSERT [FailoverServers] (
				[IDFailoverServer],
				[Name],
				[Description],
				[HostName],
				[UdpPort],
				[Enabled],
				[GroupSequence],
				[DatabaseLocation],
				[x64],
				[IDRecordingServerService],
				[LastModified]
			)
			VALUES (
				@pkid,
				@Name,
				@Description,
				@HostName,
				@UdpPort,
				1, -- When failover servers are created we always enable them
				@numberOfServers,
				@DatabaseLocation,
				1,
				@ServiceId,
				@CurrentUtcTime
			)
		END TRY
		BEGIN CATCH

			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists
		
				-- Lets find the ServerGroup name for the error message
				DECLARE @serverGroupName [NameType]

				SELECT @serverGroupName = sg.[Name]
				FROM [ServerGroup] sg
				WHERE sg.[IDServerGroup] = @parent_id

				;THROW 61012, @procedureName, 1;
			END
			;THROW
		END CATCH

		-- Register the object in the security system
		EXEC security_register_object_transaction
			@namespace_name = 'FailoverServer',
			@object_id = @pkid,
			@parent_path = '/',
			@CurrentUtcTime = @CurrentUtcTime

		-- Register object as a server group member
		EXEC [insert_server_group_member]
			@IDObject = @pkid,
			@IDServerGroup = @parent_id,
			@ObjectType = 'FAILOVERSERVER'

		-- Also register the failover recording server.
		-- Most of these values are updated by the failover recording server the
		-- first time it connects to the management server.
		IF NOT EXISTS (SELECT [pkid] FROM [RecordingServerService] WHERE [pkid] = @pkid)
		BEGIN
			INSERT [RecordingServerService] (
				[last_modified], 
				[pkid], 
				[host_name], 
				[certificate_configured], 
				[webserver_hostname], 
				[webserver_port],
				[scan_ranges], 
				[time_zone_name], 
				[failover_udp_port], 
				[devicepack_info], 
				[x64],
				[database_port],
				[version]
			)
			VALUES (
				GETUTCDATE(),
				@pkid,
				@HostName,
				@CertificateConfigured,
				@WebServerHostName,
				@WebServerPort,
				N'<?xml version="1.0"?><AddressIntervals />',
				N'Romance Standard Time',
				11000,
				N'<?xml version="1.0" encoding="utf-8"?><DevicePackInfo />',
				1,
				5210,
				'0.0.0.0'
			)

			-- Insert a default row in configurable. 
			-- If you change the default, see recording_server_create_or_update.sql where the same row is inserted.
			INSERT INTO [dbo].[RecordingServerServiceConfigurable] (
				[LastModified],
				[PKID],
				[MulticastServerAddress],
				[ShutdownOnStorageFailure],
				[PublicAccessEnabled],
				[PublicWebServerHostName],
				[PublicWebServerPort]
			)
			VALUES (
				GETUTCDATE(),
				@pkid,
				'0.0.0.0',
				0,
				0,
				'',
				0
			)
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