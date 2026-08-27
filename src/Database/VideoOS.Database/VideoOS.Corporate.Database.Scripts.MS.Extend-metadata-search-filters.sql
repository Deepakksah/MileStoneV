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

-- Paths rework to be namespace agnostic

-- Delete the existing stored procedures that will not be used anymore
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_metadata_search_namespace_by_id]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [query_metadata_search_namespace_by_id]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_metadata_search_namespaces]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [query_metadata_search_namespaces]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_metadata_search_namespaces_by_ids]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [query_metadata_search_namespaces_by_ids]
GO

-- Delete the existing namespace table, as we do not need it anymore
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MetadataSearchPathNamespaces]') AND type in (N'U'))
	DROP TABLE [dbo].[MetadataSearchPathNamespaces]
GO

-- Remove paths that will become duplicate after rework
DELETE FROM [dbo].[MetadataSearchPaths]
      WHERE Path IN (
	  './ns0:LicensePlateInfo/ns0:PlateNumber[text()]', 
	  './ns0:Extension/Properties/Property[@name=''LicensePlate'']',
	  './ns0:Class/tt:ClassCandidate/tt:Type[text()]',
	  './ns0:Extension/Properties/Property[@name=''Class'']'
	  )
GO

-- Remove object definitions that will become duplicate after rework
DELETE FROM [dbo].[MetadataSearchObjectDefinitions]
      WHERE ObjectRoot = '/ns0:MetadataStream/ns0:VideoAnalytics/ns0:Frame/ns0:Object'
GO

-- Updating existing object definitions 
UPDATE [dbo].[MetadataSearchObjectDefinitions] SET ObjectRoot = '/*[local-name()=''MetadataStream'']/*[local-name()=''Extension'']/NavigationalData' WHERE ObjectRoot = '/tt:MetadataStream/tt:Extension/NavigationalData'
UPDATE [dbo].[MetadataSearchObjectDefinitions] SET ObjectRoot = '/*[local-name()=''MetadataStream'']/*[local-name()=''VideoAnalytics'']/*[local-name()=''Frame'']/*[local-name()=''Object'']' WHERE ObjectRoot = '/tt:MetadataStream/tt:VideoAnalytics/tt:Frame/tt:Object'
UPDATE [dbo].[MetadataSearchObjectDefinitions] SET ObjectRoot = '/*[local-name()=''MetadataStream'']/*[local-name()=''VideoAnalytics'']/*[local-name()=''Frame'']/*[local-name()=''Extension'']' WHERE ObjectRoot = '/tt:MetadataStream/tt:VideoAnalytics/tt:Frame/tt:Extension'
UPDATE [dbo].[MetadataSearchObjectDefinitions] SET ObjectRoot = '/*[local-name()=''MetadataStream'']/*[local-name()=''Event'']/*[local-name()=''NotificationMessage'']/*[local-name()=''Topic''][text() = ''tnsaxis:CameraApplicationPlatform/ALPV.AllPlates'']/../*[local-name()=''Message'']/*[local-name()=''Message'']/*[local-name()=''Data'']' WHERE ObjectRoot = '/tt:MetadataStream/tt:Event/wsnt:NotificationMessage/wsnt:Topic[text() = ''tnsaxis:CameraApplicationPlatform/ALPV.AllPlates'']/../wsnt:Message/tt:Message/tt:Data'

