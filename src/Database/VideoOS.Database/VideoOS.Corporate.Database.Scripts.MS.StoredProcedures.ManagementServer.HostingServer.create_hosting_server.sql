-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_hosting_server]') AND type = 'P')
DROP PROCEDURE [create_hosting_server]
GO

CREATE PROCEDURE [create_hosting_server]
	@type NVARCHAR(256),
	@name NVARCHAR(256),
	@description NVARCHAR(MAX),
	@user_name NVARCHAR(256),
	@password NVARCHAR(256),
	@internal_address NVARCHAR(256),
	@external_address NVARCHAR(256),
	@session_id [GuidType],
	@custom_properties XML
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRANSACTION

		DECLARE @pkid INT
		DECLARE @dataSetAfter XML

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		BEGIN TRY
			INSERT	[HostingServer] (
					[created_date],
					[last_modified],
					[type],
					[name],
					[description],
					[user_name],
					[password],
					[internal_address],
					[external_address]
			)
			VALUES (
					@CurrentUtcTime,
					@CurrentUtcTime,
					@type,
					@name,
					@description,
					@user_name,
					@password,
					@internal_address,
					@external_address
			)

			SET @pkid = SCOPE_IDENTITY()
		END TRY
		BEGIN CATCH

			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists
				SET @ErrorArgs = @procedureName + N': %%name=' + @name + N'%%'
				;THROW 60601, @ErrorArgs, 1;
			END

			;THROW;
		END CATCH
	
		SET @dataSetAfter = dbo.[hostingServerAsDataSet](@pkid)

		EXEC [create_task_recorder_services_transaction]
			@type = 'diffgram_insert',
			@data_a = @dataSetAfter,
			@current_time = @CurrentUtcTime
	
		COMMIT TRANSACTION

		-- AuditHostingServerAdded = 20210
		EXEC create_pending_log @session_id, 20210, @pkid, @name

		SELECT	[pkid],
				[created_date],
				[last_modified],
				[type],
				[name],
				[description],
				[user_name],
				[password],
				[internal_address],
				[external_address]
		FROM	[HostingServer]
		WHERE	[pkid] = @pkid
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