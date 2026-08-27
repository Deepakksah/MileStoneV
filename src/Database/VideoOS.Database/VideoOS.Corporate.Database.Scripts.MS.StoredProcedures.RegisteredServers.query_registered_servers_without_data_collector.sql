-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_registered_servers_without_data_collector]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_registered_servers_without_data_collector]
GO

CREATE PROCEDURE [query_registered_servers_without_data_collector]
	@ServiceTypes tvp_StringTableGuidType READONLY
AS
BEGIN
	SELECT [IDServer]
		,[Type]
		,[Name]
		,[Description]
		,[Addresses]
		,[Trusted]
		,[CustomData]
		,[Enabled] 
		,[DataCollectorId]
		,[EndpointDefinitions]
	FROM [RegisteredServers]
	INNER JOIN @ServiceTypes ServiceTypes ON [RegisteredServers].[Type]=ServiceTypes.ID
	AND Enabled = 1
	AND [DataCollectorId] = 0
END
GO