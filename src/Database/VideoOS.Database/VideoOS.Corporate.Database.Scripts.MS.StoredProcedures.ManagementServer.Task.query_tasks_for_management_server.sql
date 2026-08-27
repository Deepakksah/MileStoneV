-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_tasks_for_management_server]') AND type = 'P')
DROP PROCEDURE [query_tasks_for_management_server]
GO

CREATE PROCEDURE [query_tasks_for_management_server]
AS
BEGIN
  SET NOCOUNT ON

  SELECT [pkid]
      ,[type]
      ,[target]
      ,[data_a]
      ,[data_b]
      ,[percent_completed]
      ,[status_details]
      ,[task_status]
      ,[start_time]
      ,[end_time]
  FROM [Task]
  WHERE [target] = '909749AE-88C4-4780-A381-383D691E98FA' AND [data_a].exist('(task/idRecorder)[1]') = 0
    AND [type] NOT IN (
		'diffgram_insert',
		'diffgram_update',
		'diffgram_delete',
		'diffgram_security_insert',
		'diffgram_security_update',
		'diffgram_security_delete')
  ORDER BY [pkid] ASC
END
GO
