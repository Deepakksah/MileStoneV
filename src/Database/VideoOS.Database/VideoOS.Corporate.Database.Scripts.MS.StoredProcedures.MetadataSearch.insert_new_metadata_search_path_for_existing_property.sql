-- This header is used by the dbupdate tool.
-- The TARGET poperty is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[insert_new_metadata_search_path_for_existing_property]') AND type in (N'P', N'PC'))
DROP PROCEDURE [insert_new_metadata_search_path_for_existing_property]
GO

CREATE PROCEDURE [insert_new_metadata_search_path_for_existing_property]
	@PropertyName nvarchar(256),
	@RootPath nvarchar(1024),
	@Path nvarchar(1024)
AS

	DECLARE @RootPathId [GuidType],
			@PropertyId [GuidType],
			@ExistingCount int;

	SELECT @RootPathId = [Id] FROM [dbo].[MetadataSearchObjectDefinitions] WHERE [ObjectRoot] = @RootPath;
	SELECT @PropertyId = [Id] FROM [dbo].[MetadataSearchProperties] WHERE [PropertyName] = @PropertyName;
	
	SELECT @ExistingCount = COUNT([Id]) from [dbo].[MetadataSearchPaths]
							WHERE @Path = [Path] AND @RootPathId = [ObjectDefinitionId] AND @PropertyId = [PropertyId];

	-- Don't insert duplicates
	IF @ExistingCount > 0
	BEGIN
		RETURN
	END

	INSERT INTO [dbo].[MetadataSearchPaths]
			   ([Id]
			   ,[Path]
			   ,[ObjectDefinitionId]
			   ,[PropertyId])
		 VALUES
			   (NEWID()
			   ,@Path
			   ,@RootPathId
			   ,@PropertyId)
GO
