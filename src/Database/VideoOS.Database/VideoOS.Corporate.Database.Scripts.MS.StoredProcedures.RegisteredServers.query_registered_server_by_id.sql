-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_registered_server_by_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_registered_server_by_id]
GO

CREATE PROCEDURE [query_registered_server_by_id]
	@ServiceId [UNIQUEIDENTIFIER]
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
	WHERE [IDServer] = @ServiceId 
END
GO