-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_video_wall_monitor_state]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_video_wall_monitor_state]
GO

CREATE PROCEDURE [update_video_wall_monitor_state]
	@pkid [GuidType],
	@monitor_state VARCHAR(2000)
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	-- Make sure the video wall monitor exists
	IF NOT EXISTS (
		SELECT [pkid]
		FROM [VideoWallMonitor] WITH(UPDLOCK)
		WHERE [pkid] = @pkid)
	BEGIN
		;THROW 60220, @procedureName, 1;
	END

	BEGIN TRY
		UPDATE [VideoWallMonitor]
		SET [monitor_state] = @monitor_state
		WHERE [pkid] = @pkid
	END TRY
	BEGIN CATCH
		;THROW 60000, @procedureName, 1;
	END CATCH
END
GO