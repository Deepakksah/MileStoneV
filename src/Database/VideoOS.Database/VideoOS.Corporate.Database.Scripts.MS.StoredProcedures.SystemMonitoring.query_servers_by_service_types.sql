-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_servers_by_service_types]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_servers_by_service_types]
GO

CREATE PROCEDURE [query_servers_by_service_types]
	@ServiceTypes tvp_StringTableGuidType READONLY,
	@Ids tvp_StringTableGuidType READONLY
AS
BEGIN
	DECLARE @NoServiceTypes BIT
	SET @NoServiceTypes = 0

	IF ((SELECT COUNT(ID) FROM @ServiceTypes) = 0)
	BEGIN
		SET @NoServiceTypes = 1
	END

	DECLARE @NoIds BIT
	SET @NoIds = 0

	IF ((SELECT COUNT(ID) FROM @Ids) = 0)
	BEGIN
		SET @NoIds = 1
	END

	-- pick up management servers 
	DECLARE @ManagementServerHostName NVARCHAR(50)
	SELECT @ManagementServerHostName = Setting
	FROM SystemSettings
	WHERE Name = 'ManagementServerHostName'
	
	SELECT IDServer, DataCollectorId, ServerType, ServiceName, ServerName 
	FROM(
		SELECT MSServer.IDServer AS IDServer, MSServer.DataCollectorId AS DataCollectorId, ServerType = '2D275C3A-50BC-4435-B0AD-ED3381FEDC1D', ServiceName = 'ManagementServer', @ManagementServerHostName as ServerName
		FROM (SELECT 
				IDServer =
				(
					SELECT  IDSite
					FROM Sites 
					WHERE IDSiteType = 1
				)
				,pkid AS DataCollectorId
			FROM DataCollector dc 
			INNER JOIN DataCollectorInstalledServices dis ON dc.pkid = dis.data_collector_id 
			WHERE service_name = 'ManagementServer' 
				AND dc.version > 1
				AND host_name = @ManagementServerHostName COLLATE SQL_Latin1_General_CP1_CI_AS ) as MSServer 
		WHERE @NoServiceTypes = 1 OR EXISTS(SELECT * FROM @ServiceTypes as st WHERE st.[ID] = '2D275C3A-50BC-4435-B0AD-ED3381FEDC1D' COLLATE SQL_Latin1_General_CP1_CI_AS)

		UNION ALL			
		SELECT RSServer.IDServer as IDServer, RSServer.DataCollectorId AS DataCollectorId, ServerType = '2B2D3601-5857-44A3-880F-BE7957B5B27E', ServiceName = 'Recorder', RSServer.Name as ServerName
		FROM (
			SELECT 
				rs.[IDRecorder] as IDServer,
				dc.[pkid] AS DataCollectorId,
				rs.Name
			FROM 
				[Recorders] rs
				INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recorder_id] = rs.[IDRecorder]
				INNER JOIN [RecordingServerService] rss ON rss.[pkid] = rssr.[recording_server_service_id] AND rssr.[type] = 0
				INNER JOIN [DataCollector] dc ON dc.[host_name] = rss.[host_name]
				INNER JOIN [DataCollectorInstalledServices] dis ON dc.[pkid] = dis.[data_collector_id]
			WHERE
				dis.[service_name] = 'Recorder' 
				AND rs.[Enabled] = 1 
				AND dc.version > 1 ) AS RSServer 
		WHERE @NoServiceTypes = 1 OR EXISTS(SELECT * FROM @ServiceTypes as st WHERE st.[ID] = '2B2D3601-5857-44A3-880F-BE7957B5B27E' COLLATE SQL_Latin1_General_CP1_CI_AS ) 
				
		UNION ALL 
		SELECT fs.IDFailoverServer AS IDServer, dc.pkid as DataCollectorId, ServerType = '3C944CF2-668F-4956-9EBE-A1C3F6BD6CD4', ServiceName = 'FailoverServer', fs.Name as ServerName
			FROM FailoverServers fs
			INNER JOIN DataCollector dc ON dc.[host_name] = fs.HostName
			INNER JOIN DataCollectorInstalledServices dis ON dc.pkid = dis.data_collector_id 
			WHERE service_name = 'FailoverServer' AND 
			dc.version > 1 AND 
			(@NoServiceTypes = 1 OR EXISTS(SELECT * FROM @ServiceTypes as st WHERE st.[ID] = '3C944CF2-668F-4956-9EBE-A1C3F6BD6CD4' COLLATE SQL_Latin1_General_CP1_CI_AS )) 

		UNION ALL 
		SELECT RegServer.IDServer AS IDServer
				, RegServer.DataCollectorId AS DataCollectorId
				, RegServer.Type AS ServerType
				, RegServer.ServiceName AS ServiceName 
				, RegServer.Name as ServerName
			FROM (
				-- pick up all registered servers
				SELECT IDServer
						,rs.DataCollectorId AS DataCollectorId
						,rs.Type AS Type
						,ServiceName = CASE rs.Type
										WHEN '08AB8F23-9AEF-4298-9CAA-F39259FE7CDA' THEN 'EventServer'
										WHEN '7510BBAE-DBA5-4C2E-B037-74A0222484C6' THEN 'MobileServer'
										WHEN '002B1B2F-0817-4592-8C62-2FE5107C160C' THEN 'LogService'
										WHEN 'E46B7BF9-03CE-44EB-BBDC-8BA16D0AAA80' THEN 'GatewayService'
										WHEN 'BA177CB7-5A9B-42B8-988D-CFBA91CA44E3' THEN 'LPRServer' 
									END
						, rs.Name
				FROM RegisteredServers rs
				WHERE rs.DataCollectorId = 0 OR (SELECT dc.version FROM DataCollector dc WHERE dc.pkid = rs.DataCollectorId) > 1
				) as RegServer
			WHERE @NoServiceTypes = 1 OR EXISTS(SELECT * FROM @ServiceTypes as st WHERE st.ID = RegServer.Type COLLATE SQL_Latin1_General_CP1_CI_AS )

	) allservers
	WHERE @NoIds = 1 OR EXISTS(SELECT i.[ID] FROM @Ids i WHERE i.[ID] = allservers.IDServer COLLATE SQL_Latin1_General_CP1_CI_AS)					
END
GO