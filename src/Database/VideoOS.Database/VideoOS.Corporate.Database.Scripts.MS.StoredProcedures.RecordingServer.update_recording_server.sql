-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_recording_server]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_recording_server]
GO

CREATE PROCEDURE [update_recording_server]
	@IDRecorder [GuidType],
	@Name [NameType],
	@Description [DescriptionType],
	@SynchronizationTime INT,
	@custom_properties XML,
	@session_id [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @dataSetBefore XML
		DECLARE @dataSetAfter XML
		DECLARE @existingName [NameType]

		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		-- Make sure the recording server exists
		IF NOT EXISTS (SELECT [IDRecorder] FROM [Recorders] WITH(UPDLOCK) WHERE [IDRecorder] = @IDRecorder)
		BEGIN
			;THROW 60047, @procedureName, 1;
		END

		SELECT @existingName = [Name] FROM [Recorders] WITH(UPDLOCK) WHERE [IDRecorder] = @IDRecorder

		IF(@Name IS NULL AND @Description IS NULL AND @custom_properties IS NULL AND @SynchronizationTime IS NULL)
		BEGIN
			ROLLBACK TRAN
			RETURN 0
		END

		SET @dataSetBefore = dbo.recordersAsDataSet(@IDRecorder)
	
		UPDATE [Recorders]
		SET	[Name] = COALESCE(@Name, [Name]),
			[Description] = COALESCE(@Description, [Description]),
			[SynchronizationTime] = COALESCE(@SynchronizationTime, [SynchronizationTime]),
			[LastModified] = GETUTCDATE()
		WHERE [IDRecorder] = @IDRecorder

		SET @dataSetAfter = dbo.recordersAsDataSet(@IDRecorder)

		EXEC [create_task_transaction]
			'diffgram_update',
			'909749AE-88C4-4780-A381-383D691E98FA',
			@dataSetBefore,
			@dataSetAfter,
			@CurrentUtcTime

		-- Update custom properties
		IF (@custom_properties IS NOT NULL)
		BEGIN
			EXEC [update_custom_properties_transaction]
				@object_id = @IDRecorder,
				@custom_properties = @custom_properties
		END

		IF(@Name IS NOT NULL AND  @existingName <> @Name)
		BEGIN
			--	Log an entry similar to "Recording server '{old recorder name}' renamed to '{new recorder name}'"
			EXEC create_pending_log @session_id, 2012, @IDRecorder, @name, @IDRecorder, @name, @existingName
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