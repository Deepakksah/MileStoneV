-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_task_by_id]') AND type = 'P')
DROP PROCEDURE [query_task_by_id]
GO

CREATE PROCEDURE [query_task_by_id]
	@pkid BIGINT
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		[pkid],
		[type],
		[target],
		[data_a],
		[data_b],
		[percent_completed],
		[status_details], 
		[task_status],
		[start_time],
		[end_time]
	FROM [Task]
	WHERE [pkid] = @pkid 
END
GO
