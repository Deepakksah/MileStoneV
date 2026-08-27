-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_video_wall_view]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_video_wall_view]
GO

CREATE PROCEDURE [update_video_wall_view]
	@pkid [GuidType],
	@name [NameType],
	@description NVARCHAR(MAX),
	@definition_xml XML,
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

		-- Make sure the view exists
		IF NOT EXISTS (
			SELECT [pkid]
			FROM [VideoWallView] WITH(UPDLOCK)
			WHERE [pkid] = @pkid)
		BEGIN
			;THROW 60215, @procedureName, 1;
		END

		BEGIN TRY
			UPDATE [VideoWallView]
			SET [name] = @name,
				[description] = @description,
				[last_modified] = @CurrentUtcTime,
				[definition_xml] = @definition_xml
			WHERE [pkid] = @pkid
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists on the video wall
				SET @ErrorArgs = @procedureName + N': %%Name=' + @name + N'%%'
				;THROW 60216, @ErrorArgs, 1;
			END
			;THROW 
		END CATCH

		-- Update custom properties
		EXEC [update_custom_properties_transaction]
			@object_id = @pkid,
			@custom_properties = @custom_properties

		COMMIT TRAN
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