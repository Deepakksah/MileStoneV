-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_video_wall_monitor_presets_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_video_wall_monitor_presets_transaction]
GO

CREATE PROCEDURE [delete_video_wall_monitor_presets_transaction]
	@monitor_id [GuidType] = NULL,
	@videowallpreset_id [GuidType] = NULL,
	@videowall_id [GuidType] = NULL
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END
	
	DECLARE @object_id VARCHAR(128)
	
	IF(@monitor_id IS NOT NULL)
	BEGIN
		INSERT [TempCustomPropertyDelete]
		SELECT @@SPID, [pkid]
			FROM [VideoWallMonitorPreset]
			WHERE [monitor_id] = @monitor_id
	END
	ELSE IF(@videowallpreset_id IS NOT NULL)
	BEGIN
		INSERT [TempCustomPropertyDelete]
		SELECT @@SPID, [pkid]
			FROM [VideoWallMonitorPreset]
			WHERE [videowallpreset_id] = @videowallpreset_id
	END
	ELSE
	BEGIN
		-- Delete custom properties for video wall monitor presets
		INSERT [TempCustomPropertyDelete]
		SELECT @@SPID, vwmp.[pkid]
		FROM [VideoWallMonitorPreset] vwmp
		INNER JOIN [VideoWallPreset] vwp ON vwp.[pkid] = vwmp.[videowallpreset_id]
		INNER JOIN [VideoWallMonitor] vwm ON vwm.[pkid] = vwmp.[monitor_id]
		INNER JOIN [VideoWall] vw ON vwp.[videowall_id] = vw.[pkid]
		WHERE vw.[pkid] = @videowall_id
	END
	
	-- Delete monitor presets
	EXEC [delete_custom_properties_transaction]

	IF(@monitor_id IS NOT NULL)
	BEGIN
		-- Delete monitor presets for monitor
		DELETE [VideoWallMonitorPreset]
		WHERE [monitor_id] = @monitor_id
	END
	ELSE IF(@videowallpreset_id IS NOT NULL)
	BEGIN
		-- Delete monitor presets for monitor
		DELETE [VideoWallMonitorPreset]
		WHERE [videowallpreset_id] = @videowallpreset_id
	END
	ELSE
	BEGIN
		DELETE [VideoWallMonitorPreset]
		WHERE [monitor_id] IN (SELECT [pkid] FROM [VideoWallMonitor] WHERE [videowall_id] = @videowall_id)
			OR [videowallpreset_id] IN (SELECT [pkid] FROM [VideoWallPreset] WHERE [videowall_id] = @videowall_id)
	END
END
GO