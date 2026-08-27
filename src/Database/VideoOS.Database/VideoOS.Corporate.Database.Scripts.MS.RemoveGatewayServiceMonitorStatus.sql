-- This header is used by the dbupdate tool.
-- The TARGET poperty is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @GatewayServiceTypeId [GuidType]	=	'E46B7BF9-03CE-44EB-BBDC-8BA16D0AAA80';

IF EXISTS (SELECT * FROM [MonitorStates] WHERE [counter_id] = @GatewayServiceTypeId)
BEGIN
	DELETE FROM [MonitorStates] WHERE [counter_id] = @GatewayServiceTypeId;
END

IF EXISTS (SELECT * FROM [PerformanceCounterInstance] WHERE Instance_name = @GatewayServiceTypeId)
BEGIN
	DELETE FROM [PerformanceCounterValue] WHERE [Instance_id] IN (SELECT [pkid] FROM [PerformanceCounterInstance] WHERE [Instance_name] = @GatewayServiceTypeId);
END

IF EXISTS (SELECT * FROM [PerformanceCounterInstance] WHERE [Instance_name] = @GatewayServiceTypeId)
BEGIN
	DELETE FROM [PerformanceCounterInstance] WHERE [Instance_name] = @GatewayServiceTypeId;
END
GO