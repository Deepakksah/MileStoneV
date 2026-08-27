-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_video_wall_monitor]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_video_wall_monitor]
GO

CREATE PROCEDURE [update_video_wall_monitor]
	@pkid [GuidType],
	@name [NameType],
	@description NVARCHAR(MAX),
	@aspect_ratio VARCHAR(50),
	@insertion_method VARCHAR(50),
	@no_layout_method VARCHAR(50),
	@empty_viewitems_method VARCHAR(50),
	@monitor_size REAL,
	@location_x FLOAT,
	@location_y FLOAT,
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

		-- Extract info for logging
		DECLARE @videoWallName [NameType]
		DECLARE @videowall_Id[GuidType]
		DECLARE @monitor_id [GuidType]
		DECLARE @currentMonitorName [NameType]
		DECLARE @currentDescription NVARCHAR(MAX)
		DECLARE @currentAspect_ratio VARCHAR(50)
		DECLARE @currentInsertion_method VARCHAR(50)
		DECLARE @currentNo_layout_method VARCHAR(50)
		DECLARE @currentEmpty_viewitems_method VARCHAR(50)
		DECLARE @currentMonitor_size REAL
		DECLARE @currentLocation_x FLOAT
		DECLARE @currentLocation_y FLOAT

		SELECT @videowall_Id = vw.[pkid], @videoWallName = vw.[name]
		FROM [VideoWall] vw WITH(UPDLOCK)
		INNER JOIN [VideoWallMonitor] vwm ON vwm.[videowall_id] = vw.[pkid]
		WHERE vwm.[pkid] = @pkid
	
		SELECT @monitor_id = [pkid], 
			@currentMonitorName = [name],
			@currentDescription = [description],
			@currentAspect_ratio = [aspect_ratio],
			@currentInsertion_method = [insertion_method],
			@currentNo_layout_method = [no_layout_method],
			@currentEmpty_viewitems_method = [empty_viewitems_method],
			@currentMonitor_size = [monitor_size],
			@currentLocation_x = [location_x],
			@currentLocation_y = [location_y]
		FROM [VideoWallMonitor] WITH(UPDLOCK)
		WHERE [pkid] = @pkid

		-- Does the monitor exist?
		IF(@monitor_id IS NULL)
		BEGIN
			;THROW 60211, @procedureName, 1;
		END

		BEGIN TRY
			UPDATE [VideoWallMonitor]
			SET [name] = @name,
				[description] = @description,
				[aspect_ratio] = @aspect_ratio,
				[insertion_method] = @insertion_method,
				[no_layout_method] = @no_layout_method,
				[empty_viewitems_method] = @empty_viewitems_method,
				[monitor_size] = @monitor_size,
				[location_x] = @location_x,
				[location_y] = @location_y,
				[last_modified] = @CurrentUtcTime
			WHERE [pkid] = @pkid
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists on the video wall
				SET @ErrorArgs = @procedureName + N': %%name=' + @name + N'%%; %%videoWallName=' + @videoWallName + N'%%'
				;THROW 60212, @ErrorArgs, 1;
			END
			;THROW
		END CATCH

		-- Update custom properties
		EXEC [update_custom_properties_transaction]
			@object_id = @pkid,
			@custom_properties = @custom_properties

		COMMIT TRAN
		
		-- If name has changed log a "renamed" log
		IF(@currentMonitorName <> @name)
		BEGIN
			EXEC create_pending_log @session_id, 10104, @pkid, @name, @videowall_id, @videoWallName, @currentMonitorName
		END
	
		-- If any thing else than the name has changed log an "edited" log
		IF(@currentDescription <> @description OR
			@currentAspect_ratio <> @aspect_ratio OR
			@currentInsertion_method <> @insertion_method OR
			@currentNo_layout_method <> @no_layout_method OR
			@currentEmpty_viewitems_method <> @empty_viewitems_method OR
			@currentMonitor_size <> @monitor_size OR
			@currentLocation_x <> @location_x OR
			@currentLocation_y <> @location_y)
		BEGIN
			EXEC create_pending_log @session_id, 10103, @pkid, @name, @videowall_id, @videoWallName
		END
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