-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[request_stop_on_task]') AND type = 'P')
DROP PROCEDURE [request_stop_on_task]
GO

CREATE PROCEDURE [request_stop_on_task]
	@pkid BIGINT
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRANSACTION

		-- task_status enum:
		--	Inactive,
		--	InProgress,
		--	Succeeded,
		--	Failed,
		--	Stopping,
		--	Stopped

		-- Make sure the task exists and that it can be stopped (has task_status Inactive=0 or InProgress=1)
		IF NOT EXISTS (SELECT [pkid], [task_status] FROM [Task] WITH(UPDLOCK) 
			WHERE [pkid] = @pkid AND ([task_status] = 0 OR [task_status] = 1))
		BEGIN
			ROLLBACK TRAN
			-- If the task does not exist we don't raise errors on the stop call
			RETURN
		END

		UPDATE [Task]
		SET [task_status] = 4 -- Setting it to "Stopping" status
		WHERE [pkid] = @pkid

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