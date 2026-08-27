-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_all_task_ids]') AND type = 'P')
DROP PROCEDURE [query_all_task_ids]
GO

CREATE PROCEDURE [query_all_task_ids]
AS
BEGIN
	SET NOCOUNT ON

	SELECT [pkid] AS task_id
	FROM [Task]
	WHERE [type] NOT IN (
				'diffgram_insert',
				'diffgram_update',
				'diffgram_delete',
				'diffgram_security_insert',
				'diffgram_security_update',
				'diffgram_security_delete'
		)

END
GO
