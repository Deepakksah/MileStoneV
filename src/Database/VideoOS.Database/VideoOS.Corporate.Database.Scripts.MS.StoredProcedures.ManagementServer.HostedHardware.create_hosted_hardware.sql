-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_hosted_hardware]') AND type = 'P')
DROP PROCEDURE [create_hosted_hardware]
GO

CREATE PROCEDURE [create_hosted_hardware]
	@type NVARCHAR(256),
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
		BEGIN TRANSACTION

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()
	
		-- Has this hardware been already added?
		IF EXISTS(
			SELECT *
			FROM [HostedHardware] WITH(UPDLOCK)
			WHERE [manufacturer_id] = @manufacturer_id
		)
		BEGIN
			;THROW 60616, @procedureName, 1;
		END

		BEGIN TRY
			INSERT [HostedHardware] (
				[created_date],
				[last_modified],
				[type],
				[name],
				[description],
				[user_name],
				[password],
				[hosting_info],
				[manufacturer_id],
				[custom_data],
				[hosting_server_id]
			)
			VALUES (
				@CurrentUtcTime,
				@CurrentUtcTime,
				@type,
				@name,
				@description,
				@user_name,
				@password,
				@hosting_info,
				@manufacturer_id,
				@custom_data,
				@hosting_server_id
			)
		END TRY
		BEGIN CATCH

			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists
				SET @ErrorArgs = @procedureName + N': %%name=' + @name + N'%%'
				;THROW 60611, @ErrorArgs, 1;
			END

			;THROW 60000, @procedureName, 1;
		END CATCH
	
		DECLARE @pkid INT
		DECLARE @RecorderServiceId [GuidType]
	
		IF EXISTS(SELECT [pkid] FROM [HostedHardware] WHERE [name] = @name)
		BEGIN
			SET @pkid = (SELECT	[pkid] FROM [HostedHardware] WHERE [name] = @name AND [hosting_server_id] = @hosting_server_id)
		
			DECLARE @diffgram XML
			SET @diffgram = dbo.[hostedHardwareAsDataSet](@pkid)

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
					-- Create diffgram
					EXEC [create_task_transaction]
						@type = 'diffgram_insert',
						@target = @RecorderServiceId,
						@data_a = @diffgram,
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
		END
		ELSE
		BEGIN
			SET @pkid = -1
		END

		COMMIT TRANSACTION

		-- AudithostedHardwareAdded = 20213
		EXEC create_pending_log @session_id, 20213, @pkid, @name

		SELECT
			[pkid],
			[created_date],
			[last_modified],
			[type],
			[name],
			[description],
			[user_name],
			[password],
			[hosting_info],
			[manufacturer_id],
			[custom_data],
			[hosting_server_id]
		FROM [HostedHardware]
		WHERE [pkid] = @pkid
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