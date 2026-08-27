-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[reporting_web_max_physical_memory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [reporting_web_max_physical_memory]
GO

CREATE PROCEDURE [reporting_web_max_physical_memory]
	@data_collector_id INT
AS
BEGIN
	SELECT ROUND(MachineProperties.value / 1024/1024, 0) as MemoryMaxValue
	FROM MachineProperties
	WHERE data_collector_id = @data_collector_id
	AND name = 'ullTotalPhys'
END
GO