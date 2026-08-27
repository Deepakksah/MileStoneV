-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_create_failover_history_entry]') AND type = 'P')
DROP PROCEDURE [admin_create_failover_history_entry]
GO

CREATE PROCEDURE [admin_create_failover_history_entry]
	@IDFailoverHistoryEntry [GuidType],
	@IDFailover [GuidType],
	@Description [DescriptionType]
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		BEGIN TRY
			DECLARE @CurrentUtcTime DATETIME
			SET @CurrentUtcTime = GETUTCDATE()

			INSERT [FailoverHistory] (
				[IDFailoverHistoryEntry],
				[IDFailover],
				[Description],
				[LastModified]
			)
			VALUES (
				@IDFailoverHistoryEntry,
				@IDFailover,
				@Description,
				@CurrentUtcTime
			)
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				;THROW 60045, @procedureName, 1;
			END
			;THROW
		END CATCH

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