-- Updating existing paths
UPDATE [dbo].[MetadataSearchPaths] SET Path = './*[local-name()=''Appearance'']/*[local-name()=''HumanFace'']/*[local-name()=''Gender''][text()]' WHERE Path = './tt:Appearance/tt:HumanFace/tt:Gender[text()]'
UPDATE [dbo].[MetadataSearchPaths] SET Path = './*[local-name()=''Appearance'']/*[local-name()=''Class'']/*[local-name()=''ClassCandidate'']/*[local-name()=''Type''][text()!=''Face'']' WHERE Path = './tt:Appearance/tt:Class/tt:ClassCandidate/tt:Type[text()!=''Face'']'
UPDATE [dbo].[MetadataSearchPaths] SET Path = './*[local-name()=''Extension'']/Properties/Property[@name=''LicensePlate'']' WHERE Path = './tt:Extension/Properties/Property[@name=''LicensePlate'']'
UPDATE [dbo].[MetadataSearchPaths] SET Path = './*[local-name()=''Appearance'']/*[local-name()=''Color'']/*[local-name()=''ColorCluster'']' WHERE Path = './tt:Appearance/tt:Color/tt:ColorCluster'
UPDATE [dbo].[MetadataSearchPaths] SET Path = './*[local-name()=''Appearance'']/*[local-name()=''VehicleInfo'']/*[local-name()=''Type''][text()]' WHERE Path = './tt:Appearance/tt:VehicleInfo/tt:Type[text()]'
UPDATE [dbo].[MetadataSearchPaths] SET Path = './*[local-name()=''Extension'']/Properties/Property[@name=''Color'']' WHERE Path = './tt:Extension/Properties/Property[@name=''Color'']'
UPDATE [dbo].[MetadataSearchPaths] SET Path = './*[local-name()=''Appearance'']/*[local-name()=''LicensePlateInfo'']/*[local-name()=''PlateNumber''][text()]' WHERE Path = './tt:Appearance/tt:LicensePlateInfo/tt:PlateNumber[text()]'
UPDATE [dbo].[MetadataSearchPaths] SET Path = './*[local-name()=''SimpleItem''][@Name=''text'']/@Value' WHERE Path = './tt:SimpleItem[@Name=''text'']/@Value'
UPDATE [dbo].[MetadataSearchPaths] SET Path = './*[local-name()=''Appearance'']/*[local-name()=''GeoLocation'']/@lon' WHERE Path = './tt:Appearance/tt:GeoLocation/@lon'
UPDATE [dbo].[MetadataSearchPaths] SET Path = './*[local-name()=''Appearance'']/*[local-name()=''Class'']/*[local-name()=''ClassCandidate'']/*[local-name()=''Type''][text()=''Face'']' WHERE Path = './tt:Appearance/tt:Class/tt:ClassCandidate/tt:Type[text()=''Face'']'
UPDATE [dbo].[MetadataSearchPaths] SET Path = './*[local-name()=''Extension'']/Properties/Property[@name=''Gender'']' WHERE Path = './tt:Extension/Properties/Property[@name=''Gender'']'
UPDATE [dbo].[MetadataSearchPaths] SET Path = './*[local-name()=''Extension'']/Properties/Property[@name=''Age'']' WHERE Path = './tt:Extension/Properties/Property[@name=''Age'']'
UPDATE [dbo].[MetadataSearchPaths] SET Path = './*[local-name()=''Behaviour'']/*[local-name()=''Speed''][text()]' WHERE Path = './tt:Behaviour/tt:Speed[text()]'
UPDATE [dbo].[MetadataSearchPaths] SET Path = './*[local-name()=''Extension'']/Properties/Property[@name=''Height'']' WHERE Path = './tt:Extension/Properties/Property[@name=''Height'']'
UPDATE [dbo].[MetadataSearchPaths] SET Path = './*[local-name()=''Extension'']/Properties/Property[@name=''Class'']' WHERE Path = './tt:Extension/Properties/Property[@name=''Class'']'
UPDATE [dbo].[MetadataSearchPaths] SET Path = './*[local-name()=''Appearance'']/*[local-name()=''GeoLocation'']/@lat' WHERE Path = './tt:Appearance/tt:GeoLocation/@lat'
GO


-- The new filters

-- Categories
-- Need to match categories in XPCO\MetadataSearch\Common\Categories.cs or existing DB (should be the same)
DECLARE @VehiclesCategoryGUID [GuidType] =		'DE799EAA-7564-4882-8E86-B0FA85919559';
DECLARE @PeopleCategoryGUID [GuidType] =		'7C4765C6-BD67-45A1-9E0B-84313207379C';

