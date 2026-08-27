-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_video_wall_monitor]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_video_wall_monitor]
GO

CREATE PROCEDURE [create_video_wall_monitor]
	@pkid [GuidType],
	@videowall_id [GuidType],
	@name [NameType],
	@description NVARCHAR(MAX),
	@aspect_ratio VARCHAR(50),
	@insertion_method VARCHAR(50),
	@no_layout_method VARCHAR(50),
	@empty_viewitems_method VARCHAR(50),
	@monitor_size FLOAT,
	@location_x FLOAT,
	@location_y FLOAT,
	@default_view_definition_xml XML,
	@default_view_item_definition_xml XML,
	@session_id [GuidType],
	@custom_properties XML = NULL
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()
	
		DECLARE @class_id VARCHAR(128)
	
		DECLARE @tmpId [GuidType]
		DECLARE @videoWallName [NameType]
	
		-- Verify that the video wall exists
		SELECT @tmpId = [pkid], @videoWallName = [name]
		FROM [VideoWall] WITH(UPDLOCK)
		WHERE [pkid] = @videowall_id

		IF(@tmpId IS NULL)
		BEGIN
			;THROW 60208, @procedureName, 1;
		END
	
		-- Create default 1x1 View
		DECLARE @view_id [GuidType]
		SELECT @view_id = newid()
	
		INSERT INTO [VideoWallView] ([pkid], [name], [created_date], [last_modified], [definition_xml])
		VALUES(@view_id, @name, @CurrentUtcTime, @CurrentUtcTime, @default_view_definition_xml)
	
		BEGIN TRY
			INSERT [VideoWallMonitor] (
				[pkid],
				[videowall_id],
				[view_id],
				[name],
				[description],
				[aspect_ratio],
				[insertion_method],
				[no_layout_method],
				[empty_viewitems_method],
				[monitor_size],
				[location_x],
				[location_y],
				[created_date],
				[last_modified]
			)
			VALUES (
				@pkid,
				@videowall_id,
				@view_id,
				@name,
				@description,
				@aspect_ratio,
				@insertion_method,
				@no_layout_method,
				@empty_viewitems_method,
				@monitor_size,
				@location_x,
				@location_y,
				@CurrentUtcTime,
				@CurrentUtcTime
			)
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists on the video wall
				SET @ErrorArgs = @procedureName + N': %%name=' + @name + N'%%; %%videoWallName=' + @videoWallName + N'%%'
				;THROW 60210, @ErrorArgs, 1;
			END
			;THROW
		END CATCH

		-- Create MonitorPresets for all Video Wall Presets
		INSERT [VideoWallMonitorPreset] (
			[pkid],
			[monitor_id],
			[videowallpreset_id],
			[last_modified],
			[definition_xml]
		)
		SELECT newid(), @pkid, [pkid], @CurrentUtcTime, NULL
		FROM [VideoWallPreset]
		WHERE videowall_id = @videowall_id

		--Create default View Item for 1x1 view
		INSERT INTO [VideoWallViewItem] 
		(
			[pkid],
			[view_id],
			[position],
			[definition_xml]
		)
		VALUES (newid(), @view_id, 0, @default_view_item_definition_xml)
	
		-- Update custom properties
		EXEC [update_custom_properties_transaction]
			@object_id = @pkid,
			@custom_properties = @custom_properties
	
		COMMIT TRAN

		EXEC create_pending_log @session_id, 10101, @pkid, @name, @videowall_id, @videoWallName
	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		DECLARE @shouldReThrowError BIT = dbo.[ShouldReThrowError](ERROR_NUMBER(), ERROR_SEVERITY())
		IF(@shouldReThrowError = 1)
			THROW;
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE())
		;THROW 60000, @ErrorArgs, 1;
	END CATCH;
END
GO