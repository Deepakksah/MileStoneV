-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_system_setting]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_system_setting]
GO

CREATE PROCEDURE [update_system_setting]
	@settingName [NameType],
	@settingValue VARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRANSACTION

		DECLARE @rowCount INT

		DECLARE @currentUtcTime DATETIME
		SET @currentUtcTime = GETUTCDATE()
	
		IF(NOT EXISTS(SELECT [Name] FROM [SystemSettings] WHERE [Name] = @settingName))
		BEGIN
			SET @ErrorArgs = @procedureName + N': %%Name=' + @settingName + N'%%'
			;THROW 65008, @ErrorArgs, 1;
		END
	
		DECLARE @dataSetBefore XML
		DECLARE @dataSetAfter XML

		-- Get DataSet before changes
		SET @dataSetBefore = dbo.systemSettingsAsDataSet(@settingName)

		UPDATE	[SystemSettings]
		SET		[Setting] = @settingValue,
				[LastModified] = @currentUtcTime
		WHERE	[Name] = @settingName
		AND		dbo.StringValueChanging([Setting], @settingValue) = 1

		SET @rowcount = @@ROWCOUNT

		-- Get DataSet after changes
		SET @dataSetAfter = dbo.systemSettingsAsDataSet(@settingName)
	
		IF @rowcount <> 0
		BEGIN
			-- Create diffgram
			EXEC [create_task_transaction]
				@type = 'diffgram_update',
				@target = '909749AE-88C4-4780-A381-383D691E98FA',
				@data_a = @dataSetBefore,
				@data_b = @dataSetAfter,
				@current_time = @CurrentUtcTime
		END

		COMMIT TRANSACTION

		SELECT @rowcount as [RowCount]
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