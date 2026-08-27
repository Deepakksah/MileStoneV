-- This header is used by the dbupdate tool.
-- The TARGET poperty is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[insert_new_path_for_existing_property]') AND type in (N'P', N'PC'))
DROP PROCEDURE [insert_new_path_for_existing_property]
GO

CREATE PROCEDURE [insert_new_path_for_existing_property]
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

-- Add new property, Colorspace
-- The @VehicleColorFilterGUID is hardcoded, from 20.2.03.Add Metadata Search tables.sql 
DECLARE @VehicleColorFilterGUID [GuidType] =			'2D812F0D-ED3E-4553-B3FA-642BDC95D886';

INSERT INTO [dbo].[MetadataSearchProperties] (
	Id,
	PropertyName,
	FilterId 
)
VALUES 
	(
		NEWID(),
		'Colorspace',
		@VehicleColorFilterGUID
	);
GO

--Add new paths for existing and new property

EXEC insert_new_path_for_existing_property 'Height', '/tt:MetadataStream/tt:VideoAnalytics/tt:Frame/tt:Object', './tt:Appearance/tt:HumanBody/tt:BodyMetric/tt:Height[text()]'
EXEC insert_new_path_for_existing_property 'Gender', '/tt:MetadataStream/tt:VideoAnalytics/tt:Frame/tt:Object', './tt:Appearance/tt:HumanFace/tt:Gender[text()]'
EXEC insert_new_path_for_existing_property 'VehicleColor', '/tt:MetadataStream/tt:VideoAnalytics/tt:Frame/tt:Object', './tt:Appearance/tt:Color/tt:ColorCluster'
EXEC insert_new_path_for_existing_property 'Colorspace', '/tt:MetadataStream/tt:VideoAnalytics/tt:Frame/tt:Object', './../@Colorspace'
GO