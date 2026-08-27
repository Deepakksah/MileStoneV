-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Update performance counter instance name to remove configuration item name
UPDATE [dbo].[PerformanceCounterInstance] 
SET [instance_name] = '[' + counter_id + ']'
WHERE counter_id IS NOT NULL
GO

-- Update counter_path in monitor states to remove configuration item name from the path  
UPDATE [dbo].[MonitorStates] 
SET counter_path = SUBSTRING(ms.[counter_path],0,CHARINDEX('(',ms.[counter_path])) 
				   + '([' + ms.counter_id + '])' + 
				   REVERSE(SUBSTRING(REVERSE(ms.counter_path),0,CHARINDEX(')', REVERSE(ms.counter_path))))
from [dbo].[MonitorStates] as ms
WHERE counter_id IS NOT NULL AND CHARINDEX('[',[counter_path])<>(0)
GO