-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[cleanup_task]') AND type = 'P')
DROP PROCEDURE [cleanup_task]
GO

CREATE PROCEDURE [cleanup_task]
	@pkid INT
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)
	
	BEGIN TRY
		BEGIN TRANSACTION

		DECLARE @task_status INT

		SELECT 
			@task_status = [task_status]
		FROM
			[Task]
		WHERE
			[pkid] = @pkid

		-- Does the task exist?
		IF (@task_status IS NULL)
		BEGIN
			-- Silently return - task already cleaned up
			COMMIT TRAN
			RETURN 0
		END

		-- task_status must be Succeeded (2), Failed (3) or Stopped (5) in order to be cleaned up
		IF (@task_status <> 2 AND @task_status <> 3 AND @task_status <> 5)
		BEGIN
			;THROW 60158, @procedureName, 1;
		END

		DELETE [Task]
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