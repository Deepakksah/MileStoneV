-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_completed_tasks_by_device_id_before_time]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_completed_tasks_by_device_id_before_time]
GO

CREATE PROCEDURE [delete_completed_tasks_by_device_id_before_time]
	@IDDevice [GuidType],
	@Time [DateTime]
AS	
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRANSACTION

		DELETE [Task]
		WHERE data_a.value('(/task/idDevice)[1]', 'GuidType') = @IDDevice AND data_a.value('(/task/endTime)[1]', 'DateTime') < @Time AND
		(task_status = 2 OR task_status = 3)

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