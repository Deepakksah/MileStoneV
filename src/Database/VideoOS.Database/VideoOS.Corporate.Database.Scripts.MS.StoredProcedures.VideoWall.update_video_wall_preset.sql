-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_video_wall_preset]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_video_wall_preset]
GO

CREATE PROCEDURE [update_video_wall_preset]
	@pkid [GuidType],
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

		-- Extract info for logging
		DECLARE @videoWallName [NameType]
		DECLARE @videowall_Id[GuidType]
		DECLARE @preset_id [GuidType]
		DECLARE @currentPresetName [NameType]

		SELECT @videowall_Id = vw.[pkid], @videoWallName = vw.[name]
		FROM [VideoWall] vw WITH(UPDLOCK)
		INNER JOIN [VideoWallpreset] vwm ON vwm.[videowall_id] = vw.[pkid]
		WHERE vwm.[pkid] = @pkid
	
		SELECT @preset_id = [pkid], @currentPresetName = [name]
		FROM [VideoWallpreset] WITH(UPDLOCK)
		WHERE [pkid] = @pkid

		-- Does the preset exist?
		IF(@preset_id IS NULL)
		BEGIN
			;THROW 60205, @procedureName, 1;
		END

		BEGIN TRY
			UPDATE [VideoWallPreset]
			SET [name] = @name,
				[description] = @description,
				[last_modified] = @CurrentUtcTime
			WHERE [pkid] = @pkid
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists on the video wall
				SET @ErrorArgs = @procedureName + N': %%name=' + @name + N'%%; %%videoWallName=' + @videoWallName + N'%%'
				;THROW 60206, @ErrorArgs, 1;
			END
			;THROW
		END CATCH

		-- Update custom properties
		EXEC [update_custom_properties_transaction]
			@object_id = @pkid,
			@custom_properties = @custom_properties

		COMMIT TRAN

		IF(@currentPresetName <> @name)
		BEGIN
			EXEC create_pending_log @session_id, 10204, @pkid, @name, @videowall_id, @videoWallName, @currentPresetName
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