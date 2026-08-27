-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_hosting_server]') AND type = 'P')
DROP PROCEDURE [update_hosting_server]
GO

CREATE PROCEDURE [update_hosting_server]
	@pkid INT,
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
		DECLARE @rowcount INT

		DECLARE @dataSetBefore XML
		DECLARE @dataSetAfter XML

		DECLARE @auditHostingServerRenamed BIT
		DECLARE @auditHostingServerModified BIT

		BEGIN TRANSACTION

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		DECLARE @hosting_server_id INT
		DECLARE @currentHostingServerName NVARCHAR(256)

		SELECT @hosting_server_id = [pkid], @currentHostingServerName = [name]
		FROM [HostingServer] WITH(UPDLOCK)
		WHERE [pkid] = @pkid

		-- Make sure the hosting server exists
		IF(@hosting_server_id IS NULL)
		BEGIN
			SET @ErrorArgs = @procedureName + N': %%pkid=' + CAST(@pkid AS NVARCHAR(10)) + N'%%'
			;THROW 60603, @ErrorArgs, 1;
		END

		-- Get DataSet before changes
		SET @dataSetBefore = dbo.[hostingServerAsDataSet](@pkid)

		BEGIN TRY
			UPDATE	[HostingServer]
			SET		[last_modified] = @CurrentUtcTime,
					[name] = @name
			WHERE	[pkid] = @pkid
					AND dbo.StringValueChanging([name], @name) = 1

			SET @rowcount = @@ROWCOUNT
		END TRY
		BEGIN CATCH

			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists
				SET @ErrorArgs = @procedureName + N': %%name=' + @name + N'%%'
				;THROW 60602, @ErrorArgs, 1;
			END

			;THROW;
		END CATCH

		IF (@rowcount > 0)
		BEGIN
			-- AuditHostingServerRenamed = 20216
			SET @auditHostingServerRenamed = 1
		END

		UPDATE	[HostingServer]
		SET		[last_modified] = @CurrentUtcTime,
				[description] = @description,
				[user_name] = @user_name,
				[password] = @password,
				[internal_address] = @internal_address,
				[external_address] = @external_address
		WHERE	[pkid] = @pkid
				AND (	dbo.StringValueChanging([description], @description) = 1
					OR dbo.StringValueChanging([user_name], @user_name) = 1
					OR dbo.StringValueChanging([password], @password) = 1
					OR dbo.StringValueChanging([internal_address], @internal_address) = 1
					OR dbo.StringValueChanging([external_address], @external_address) = 1)
			
		SET @rowcount = @@ROWCOUNT

		IF (@rowcount > 0)
		BEGIN
			-- AuditHostingServerModified = 20211
			SET @auditHostingServerModified = 1
		END

		-- Get DataSet after changes
		SET @dataSetAfter = dbo.[hostingServerAsDataSet](@pkid)

		-- Create diffgram task for all recorder services
		EXEC [create_task_recorder_services_transaction]
			@type = 'diffgram_update',
			@data_a = @dataSetBefore,
			@data_b = @dataSetAfter,
			@current_time = @CurrentUtcTime
	
		COMMIT TRANSACTION

		IF (@auditHostingServerRenamed = 1)
		BEGIN
			EXEC create_pending_log @session_id, 20216, @pkid, @name, @currentHostingServerName
		END

		IF (@auditHostingServerModified = 1)
		BEGIN
			EXEC create_pending_log @session_id, 20211, @pkid, @name
		END
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