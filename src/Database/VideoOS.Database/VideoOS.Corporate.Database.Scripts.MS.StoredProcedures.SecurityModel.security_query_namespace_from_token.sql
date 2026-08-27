-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_namespace_from_token]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_namespace_from_token]
GO

CREATE PROCEDURE [security_query_namespace_from_token]
	@object_id NVARCHAR(400)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @namespace_id UNIQUEIDENTIFIER

	-- If object is a security object, just get the namespace
	SELECT @namespace_id = [namespace_id]
	FROM [SecurityObject]
	WHERE [object_id] = @object_id

	IF(@namespace_id IS NOT NULL)
	BEGIN
		SELECT @namespace_id
		RETURN 0
	END

	-- Object might be a VideoWall, VideoWallPreset, VideoWallMonitor or VideoWallMonitorPreset
	DECLARE @videowall_id [GuidType]

	DECLARE @pkids TABLE(value [GuidType]); 
	
	INSERT INTO @pkids (value)
	SELECT TOP 1 vw.pkid 
	FROM VideoWall vw
	LEFT JOIN VideoWallPreset vwp ON vwp.videowall_id = vw.pkid
	LEFT JOIN VideoWallMonitor vwm ON vwm.videowall_id = vw.pkid
	LEFT JOIN VideoWallMonitorPreset mp WITH (INDEX(IX_VideoWallMonitorPreset_MonitorId)) ON mp.monitor_id = vwm.pkid
	WHERE vw.pkid = @object_id 
	UNION ALL
	SELECT top 1 vw.pkid
	FROM VideoWall vw
	LEFT JOIN VideoWallPreset vwp ON vwp.videowall_id = vw.pkid
	LEFT JOIN VideoWallMonitor vwm ON vwm.videowall_id = vw.pkid
	LEFT JOIN VideoWallMonitorPreset mp WITH (INDEX(IX_VideoWallMonitorPreset_MonitorId)) ON mp.monitor_id = vwm.pkid
	WHERE vwm.pkid = @object_id 
	UNION ALL
	SELECT top 1 vw.pkid
	FROM VideoWall vw
	LEFT JOIN VideoWallPreset vwp ON vwp.videowall_id = vw.pkid
	LEFT JOIN VideoWallMonitor vwm ON vwm.videowall_id = vw.pkid
	LEFT JOIN VideoWallMonitorPreset mp WITH (INDEX(IX_VideoWallMonitorPreset_MonitorId)) ON mp.monitor_id = vwm.pkid
	WHERE vwp.pkid = @object_id 
	UNION ALL
	SELECT top 1 vw.pkid
	FROM VideoWall vw
	LEFT JOIN VideoWallPreset vwp ON vwp.videowall_id = vw.pkid
	LEFT JOIN VideoWallMonitor vwm ON vwm.videowall_id = vw.pkid
	LEFT JOIN VideoWallMonitorPreset mp WITH (INDEX(IX_VideoWallMonitorPreset_MonitorId)) ON mp.monitor_id = vwm.pkid
	WHERE mp.pkid = @object_id

	SELECT TOP 1 @videowall_id = value FROM @pkids

	IF(@videowall_id IS NOT NULL)
	BEGIN
		SELECT @namespace_id = [namespace_id]
		FROM [SecurityObject]
		WHERE [object_id] = @videowall_id

		SELECT @namespace_id
	END
END
GO