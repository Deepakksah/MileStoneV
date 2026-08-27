-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_task]') AND type = 'P')
DROP PROCEDURE [update_task]
GO

CREATE PROCEDURE [update_task]
	@pkid BIGINT,
	@percent_completed SMALLINT,
	@status_details NVARCHAR(max),
	@task_status INT,
	@end_time DATETIME
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRANSACTION

		-- Make sure the task exists
		IF NOT EXISTS (SELECT [pkid] FROM [Task] WITH(UPDLOCK) WHERE [pkid] = @pkid)
		BEGIN
			;THROW 60502, @procedureName, 1;
		END

		-- task_status enum:
		--	Inactive,
		--	InProgress,
		--	Succeeded,
		--	Failed,
		--	Stopping,
		--	Stopped

		-- Only allow InProgress updates if task already is InProgress or Inactive
		IF (@task_status = 1)
		BEGIN
			UPDATE [Task]
			SET [status_details] = COALESCE(@status_details, [status_details]),
				[percent_completed] = @percent_completed,
				[task_status] = @task_status,
				[end_time] = @end_time
			WHERE [pkid] = @pkid AND
				([task_status] = 0 OR [task_status] = 1)
		END
		ELSE
		BEGIN
			UPDATE [Task]
			SET [status_details] = COALESCE(@status_details, [status_details]),
				[percent_completed] = @percent_completed,
				[task_status] = @task_status,
				[end_time] = @end_time
			WHERE [pkid] = @pkid
		END

		SELECT [data_a]
		FROM [Task]
		WHERE [pkid] = @pkid

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		THROW; -- Used from ServerService, where Error 60000 is NOT expected
	END CATCH;
END
GO