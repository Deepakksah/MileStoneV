-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_recording_server_failover]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_recording_server_failover]
GO

CREATE PROCEDURE [update_recording_server_failover]
	@IDRecorder [GuidType],
	@IDPrimary [GuidType],
	@IDSecondary [GuidType],
	@Port INT
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

		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		-- Make sure the recording server exists
		IF NOT EXISTS (SELECT [IDRecorder] FROM [Recorders] WITH(UPDLOCK) WHERE [IDRecorder] = @IDRecorder)
		BEGIN
			;THROW 60047, @procedureName, 1;
		END

		SET @dataSetBefore = dbo.recordersAsDataSet(@IDRecorder)
		UPDATE [Recorders]
		SET	[IDFailoverPrimaryGroup] = @IDPrimary,
			[IDFailoverSecondaryGroup] = @IDSecondary,
			[LastModified] = GETUTCDATE()
		WHERE [IDRecorder] = @IDRecorder

		UPDATE [RecordingServerService]
		SET	[failover_udp_port] = @Port,
			[last_modified] = GETUTCDATE()
		WHERE [pkid] = @IDRecorder

		SET @dataSetAfter = dbo.recordersAsDataSet(@IDRecorder)

		EXEC [create_task_transaction]
			'diffgram_update',
			'909749AE-88C4-4780-A381-383D691E98FA',
			@dataSetBefore,
			@dataSetAfter,
			@CurrentUtcTime

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