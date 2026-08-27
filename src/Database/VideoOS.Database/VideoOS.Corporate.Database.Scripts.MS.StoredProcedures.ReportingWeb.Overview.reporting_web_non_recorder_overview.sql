-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[reporting_web_non_recorder_overview]') AND type in (N'P', N'PC'))
DROP PROCEDURE [reporting_web_non_recorder_overview]
GO

CREATE PROCEDURE [reporting_web_non_recorder_overview]
	@server_id NVARCHAR(100)

AS
BEGIN

	DECLARE @ManagementServerHostName NVARCHAR(50)
	SELECT @ManagementServerHostName = Setting
	FROM SystemSettings
	WHERE Name = 'ManagementServerHostName'

	SELECT TOP 1 RegServer.IDServer AS IDServer
			, RegServer.DataCollectorId AS DataCollectorId
			, RegServer.ServerType AS ServerType
			, RegServer.ServerName AS ServerName 
			, RegServer.HostName AS HostName
			, CPU.CpuInstanceId AS CpuInstanceId
			, Memory.MemoryInstanceId AS MemoryInstanceId
		FROM (
			-- pick up all registered servers
			SELECT IDServer
					,dc.pkid AS DataCollectorId
					,ServerName = dc.[host_name]
					, ServerType = CASE rs.Type
									WHEN '08AB8F23-9AEF-4298-9CAA-F39259FE7CDA' THEN 'EventServer'
									WHEN '3D6F1153-92AD-43F1-B467-9482FFD291B2' THEN 'LogServer'
									WHEN '002B1B2F-0817-4592-8C62-2FE5107C160C' THEN 'LogService'
									WHEN 'E46B7BF9-03CE-44EB-BBDC-8BA16D0AAA80' THEN 'GatewayService'
									WHEN 'BA177CB7-5A9B-42B8-988D-CFBA91CA44E3' THEN 'LPRServer' 
									ELSE rs.Name
								END
					, dc.[host_name] AS HostName
			FROM RegisteredServers rs
			INNER JOIN DataCollector dc ON dc.version > 1 
			AND (rs.DataCollectorId = dc.pkid OR rs.Addresses LIKE '%' + dbo.EscapeWildcards(dc.[host_name]) + '%' ESCAPE '\') 

			UNION ALL

			SELECT MSServer.IDServer AS IDServer
					, MSServer.DataCollectorId AS DataCollectorId
					, ServerName = @ManagementServerHostName
					, ServerType = 'ManagementServer'
					, @ManagementServerHostName as HostName
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
			
			UNION ALL
			SELECT fs.IDFailoverServer AS IDServer
					, dc.pkid AS DataCollectorId
					, fs.Name AS ServerName
					, ServerType = 'FailoverServer'
					, fs.HostName AS HostName
			FROM [FailoverServers] fs
			INNER JOIN DataCollector dc ON dc.[host_name] = fs.[HostName]

			) as RegServer
			OUTER APPLY 
			(
				SELECT pkid as CpuInstanceId
				FROM PerformanceCounterInstance 
				WHERE data_collector_id = RegServer.DataCollectorId
				AND category_name = 'Processor' AND counter_name = '% Processor Time' AND instance_name = '_Total'
			) AS CPU
			OUTER APPLY 
			(
				SELECT pkid as MemoryInstanceId
				FROM PerformanceCounterInstance 
				WHERE data_collector_id = RegServer.DataCollectorId
				AND category_name = 'Memory' AND counter_name = 'Available MBytes'
			) AS Memory
			WHERE RegServer.IDServer = @server_id


END
GO

