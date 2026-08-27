-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_dc_machines]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_dc_machines]
GO

CREATE PROCEDURE [query_dc_machines]
AS
BEGIN
	SELECT 
		addSvr.pkid as CollectorId
		, addSvr.[host_name] as HostName
		, addSvr.Name as ServiceName
		, addSvr.ServiceType as ServiceType

	FROM
	(
		SELECT [host_name], pkid, Name, ServiceType
		FROM DataCollector dc
		OUTER APPLY 
		(
			SELECT st.Name as Name, st.service_type as ServiceType
			FROM DataCollectorInstalledServices dis2
			INNER JOIN ServerType st ON dis2.service_name = st.Name
			WHERE data_collector_id = dc.pkid
		
		) dis
	) AS addSvr
END
GO