-- Filters
-- The GUIDs need to match the ones in XPCO\MetadataSearch\Common\Filters.cs
DECLARE @TopCategoryFilterGUID [GuidType] =					'58687AEB-A7A3-4BE9-8D9B-59467E1B0543';
DECLARE @TopGrainFilterGUID [GuidType] =					'C558996C-BD40-4F7E-B22D-9067DB61AEAF';
DECLARE @TopColorFilterGUID [GuidType] =					'BD42A66B-FF5E-4951-AAEC-6299C1D6381A';
DECLARE @TopStyleFilterGUID [GuidType] =					'8D8404A4-4C7F-4C30-B52E-3C2A0194B3FE';
DECLARE @BottomCategoryFilterGUID [GuidType] =				'844774F8-76BB-404F-A8B8-7A0EAFF51FB6';
DECLARE @BottomGrainFilterGUID [GuidType] =					'6BC1B79B-D799-4B71-9C23-B80F512875B5';
DECLARE @BottomColorFilterGUID [GuidType] =					'103A47C0-A405-4F5E-A1A0-CE50A59E35DB';
DECLARE @BottomStyleFilterGUID [GuidType] =					'1D7664E0-F67A-4DD5-A70F-62362896397B';
DECLARE @HairLengthFilterGUID [GuidType] =					'3367DAFC-A27E-4F73-BB51-B1E586A2FE9F';
DECLARE @HairColorFilterGUID [GuidType] =					'FC7FA276-DD38-4456-AD3B-B87F76356757';
DECLARE @BagCategoryFilterGUID [GuidType] =					'ECFA5207-34B1-4588-A037-4AA6885AFF2E';
DECLARE @BagColorFilterGUID [GuidType] =					'A1F57724-941A-46DD-8773-CD8B79B6254E';
DECLARE @BagFilterGUID [GuidType] =							'135659D7-1934-4249-B6CA-D9AA2E3428C2';
DECLARE @BeardFilterGUID [GuidType] =						'5082F29A-92E0-4DA2-B2A8-70511A7F11D9';
DECLARE @MustacheFilterGUID [GuidType] =					'9A3D4D02-02B4-4DEB-9808-E0FE1D18D70F';
DECLARE @SideburnFilterGUID [GuidType] =					'0780514A-C1AE-497F-B2BC-A54CE72FF067';
DECLARE @GlassesFilterGUID [GuidType] =						'E3F21D4C-84AE-44E1-80BE-0552FD254449';
DECLARE @GlovesFilterGUID [GuidType] =						'68A2F1BF-3954-4638-8A47-FA05B7A053D1';
DECLARE @GlovesColorFilterGUID [GuidType] =					'BF1F0B6C-CE06-428A-A9E7-69DBEBBF4F3A';
DECLARE @HatFilterGUID [GuidType] =							'EA408F87-BE05-45E7-838D-1798C30352E2';
DECLARE @HatColorFilterGUID [GuidType] =					'4C345605-021F-47C6-AAF8-B76612541C9C';
DECLARE @HelmetFilterGUID [GuidType] =						'1503C856-3172-450B-AF16-D3A260ECE8E3';
DECLARE @HelmetColorFilterGUID [GuidType] =					'1A692927-1179-4213-8C2C-29BB9D66C1F3';
DECLARE @MaskFilterGUID [GuidType] =						'086DB922-ED67-4AEA-9F28-18EA90720AAB';
DECLARE @MaskColorFilterGUID [GuidType] =					'6091A365-B25D-4350-B8DE-88A2BF5AFD6C';
DECLARE @ScarfFilterGUID [GuidType] =						'1A78575F-639A-456D-A11E-84D1E8D33A24';
DECLARE @ScarfColorFilterGUID [GuidType] =					'F5E210A8-1CE3-4C79-A249-B1E131AB1A74';
DECLARE @ShoesCategoryFilterGUID [GuidType] =				'0704C12E-74E7-48BF-8F1A-AE4B57B643C3';
DECLARE @ShoesColorFilterGUID [GuidType] =					'0BF6FCAD-60CC-41F8-9067-0D03DA84ACB7';
DECLARE @VehicleModelFilterGUID [GuidType] =				'39A4FE74-D2F7-4DA6-AEFC-68D4B3517A22';
DECLARE @VehicleBrandFilterGUID [GuidType] =				'ABF9A511-DECA-44D8-BD1C-2C8ACA544B39';
DECLARE @LicensePlateCountryCodeFilterGUID [GuidType] =		'8570EEBF-0E5D-483F-97AC-00E577A8F434';
DECLARE @LicensePlateIssuingEntityFilterGUID [GuidType] =	'61FBB9BF-A652-4E5B-B98D-201B7A1805A5';
DECLARE @LicensePlateTypeFilterGUID [GuidType] =			'CF04872F-24FC-46F8-A51F-64D5BB5C6557';


