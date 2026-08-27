-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_pending_tasks_for_target]') AND type = 'P')
DROP PROCEDURE [query_pending_tasks_for_target]
GO

CREATE PROCEDURE [query_pending_tasks_for_target]
	@target_id [GuidType],
	@sequence_number BIGINT,
	@delete_old BIT
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRANSACTION

		IF(@delete_old = 1)
		BEGIN
			DELETE FROM [Task] WITH(READCOMMITTED)
			WHERE
				[target] = @target_id AND
				[pkid] <= @sequence_number AND
				[type] IN (
					'diffgram_insert',
					'diffgram_update',
					'diffgram_delete',
					'diffgram_security_insert',
					'diffgram_security_update',
					'diffgram_security_delete'
				)
		END

		SELECT
			[pkid],
			[type],
			[data_a],
			[data_b],
			[start_time],
			[status_details],
			[task_status]
		FROM [Task]
		WHERE [target] = @target_id 
			AND [pkid] > @sequence_number

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