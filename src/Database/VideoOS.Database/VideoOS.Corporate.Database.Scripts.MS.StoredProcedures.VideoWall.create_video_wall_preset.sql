-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_video_wall_preset]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_video_wall_preset]
GO

CREATE PROCEDURE [create_video_wall_preset]
	@pkid [GuidType],
	@videowall_id [GuidType],
	@name [NameType],
	@description NVARCHAR(MAX),
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
	
		DECLARE @tmpId [GuidType]
		DECLARE @videoWallName [NameType]
	
		-- Verify that the video wall exists
		SELECT @tmpId = [pkid], @videoWallName = [name]
		FROM [VideoWall] WITH(UPDLOCK)
		WHERE [pkid] = @videowall_id

		IF(@tmpId IS NULL)
		BEGIN
			;THROW 60209, @procedureName, 1;
		END

		BEGIN TRY
			INSERT [VideoWallPreset] (
				[pkid],
				[videowall_id],
				[name],
				[description],
				[created_date],
				[last_modified]
			)
			VALUES (
				@pkid,
				@videowall_id,
				@name,
				@description,
				@CurrentUtcTime,
				@CurrentUtcTime
			)
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists on the video wall
				SET @ErrorArgs = @procedureName + N': %%name=' + @name + N'%%; %%videoWallName=' + @videoWallName + N'%%'
				;THROW 60204, @ErrorArgs, 1;
			END
			;THROW
		END CATCH

		-- Create MonitorPresets for all Monitors
		INSERT [VideoWallMonitorPreset] (
			[pkid],
			[monitor_id],
			[videowallpreset_id],
			[last_modified]
		)
		SELECT newid(), [pkid], @pkid, @CurrentUtcTime
		FROM [VideoWallMonitor]
		WHERE videowall_id = @videowall_id
	
		-- Update custom properties
		EXEC [update_custom_properties_transaction]
			@object_id = @pkid,
			@custom_properties = @custom_properties

		COMMIT TRAN

		EXEC create_pending_log @session_id, 10201, @pkid, @name, @videowall_id, @videoWallName
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