DECLARE @HeightFilterGUID [GuidType] =						'ECDCB7FF-0A4B-4B4C-9272-A2FD23639070';
DECLARE @AgeFilterGUID [GuidType] =							'59FE8653-4335-497B-8BF2-490867DD6973';

DECLARE @HeightOnvifPropertyId [GuidType] =					NEWID();

-- Object definitions
-- We are doing a lookup for these because their IDs are generated dynamically
DECLARE @ObjectRoot1 [nvarchar](1024) =	'/*[local-name()=''MetadataStream'']/*[local-name()=''VideoAnalytics'']/*[local-name()=''Frame'']/*[local-name()=''Object'']';

INSERT INTO [dbo].[MetadataSearchFilters] (
Id,
Visibility,
NonLocName, 
CategoryId 
)
VALUES	( @TopCategoryFilterGUID, 0, 'ClothingTopCategory', @PeopleCategoryGUID ),
		( @TopGrainFilterGUID, 0, 'ClothingTopGrain', @PeopleCategoryGUID ),
		( @TopColorFilterGUID, 0, 'ClothingTopColor', @PeopleCategoryGUID ),
		( @TopStyleFilterGUID, 0, 'ClothingTopStyle', @PeopleCategoryGUID ),
		( @BottomCategoryFilterGUID, 0, 'ClothingBottomCategory', @PeopleCategoryGUID ),
		( @BottomGrainFilterGUID, 0, 'ClothingBottomGrain', @PeopleCategoryGUID ),
		( @BottomColorFilterGUID, 0, 'ClothingBottomColor', @PeopleCategoryGUID ),
		( @BottomStyleFilterGUID, 0, 'ClothingBottomStyle', @PeopleCategoryGUID ),
		( @HairLengthFilterGUID, 0, 'HairLength', @PeopleCategoryGUID ),
		( @HairColorFilterGUID, 0, 'HairColor', @PeopleCategoryGUID ),
		( @BagCategoryFilterGUID, 0, 'BagCategory', @PeopleCategoryGUID ),
		( @BagColorFilterGUID, 0, 'BagColor', @PeopleCategoryGUID ),
		
		( @BagFilterGUID, 0, 'Bag', @PeopleCategoryGUID ),
		( @BeardFilterGUID, 0, 'Beard', @PeopleCategoryGUID ),
		( @MustacheFilterGUID, 0, 'Mustache', @PeopleCategoryGUID ),
		( @SideburnFilterGUID, 0, 'Sideburn', @PeopleCategoryGUID ),
		( @GlassesFilterGUID, 0, 'Glasses', @PeopleCategoryGUID ),
		( @GlovesFilterGUID, 0, 'Gloves', @PeopleCategoryGUID ),
		( @GlovesColorFilterGUID, 0, 'GlovesColor', @PeopleCategoryGUID ),
		( @HatFilterGUID, 0, 'Hat', @PeopleCategoryGUID ),
		( @HatColorFilterGUID, 0, 'HatColor', @PeopleCategoryGUID ),
		( @HelmetFilterGUID, 0, 'Helmet', @PeopleCategoryGUID ),
		( @HelmetColorFilterGUID, 0, 'HelmetColor', @PeopleCategoryGUID ),
		( @MaskFilterGUID, 0, 'Mask', @PeopleCategoryGUID ),
		( @MaskColorFilterGUID, 0, 'MaskColor', @PeopleCategoryGUID ),
		( @ScarfFilterGUID, 0, 'Scarf', @PeopleCategoryGUID ),
		( @ScarfColorFilterGUID, 0, 'ScarfColor', @PeopleCategoryGUID ),
		( @ShoesCategoryFilterGUID, 0, 'ShoesCategory', @PeopleCategoryGUID ),
		( @ShoesColorFilterGUID, 0, 'ShoesColor', @PeopleCategoryGUID ),
		( @VehicleModelFilterGUID, 0, 'VehicleModel', @VehiclesCategoryGUID ),
		( @VehicleBrandFilterGUID, 0, 'VehicleBrand', @VehiclesCategoryGUID ),
		( @LicensePlateCountryCodeFilterGUID, 0, 'LicensePlateCountryCode', @VehiclesCategoryGUID ),
		( @LicensePlateIssuingEntityFilterGUID, 0, 'LicensePlateIssuingEntity', @VehiclesCategoryGUID ),
		( @LicensePlateTypeFilterGUID, 0, 'LicensePlateType', @VehiclesCategoryGUID );


