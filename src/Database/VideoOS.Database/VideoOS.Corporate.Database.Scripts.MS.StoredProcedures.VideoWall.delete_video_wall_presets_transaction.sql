-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_video_wall_presets_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_video_wall_presets_transaction]
GO

CREATE PROCEDURE [delete_video_wall_presets_transaction]
	@videowall_id [GuidType],
	@videowallName [NameType],
	@session_id [GuidType]
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
	
	-- Delete monitor presets
	EXEC [delete_video_wall_monitor_presets_transaction]
		@videowall_id = @videowall_id

	-- Delete custom properties
	INSERT [TempCustomPropertyDelete]
	SELECT @@SPID, [pkid]
	FROM [VideoWallPreset]
	WHERE [videowall_id] = @videowall_id
	
	EXEC [delete_custom_properties_transaction]

	-- Extract info for logging
	DECLARE @presetName [NameType]
	DECLARE @preset_id [GuidType]
	
	DECLARE preset_cursor CURSOR FOR 
	SELECT [pkid], [name] 
	FROM [VideoWallPreset]
	WHERE [videowall_id] = @videowall_id
	
	OPEN preset_cursor
	BEGIN TRY
		FETCH NEXT FROM preset_cursor INTO @preset_id, @presetName

		WHILE @@FETCH_STATUS = 0
		BEGIN
			EXEC create_pending_log @session_id, 10203, @preset_id, @presetName, @videowall_id, @videoWallName
			FETCH NEXT FROM preset_cursor INTO @preset_id, @presetName
		END
	END TRY
	BEGIN CATCH
		CLOSE preset_cursor
		DEALLOCATE preset_cursor
		;THROW
	END CATCH

	CLOSE preset_cursor
	DEALLOCATE preset_cursor
	
	-- Delete the video wall preset
	DELETE FROM [VideoWallPreset]
	WHERE [videowall_id] = @videowall_id
END
GO