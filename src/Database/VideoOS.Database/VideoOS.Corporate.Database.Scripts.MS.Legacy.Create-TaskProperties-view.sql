---- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Revising the validation of tasks. This procedure is not used any more
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[validate_task_creation_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [validate_task_creation_transaction]
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[TaskProperties]'))
DROP VIEW [dbo].[TaskProperties]
GO

CREATE VIEW TaskProperties
AS
SELECT 
	t.[pkid] AS TaskId,
	TaskProperty.value('../../task[1]/@type', 'VARCHAR(200)') AS TaskType,
	TaskProperty.value('local-name(.)', 'VARCHAR(200)') AS TaskPropertyName,
	TaskProperty.value('.', 'VARCHAR(200)') as TaskPropertyValue
FROM [Task] t
-- Cross apply all child nodes of the task xml definition (data_a column of Task table)
CROSS APPLY t.[data_a].nodes('/task/*') MyTaskProperties(TaskProperty)
GO
