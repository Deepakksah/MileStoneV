-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_time_profile]') AND type = 'P')
DROP PROCEDURE [update_time_profile]
GO

CREATE PROCEDURE [update_time_profile]
	@pkid GuidType,
	@name NameType,
	@description NVARCHAR(MAX),
	@xml_data XML,
	@custom_properties XML,
	@session_id [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)
	DECLARE @rowcount INT

	BEGIN TRY
		DECLARE @dataSetBefore XML
		DECLARE @dataSetAfter XML

		BEGIN TRANSACTION

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		DECLARE @timeprofile_id [GuidType]
		DECLARE @currentTimeProfileName [NameType]

		SELECT @timeprofile_id = [pkid], @currentTimeProfileName = [name]
		FROM [TimeProfile] WITH(UPDLOCK)
		WHERE [pkid] = @pkid

		-- Make sure the time profile exists
		IF(@timeprofile_id IS NULL)
		BEGIN
			;THROW 60221, @procedureName, 1;
		END

		-- Get DataSet before changes
		SET @dataSetBefore = dbo.timeProfileAsDataSet(@pkid)

		BEGIN TRY

			IF(@name IS NOT NULL)
			BEGIN
				-- Update time profile name
				UPDATE [TimeProfile]
				SET [name] = @name,
					[last_modified] = @CurrentUtcTime
				WHERE [pkid] = @pkid
					AND dbo.StringValueChanging([Name], @name) = 1

				SELECT @rowcount = @@ROWCOUNT

				IF (@rowcount > 0)
				BEGIN
					--	Log an entry similar to "Time profile '{0}' renamed to '{1}'"
					EXEC create_pending_log @session_id, 8004, @timeprofile_id, @name, @currentTimeProfileName
				END
			END

			IF(@xml_data IS NOT NULL)
			BEGIN
				-- Update time profile definition
				UPDATE [TimeProfile]
				SET [xml_data] = @xml_data,
					[last_modified] = @CurrentUtcTime
				WHERE [pkid] = @pkid

				SELECT @rowcount = @@ROWCOUNT

				IF (@rowcount > 0)
				BEGIN
					--	Log an entry similar to "Time profile '{0}' edited"
					EXEC create_pending_log @session_id, 8003, @timeprofile_id, @name
				END
			END

			IF(@description IS NOT NULL)
			BEGIN
			    -- Update time profile description
				UPDATE [TimeProfile]
				SET [description] = @description,
					[last_modified] = @CurrentUtcTime
				WHERE [pkid] = @pkid
			END

		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists
				SET @ErrorArgs = @procedureName + N': %%Name=' + @Name + N'%%'
				;THROW 60222, @ErrorArgs, 1;
			END
			;THROW;
		END CATCH

		-- Get DataSet after changes
		SET @dataSetAfter = dbo.[timeProfileAsDataSet](@pkid)

		-- Call the create task procedure to create diffgram update task
		EXEC [create_task_transaction]
			@type = 'diffgram_update',
			@target = '909749AE-88C4-4780-A381-383D691E98FA',
			@data_a = @dataSetBefore,
			@data_b = @dataSetAfter,
			@current_time = @CurrentUtcTime
	
		-- Update custom properties
		EXEC [update_custom_properties_transaction]
			@object_id = @pkid,
			@custom_properties = @custom_properties

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