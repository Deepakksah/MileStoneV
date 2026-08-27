-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS (SELECT 1 FROM sys.views WHERE [object_id] = OBJECT_ID(N'[dbo].[TaskProperties]'))
DROP VIEW [dbo].[TaskProperties]
GO

CREATE VIEW TaskProperties
AS
SELECT	t.[pkid] AS TaskId,
		t.[type] AS TaskType,
		TaskPropertyName = CASE tp.value('local-name(.)', 'VARCHAR(200)')
		WHEN 'idStorage' THEN 'storage_id'			-- Unifying storage id property names, for simpler queries when using this view
		WHEN 'idStorageTarget' THEN 'storage_id'
		WHEN 'idDevice' THEN 'device_id'			-- Unifying device id property names, for simpler queries when using this view
		ELSE tp.value('local-name(.)', 'VARCHAR(200)')
		END,
		tp.value('.' , 'VARCHAR(200)') as TaskPropertyValue
FROM	[Task] t WITH(UPDLOCK, HOLDLOCK)
		-- Cross apply all child nodes of the task xml definition (data_a column of Task table)
		CROSS APPLY t.[data_a].nodes('/task/*') MyTaskProperties(tp)
WHERE   t.[type] NOT IN ('Database.Archiving', 'Database.Repairing2')	-- These are never considered conflicting tasks anyway
		AND t.[type] LIKE 'Database.%'									-- Currently we are only using this table with database tasks
		AND t.[task_status] NOT IN (2, 3)								-- Ignore tasks that are Succeeded (2) or Failed (3)
GO
