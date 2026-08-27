-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_video_wall]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_video_wall]
GO

CREATE PROCEDURE [update_video_wall]
	@pkid [GuidType],
	@name [NameType],
	@description NVARCHAR(MAX),
	@width FLOAT,
	@height FLOAT,
	@statustext BIT,
	@titlebar VARCHAR(50),
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

		DECLARE @videoWall_id [GuidType]
		DECLARE @currentVideoWallName [NameType]

		SELECT @videoWall_id = [pkid], @currentVideoWallName = [name]
		FROM [VideoWall] WITH(UPDLOCK)
		WHERE [pkid] = @pkid

		-- Make sure the video wall exists
		IF(@videoWall_id IS NULL)
		BEGIN
			;THROW 60201, @procedureName, 1;
		END

		BEGIN TRY
			UPDATE [VideoWall]
			SET [name] = @name,
				[description] = @description,
				[width] = @width,
				[height] = @height,
				[statustext] = @statustext,
				[titlebar] = @titlebar,
				[last_modified] = @CurrentUtcTime
			WHERE [pkid] = @pkid
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists on the server
				SET @ErrorArgs = @procedureName + N': %%name=' + @name + N'%%'
				;THROW 60202, @ErrorArgs, 1;
			END
			;THROW 
		END CATCH
	
		-- Update custom properties
		EXEC [update_custom_properties_transaction]
			@object_id = @pkid,
			@custom_properties = @custom_properties

		COMMIT TRAN

		IF(@currentVideoWallName <> @name)
		BEGIN
			EXEC create_pending_log @session_id, 10003, @pkid, @name, @currentVideoWallName
		END
		ELSE
		BEGIN
			EXEC create_pending_log @session_id, 10119, @pkid, @name, @currentVideoWallName
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