INSERT INTO [dbo].[MetadataSearchProperties] (
	Id,
	PropertyName,
	FilterId 
)
VALUES	( NEWID(), 'ClothingTopCategory', @TopCategoryFilterGUID ),
		( NEWID(), 'ClothingTopGrain', @TopGrainFilterGUID ),
		( NEWID(), 'ClothingTopColor', @TopColorFilterGUID ),
		( NEWID(), 'ClothingTopStyle', @TopStyleFilterGUID ),
		( NEWID(), 'ClothingBottomCategory', @BottomCategoryFilterGUID ),
		( NEWID(), 'ClothingBottomGrain', @BottomGrainFilterGUID ),
		( NEWID(), 'ClothingBottomColor', @BottomColorFilterGUID ),
		( NEWID(), 'ClothingBottomStyle', @BottomStyleFilterGUID ),
		( NEWID(), 'HairLength', @HairLengthFilterGUID ),
		( NEWID(), 'HairColor', @HairColorFilterGUID ),
		( NEWID(), 'BagCategory', @BagCategoryFilterGUID ),
		( NEWID(), 'BagColor', @BagColorFilterGUID ),

		( NEWID(),	'Bag',	@BagFilterGUID),
		( NEWID(),	'Beard',	@BeardFilterGUID),
		( NEWID(),	'Mustache',	@MustacheFilterGUID),
		( NEWID(),	'Sideburn',	@SideburnFilterGUID),
		( NEWID(),	'Glasses',	@GlassesFilterGUID),
		( NEWID(),	'Gloves',	@GlovesFilterGUID),
		( NEWID(),	'GlovesColor',	@GlovesColorFilterGUID),
		( NEWID(),	'Hat',	@HatFilterGUID),
		( NEWID(),	'HatColor',	@HatColorFilterGUID),
		( NEWID(),	'Helmet',	@HelmetFilterGUID),
		( NEWID(),	'HelmetColor',	@HelmetColorFilterGUID),
		( NEWID(),	'Mask',	@MaskFilterGUID),
		( NEWID(),	'MaskColor',	@MaskColorFilterGUID),
		( NEWID(),	'Scarf',	@ScarfFilterGUID),
		( NEWID(),	'ScarfColor',	@ScarfColorFilterGUID),
		( NEWID(),	'ShoesCategory',	@ShoesCategoryFilterGUID),
		( NEWID(),	'ShoesColor',	@ShoesColorFilterGUID),
		( NEWID(),	'VehicleModel',	@VehicleModelFilterGUID),
		( NEWID(),	'VehicleBrand',	@VehicleBrandFilterGUID),
		( NEWID(),	'LicensePlateCountryCode',	@LicensePlateCountryCodeFilterGUID),
		( NEWID(),	'LicensePlateIssuingEntity',	@LicensePlateIssuingEntityFilterGUID),
		( NEWID(),	'LicensePlateType',	@LicensePlateTypeFilterGUID), 
		
		( @HeightOnvifPropertyId,	'HeightOnvif',	@HeightFilterGUID), 
		( NEWID(),	'AgeOnvif',	@AgeFilterGUID); 


