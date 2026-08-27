-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_stopping_tasks_for_target]') AND type = 'P')
DROP PROCEDURE [query_stopping_tasks_for_target]
GO

CREATE PROCEDURE [query_stopping_tasks_for_target]
	@target_id [GuidType]
AS
BEGIN
	SET NOCOUNT ON

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
		AND [task_status] = 4 -- task_status Stopping=4 
END
GO
