-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


-- Add new calculated column for id of the instance
IF EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'MonitorStates') AND name = N'counter_id')
BEGIN 
	ALTER TABLE [MonitorStates]
	DROP COLUMN [counter_id]
END  

ALTER TABLE [MonitorStates]
ADD	[counter_id] AS 
		(CASE WHEN 
			CHARINDEX('[', [counter_path]) != 0 AND
			SUBSTRING([counter_path], CHARINDEX('[', [counter_path]), 1) = '[' AND -- Ensure we have a '[' 37 characters before enclosing ']'
			SUBSTRING([counter_path], CHARINDEX('[', [counter_path]) + 1, 36) LIKE '[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]'
		THEN SUBSTRING([counter_path], CHARINDEX('[', [counter_path]) + 1, 36)
		ELSE 
			CASE WHEN
				CHARINDEX('(', [counter_path]) != 0 AND
				SUBSTRING([counter_path], CHARINDEX('(', [counter_path]), 1) = '(' AND -- Ensure we have a '[' 37 characters before enclosing ']'
				SUBSTRING([counter_path], CHARINDEX('(', [counter_path]) + 1, 36) LIKE '[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]'
				THEN SUBSTRING([counter_path], CHARINDEX('(', [counter_path]) + 1, 36)
			ELSE NULL END
		END) PERSISTED									
GO