-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_video_wall_view_layout]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_video_wall_view_layout]
GO

CREATE PROCEDURE [create_video_wall_view_layout]
	@pkid [GuidType],
	@parent_id [GuidType],
	@name [NameType],
	@description NVARCHAR(MAX),
	@definition_xml XML
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
	
		-- Verify that the server group exists
		SELECT @tmpId = [IDServerGroup]
		FROM [ServerGroup] WITH(UPDLOCK)
		WHERE [IDServerGroup] = @parent_id

		IF(@tmpId IS NULL)
		BEGIN
			;THROW 61012, @procedureName, 1;
		END

		BEGIN TRY
			INSERT [VideoWallViewLayout] (
				[pkid],
				[name],
				[description],
				[created_date],
				[last_modified],
				[definition_xml]
			)
			VALUES (
				@pkid,
				@name,
				@description,
				@CurrentUtcTime,
				@CurrentUtcTime,
				@definition_xml
			)
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists on the video wall
				;THROW 61034, @procedureName, 1;
			END
			;THROW 
		END CATCH

		-- Register object as a server group member
		EXEC [insert_server_group_member]
			@IDObject = @pkid,
			@IDServerGroup = @parent_id,
			@ObjectType = 'VIEW'

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