-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_update_failover_history_entry]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_update_failover_history_entry]
GO

CREATE PROCEDURE [admin_update_failover_history_entry]
	@IDFailoverHistoryEntry [GuidType],
	@IDFailover [GuidType],
	@Description [DescriptionType],
	@IDRecorder [GuidType],
	@StartTime [DATETIME],
	@EndTime [DATETIME]
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

		-- Make sure the entry exists
		IF NOT EXISTS (
			SELECT [IDFailoverHistoryEntry]
			FROM [FailoverHistory] WITH(UPDLOCK)
			WHERE [IDFailoverHistoryEntry] = @IDFailoverHistoryEntry)
		BEGIN
			;THROW 60000, @procedureName, 1;
		END

		UPDATE [FailoverHistory]
		SET [IDFailover] = @IDFailover,
			[Description] = @Description,
			[IDRecorder] = @IDRecorder,
			[StartTime] = @StartTime,
			[EndTime] = @EndTime,
			[LastModified] = @CurrentUtcTime
		WHERE [IDFailoverHistoryEntry] = @IDFailoverHistoryEntry

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