EXEC insert_new_metadata_search_path_for_existing_property 'ClothingTopCategory', @ObjectRoot1, './*[local-name()=''Appearance'']/*[local-name()=''HumanBody'']/*[local-name()=''Clothing'']/*[local-name()=''Tops'']/*[local-name()=''Category''][text()]'
EXEC insert_new_metadata_search_path_for_existing_property 'ClothingTopGrain', @ObjectRoot1, './*[local-name()=''Appearance'']/*[local-name()=''HumanBody'']/*[local-name()=''Clothing'']/*[local-name()=''Tops'']/*[local-name()=''Grain''][text()]'
EXEC insert_new_metadata_search_path_for_existing_property 'ClothingTopColor', @ObjectRoot1, './*[local-name()=''Appearance'']/*[local-name()=''HumanBody'']/*[local-name()=''Clothing'']/*[local-name()=''Tops'']/*[local-name()=''Color'']/*[local-name()=''ColorCluster'']'
EXEC insert_new_metadata_search_path_for_existing_property 'ClothingTopStyle', @ObjectRoot1, './*[local-name()=''Appearance'']/*[local-name()=''HumanBody'']/*[local-name()=''Clothing'']/*[local-name()=''Tops'']/*[local-name()=''Style''][text()]'
EXEC insert_new_metadata_search_path_for_existing_property 'ClothingBottomCategory', @ObjectRoot1, './*[local-name()=''Appearance'']/*[local-name()=''HumanBody'']/*[local-name()=''Clothing'']/*[local-name()=''Bottoms'']/*[local-name()=''Category''][text()]'
EXEC insert_new_metadata_search_path_for_existing_property 'ClothingBottomGrain', @ObjectRoot1, './*[local-name()=''Appearance'']/*[local-name()=''HumanBody'']/*[local-name()=''Clothing'']/*[local-name()=''Bottoms'']/*[local-name()=''Grain''][text()]'
EXEC insert_new_metadata_search_path_for_existing_property 'ClothingBottomColor', @ObjectRoot1, './*[local-name()=''Appearance'']/*[local-name()=''HumanBody'']/*[local-name()=''Clothing'']/*[local-name()=''Bottoms'']/*[local-name()=''Color'']/*[local-name()=''ColorCluster'']'
EXEC insert_new_metadata_search_path_for_existing_property 'ClothingBottomStyle', @ObjectRoot1, './*[local-name()=''Appearance'']/*[local-name()=''HumanBody'']/*[local-name()=''Clothing'']/*[local-name()=''Bottoms'']/*[local-name()=''Style''][text()]'
EXEC insert_new_metadata_search_path_for_existing_property 'HairLength', @ObjectRoot1, './*[local-name()=''Appearance'']/*[local-name()=''HumanFace'']/*[local-name()=''Hair'']/*[local-name()=''Length''][text()]'
EXEC insert_new_metadata_search_path_for_existing_property 'HairColor', @ObjectRoot1, './*[local-name()=''Appearance'']/*[local-name()=''HumanFace'']/*[local-name()=''Hair'']/*[local-name()=''Color'']/*[local-name()=''ColorCluster'']'
EXEC insert_new_metadata_search_path_for_existing_property 'BagCategory', @ObjectRoot1, './*[local-name()=''Appearance'']/*[local-name()=''HumanBody'']/*[local-name()=''Belonging'']/*[local-name()=''Bag'']/*[local-name()=''Category''][text()]'
EXEC insert_new_metadata_search_path_for_existing_property 'BagColor', @ObjectRoot1, './*[local-name()=''Appearance'']/*[local-name()=''HumanBody'']/*[local-name()=''Belonging'']/*[local-name()=''Bag'']/*[local-name()=''Color'']/*[local-name()=''ColorCluster'']'

