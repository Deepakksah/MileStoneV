-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_hosted_hardware]') AND type = 'P')
DROP PROCEDURE [update_hosted_hardware]
GO

CREATE PROCEDURE [update_hosted_hardware]
	@pkid INT,
	@name NVARCHAR(256),
	@description NVARCHAR(MAX),
	@user_name NVARCHAR(256),
	@password NVARCHAR(256),
	@hosting_info NVARCHAR(MAX),
	@manufacturer_id NVARCHAR(MAX),
	@custom_data NVARCHAR(MAX),
	@hosting_server_id INT,
	@session_id [GuidType],
	@custom_properties XML

AS
BEGIN
	SET NOCOUNT ON;

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @rowcount INT

		DECLARE @dataSetBefore XML
		DECLARE @dataSetAfter XML

		BEGIN TRANSACTION

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		DECLARE @hosted_hardware_id INT
		DECLARE @currentHostedHardwareName NVARCHAR(256)

		SELECT @hosted_hardware_id = [pkid], @currentHostedHardwareName = [name]
		FROM [HostedHardware] WITH(UPDLOCK)
		WHERE [pkid] = @pkid

		-- Make sure the hosted hardware exists
		IF(@hosted_hardware_id IS NULL)
		BEGIN
			SET @ErrorArgs = @procedureName + N': %%pkid=' + CAST(@pkid AS NVARCHAR(10)) + N'%%'
			;THROW 60613, @ErrorArgs, 1;
		END

		-- Get DataSet before changes
		SET @dataSetBefore = dbo.[hostedHardwareAsDataSet](@pkid)

		BEGIN TRY
			UPDATE [HostedHardware]
			SET [last_modified] = @CurrentUtcTime,
				[name] = @name
			WHERE [pkid] = @pkid
				AND dbo.StringValueChanging([name], @name) = 1

			SET @rowcount = @@ROWCOUNT
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists
				SET @ErrorArgs = @procedureName + N': %%name=' + @name + N'%%'
				;THROW 60612, @ErrorArgs, 1;
			END	
			;THROW 60000, @procedureName, 1;
		END CATCH	

		IF (@rowcount > 0)
		BEGIN
			-- AudithostedHardwareRenamed = 20217
			EXEC create_pending_log @session_id, 20217, @pkid, @name, @currentHostedHardwareName
		END

		BEGIN TRY
			UPDATE [HostedHardware]
			SET [last_modified] = @CurrentUtcTime,
				[description] = @description,
				[user_name] = @user_name,
				[password] = @password,
				[hosting_info] = @hosting_info,
				[manufacturer_id] = @manufacturer_id,
				[custom_data] = @custom_data,
				[hosting_server_id] = @hosting_server_id
			WHERE [pkid] = @pkid
				AND (	dbo.StringValueChanging([description], @description) = 1
						OR dbo.StringValueChanging([user_name], @user_name) = 1
						OR dbo.StringValueChanging([password], @password) = 1
						OR dbo.StringValueChanging([hosting_info], @hosting_info) = 1
						OR dbo.StringValueChanging([manufacturer_id], @manufacturer_id) = 1
						OR dbo.StringValueChanging([custom_data], @custom_data) = 1
						OR [hosting_server_id] <> @hosting_server_id )

			SET @rowcount = @@ROWCOUNT
		END TRY
		BEGIN CATCH
			;THROW 60000, @procedureName, 1;
		END CATCH

		IF (@rowcount > 0)
		BEGIN
			-- AudithostedHardwareModified = 20214
			EXEC create_pending_log @session_id, 20214, @pkid, @name
		END

		-- Get DataSet after changes
		SET @dataSetAfter = dbo.[hostedHardwareAsDataSet](@pkid)

		DECLARE @RecorderServiceId [GuidType]

		-- find the recorders that will receive this diffgram
		DECLARE recorder_cursor CURSOR FOR 
			SELECT DISTINCT rssr.[recording_server_service_id]
			FROM [RecordingServerServiceRecorder] rssr
			INNER JOIN [Hardware] hw ON hw.[IDRecorder] = rssr.[recorder_id]
			INNER JOIN [HostedHardware] hh ON hh.[pkid] = hw.[HostedHardware_id]
			WHERE hh.[pkid] = @pkid

		OPEN recorder_cursor
		BEGIN TRY
			FETCH NEXT FROM recorder_cursor
			INTO @RecorderServiceId

			WHILE(@@FETCH_STATUS = 0)
			BEGIN
				-- Send diffgram to all recorders it involves
				EXEC [create_task_transaction]
					@type = 'diffgram_update',
					@target = @RecorderServiceId,
					@data_a = @dataSetBefore,
					@data_b = @dataSetAfter,
					@current_time = @CurrentUtcTime

				FETCH NEXT FROM recorder_cursor
				INTO @RecorderServiceId
			END
		END TRY
		BEGIN CATCH
			CLOSE recorder_cursor
			DEALLOCATE recorder_cursor
			;THROW
		END CATCH

		CLOSE recorder_cursor
		DEALLOCATE recorder_cursor

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