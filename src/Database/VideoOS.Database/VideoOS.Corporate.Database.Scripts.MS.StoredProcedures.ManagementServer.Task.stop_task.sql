-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[stop_task]') AND type = 'P')
DROP PROCEDURE [stop_task]
GO

CREATE PROCEDURE [stop_task]
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

		-- Make sure the task exists
		IF NOT EXISTS (SELECT [pkid] FROM [Task] WITH(UPDLOCK) WHERE [pkid] = @pkid)
		BEGIN
			;THROW 60501, @procedureName, 1;
		END

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		UPDATE [Task]
		SET [task_status] = 5, -- task_status Stopped=5
			[end_time] = @CurrentUtcTime
		WHERE [pkid] = @pkid AND
			[task_status] <> 5

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