EXEC insert_new_metadata_search_path_for_existing_property 'Bag',	@ObjectRoot1,	'./*[local-name()=''Appearance'']/*[local-name()=''HumanBody'']/*[local-name()=''Belonging'']/*[local-name()=''Bag'']'
EXEC insert_new_metadata_search_path_for_existing_property 'Beard',	@ObjectRoot1,	'./*[local-name()=''Appearance'']/*[local-name()=''HumanFace'']/*[local-name()=''FacialHair'']/*[local-name()=''Beard''][text()]'
EXEC insert_new_metadata_search_path_for_existing_property 'Mustache',	@ObjectRoot1,	'./*[local-name()=''Appearance'']/*[local-name()=''HumanFace'']/*[local-name()=''FacialHair'']/*[local-name()=''Mustache''][text()]'
EXEC insert_new_metadata_search_path_for_existing_property 'Sideburn',	@ObjectRoot1,	'./*[local-name()=''Appearance'']/*[local-name()=''HumanFace'']/*[local-name()=''FacialHair'']/*[local-name()=''Sideburn''][text()]'
EXEC insert_new_metadata_search_path_for_existing_property 'Glasses',	@ObjectRoot1,	'./*[local-name()=''Appearance'']/*[local-name()=''HumanFace'']/*[local-name()=''Accessory'']/*[local-name()=''Opticals'']/*[local-name()=''Wear''][text()]'
EXEC insert_new_metadata_search_path_for_existing_property 'Gloves',	@ObjectRoot1,	'./*[local-name()=''Appearance'']/*[local-name()=''HumanBody'']/*[local-name()=''Clothing'']/*[local-name()=''Gloves'']/*[local-name()=''Wear''][text()]'
EXEC insert_new_metadata_search_path_for_existing_property 'GlovesColor',	@ObjectRoot1,	'./*[local-name()=''Appearance'']/*[local-name()=''HumanBody'']/*[local-name()=''Clothing'']/*[local-name()=''Gloves'']/*[local-name()=''Color'']/*[local-name()=''ColorCluster'']'
EXEC insert_new_metadata_search_path_for_existing_property 'Hat',	@ObjectRoot1,	'./*[local-name()=''Appearance'']/*[local-name()=''HumanFace'']/*[local-name()=''Accessory'']/*[local-name()=''Hat'']/*[local-name()=''Wear''][text()]'
EXEC insert_new_metadata_search_path_for_existing_property 'HatColor',	@ObjectRoot1,	'./*[local-name()=''Appearance'']/*[local-name()=''HumanFace'']/*[local-name()=''Accessory'']/*[local-name()=''Hat'']/*[local-name()=''Color'']/*[local-name()=''ColorCluster'']'
EXEC insert_new_metadata_search_path_for_existing_property 'Helmet',	@ObjectRoot1,	'./*[local-name()=''Appearance'']/*[local-name()=''HumanFace'']/*[local-name()=''Accessory'']/*[local-name()=''Helmet'']/*[local-name()=''Wear''][text()]'
EXEC insert_new_metadata_search_path_for_existing_property 'HelmetColor',	@ObjectRoot1,	'./*[local-name()=''Appearance'']/*[local-name()=''HumanFace'']/*[local-name()=''Accessory'']/*[local-name()=''Helmet'']/*[local-name()=''Color'']/*[local-name()=''ColorCluster'']'
EXEC insert_new_metadata_search_path_for_existing_property 'Mask',	@ObjectRoot1,	'./*[local-name()=''Appearance'']/*[local-name()=''HumanFace'']/*[local-name()=''Accessory'']/*[local-name()=''Mask'']/*[local-name()=''Wear''][text()]'
EXEC insert_new_metadata_search_path_for_existing_property 'MaskColor',	@ObjectRoot1,	'./*[local-name()=''Appearance'']/*[local-name()=''HumanFace'']/*[local-name()=''Accessory'']/*[local-name()=''Mask'']/*[local-name()=''Color'']/*[local-name()=''ColorCluster'']'
EXEC insert_new_metadata_search_path_for_existing_property 'Scarf',	@ObjectRoot1,	'./*[local-name()=''Appearance'']/*[local-name()=''HumanBody'']/*[local-name()=''Clothing'']/*[local-name()=''Scarf'']/*[local-name()=''Wear''][text()]'
EXEC insert_new_metadata_search_path_for_existing_property 'ScarfColor',	@ObjectRoot1,	'./*[local-name()=''Appearance'']/*[local-name()=''HumanBody'']/*[local-name()=''Clothing'']/*[local-name()=''Scarf'']/*[local-name()=''Color'']/*[local-name()=''ColorCluster'']'
EXEC insert_new_metadata_search_path_for_existing_property 'ShoesCategory',	@ObjectRoot1,	'./*[local-name()=''Appearance'']/*[local-name()=''HumanBody'']/*[local-name()=''Clothing'']/*[local-name()=''Shoes'']/*[local-name()=''Category''][text()]'
EXEC insert_new_metadata_search_path_for_existing_property 'ShoesColor',	@ObjectRoot1,	'./*[local-name()=''Appearance'']/*[local-name()=''HumanBody'']/*[local-name()=''Clothing'']/*[local-name()=''Shoes'']/*[local-name()=''Color'']/*[local-name()=''ColorCluster'']'
EXEC insert_new_metadata_search_path_for_existing_property 'VehicleModel',	@ObjectRoot1,	'./*[local-name()=''Appearance'']/*[local-name()=''VehicleInfo'']/*[local-name()=''Model''][text()]'
EXEC insert_new_metadata_search_path_for_existing_property 'VehicleBrand',	@ObjectRoot1,	'./*[local-name()=''Appearance'']/*[local-name()=''VehicleInfo'']/*[local-name()=''Brand''][text()]'
EXEC insert_new_metadata_search_path_for_existing_property 'LicensePlateCountryCode',	@ObjectRoot1,	'./*[local-name()=''Appearance'']/*[local-name()=''LicensePlateInfo'']/*[local-name()=''CountryCode''][text()]'
EXEC insert_new_metadata_search_path_for_existing_property 'LicensePlateIssuingEntity',	@ObjectRoot1,	'./*[local-name()=''Appearance'']/*[local-name()=''LicensePlateInfo'']/*[local-name()=''IssuingEntity''][text()]'
EXEC insert_new_metadata_search_path_for_existing_property 'LicensePlateType',	@ObjectRoot1,	'./*[local-name()=''Appearance'']/*[local-name()=''LicensePlateInfo'']/*[local-name()=''PlateType''][text()]'
EXEC insert_new_metadata_search_path_for_existing_property 'AgeOnvif',	@ObjectRoot1,	'./*[local-name()=''Appearance'']/*[local-name()=''HumanFace'']/*[local-name()=''Age'']'

-- Already existing properties
EXEC insert_new_metadata_search_path_for_existing_property 'Class', @ObjectRoot1, './*[local-name()=''Appearance'']/*[local-name()=''Class'']/*[local-name()=''Type''][text()!=''HumanFace'']'
EXEC insert_new_metadata_search_path_for_existing_property 'Face', @ObjectRoot1, './*[local-name()=''Appearance'']/*[local-name()=''Class'']/*[local-name()=''Type''][text()=''HumanFace'']'

UPDATE [dbo].[MetadataSearchPaths] SET Path = './*[local-name()=''Appearance'']/*[local-name()=''HumanBody'']/*[local-name()=''BodyMetric'']/*[local-name()=''Height''][text()]', PropertyId = @HeightOnvifPropertyId WHERE Path = './tt:Appearance/tt:HumanBody/tt:BodyMetric/tt:Height[text()]'
GO