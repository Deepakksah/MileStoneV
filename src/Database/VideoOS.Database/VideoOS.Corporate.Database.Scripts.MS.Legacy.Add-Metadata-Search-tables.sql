-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MetadataSearchCategories]') AND type = 'U')
CREATE TABLE [dbo].[MetadataSearchCategories](
	[Id] [GuidType] NOT NULL,
	[Visibility] [bit] NOT NULL,
	[NonLocName] [nvarchar](256) NOT NULL
 CONSTRAINT [PK_MetadataSearchCategories] PRIMARY KEY ( [Id] ));
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MetadataSearchFilters]') AND type = 'U')
CREATE TABLE [dbo].[MetadataSearchFilters](
	[Id] [GuidType] NOT NULL,
	[Visibility] [bit] NOT NULL,
	[NonLocName] [nvarchar](256) NOT NULL,
	[CategoryId] [GuidType] NOT NULL
 CONSTRAINT [PK_MetadataSearchFilters] PRIMARY KEY ( [Id] ), 
 CONSTRAINT [FK_MetadataSearchFilters_MetadataSearchCategories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[MetadataSearchCategories] ([Id]));
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MetadataSearchObjectDefinitions]') AND type = 'U')
CREATE TABLE [dbo].[MetadataSearchObjectDefinitions](
	[Id] [GuidType] NOT NULL,
	[ObjectRoot] [nvarchar](1024) NOT NULL
 CONSTRAINT [PK_MetadataSearchObjectDefinitions] PRIMARY KEY ( [Id] )); 
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MetadataSearchPaths]') AND type = 'U')
CREATE TABLE [dbo].[MetadataSearchPaths](
	[Id] [GuidType] NOT NULL,
	[PropertyName] [nvarchar](256) NOT NULL,
	[Path] [nvarchar](1024) NOT NULL,
	[ObjectDefinitionId] [GuidType] NOT NULL
 CONSTRAINT [PK_MetadataSearchPaths] PRIMARY KEY( [Id] ),
 CONSTRAINT [FK_MetadataSearchPaths_MetadataSearchObjectDefinitions_ObjectDefinitionId] FOREIGN KEY ([ObjectDefinitionId]) REFERENCES [dbo].[MetadataSearchObjectDefinitions] ([Id]));
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MetadataSearchPathNamespaces]') AND type = 'U')
CREATE TABLE [dbo].[MetadataSearchPathNamespaces](
	[Id] [GuidType] NOT NULL,
	[Prefix] [nvarchar](256) NOT NULL,
	[Uri] [nvarchar](1024) NOT NULL,
 CONSTRAINT [PK_MetadataSearchPathNamespaces] PRIMARY KEY ( [Id] ));
GO


--Define helper stored procedures which will we be cleaned up at the end

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Temp_Insert_MetadataSearchFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [Temp_Insert_MetadataSearchFilter]
GO

CREATE PROCEDURE [Temp_Insert_MetadataSearchFilter]
	@Id [GuidType],
	@Visibility [bit],
	@NonLocName [nvarchar](256),
	@CategoryGuid [GuidType] 
AS
	INSERT INTO [dbo].[MetadataSearchFilters] (
	Id,
	Visibility,
	NonLocName, 
	CategoryId 
	)
	VALUES(@Id, @Visibility, @NonLocName, @CategoryGuid)
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Temp_Insert_MetadataSearchPath]') AND type in (N'P', N'PC'))
DROP PROCEDURE [Temp_Insert_MetadataSearchPath]
GO

CREATE PROCEDURE [Temp_Insert_MetadataSearchPath]
	@ObjectRoot [nvarchar](1024),
	@PropertyName [nvarchar](256),
	@PathToBeInserted [nvarchar](1024)
AS
	DECLARE @Id [GuidType] = NEWID()
	INSERT INTO [dbo].[MetadataSearchPaths] (
	[id],
	[PropertyName],
	[Path],
	[ObjectDefinitionId]
	)
	SELECT TOP 1 @Id, @PropertyName, @PathToBeInserted, Id 
	FROM [dbo].[MetadataSearchObjectDefinitions]
	WHERE [ObjectRoot]=@ObjectRoot
GO

--Manually insert the default values

--Categories
DECLARE @VehiclesCategoryGUID [GuidType] =		'DE799EAA-7564-4882-8E86-B0FA85919559';
DECLARE @PeopleCategoryGUID [GuidType] =		'7C4765C6-BD67-45A1-9E0B-84313207379C';
DECLARE @LocationCategoryGUID [GuidType] =	    '4E22B171-B878-4751-8184-DD87B2DD65F1';

--Filters
DECLARE @LicensePlateFilterGUID [GuidType] =			'489A1205-5119-40B7-B1BA-C9A3F6DC2E92';
DECLARE @CountryCodeFilterGUID [GuidType] =				'D3441184-4BF8-4747-AFF5-AAE420F59960';
DECLARE @SpeedFilterGUID [GuidType] =					'621FEB37-B0A1-4C63-9F0A-E78B1AC1966D';
DECLARE @VehicleTypeFilterGUID [GuidType] =				'E07EF69F-F064-48FA-B2EA-2C0C6F1F19FC';
DECLARE @VehicleColorFilterGUID [GuidType] =			'2D812F0D-ED3E-4553-B3FA-642BDC95D886';
DECLARE @AgeFilterGUID [GuidType] =						'59FE8653-4335-497B-8BF2-490867DD6973';
DECLARE @FaceFilterGUID [GuidType] =					'4C6AEECB-D705-44BF-8BEA-2D866FBD3BA8';
DECLARE @GenderFilterGUID [GuidType] =					'C592F9F6-4CE7-4CEE-B7C3-FC18B58E8B36';
DECLARE @HeightFilterGUID [GuidType] =					'ECDCB7FF-0A4B-4B4C-9272-A2FD23639070';
DECLARE @GeoCoordinatesFilterGUID [GuidType] =	    '97B0BD8A-719D-4235-959A-76B7CB8FFAE2';

DECLARE @ObjectRoot1 [nvarchar](1024) =	'/tt:MetadataStream/tt:VideoAnalytics/tt:Frame/tt:Object';
DECLARE @ObjectRoot2 [nvarchar](1024) =	'/ns0:MetadataStream/ns0:VideoAnalytics/ns0:Frame/ns0:Object';
DECLARE @ObjectRoot3 [nvarchar](1024) =	'/tt:MetadataStream/tt:Event/wsnt:NotificationMessage/wsnt:Topic[text() = ''tnsaxis:CameraApplicationPlatform/ALPV.AllPlates'']/../wsnt:Message/tt:Message/tt:Data';
DECLARE @ObjectRoot4 [nvarchar](1024) =	'/tt:MetadataStream/tt:VideoAnalytics/tt:Frame/tt:Extension';
DECLARE @ObjectRoot5 [nvarchar](1024) =	'/tt:MetadataStream/tt:Extension/NavigationalData';

INSERT INTO [dbo].[MetadataSearchCategories] (
	Id,
	Visibility,
	NonLocName 
)
VALUES 
	(
		@VehiclesCategoryGUID,
		0,
		'Vehicles'
	),
	(
		@PeopleCategoryGUID,
		0,
		'People'
	),
		(
		@LocationCategoryGUID,
		0,
		'Location'
	);

--Filters
EXEC [Temp_Insert_MetadataSearchFilter] @LicensePlateFilterGUID,			0,	'LicensePlate',				@VehiclesCategoryGUID		
EXEC [Temp_Insert_MetadataSearchFilter] @CountryCodeFilterGUID,				0,	'CountryCode',				@VehiclesCategoryGUID		
EXEC [Temp_Insert_MetadataSearchFilter] @SpeedFilterGUID,					0,	'Speed',					@VehiclesCategoryGUID		
EXEC [Temp_Insert_MetadataSearchFilter] @VehicleTypeFilterGUID,				0,	'VehicleType',				@VehiclesCategoryGUID		
EXEC [Temp_Insert_MetadataSearchFilter] @VehicleColorFilterGUID,			0,	'VehicleColor',				@VehiclesCategoryGUID		
EXEC [Temp_Insert_MetadataSearchFilter] @AgeFilterGUID,						0,	'Age',						@PeopleCategoryGUID		
EXEC [Temp_Insert_MetadataSearchFilter] @FaceFilterGUID,					0,	'Face',						@PeopleCategoryGUID		
EXEC [Temp_Insert_MetadataSearchFilter] @GenderFilterGUID,					0,	'Gender',					@PeopleCategoryGUID		
EXEC [Temp_Insert_MetadataSearchFilter] @HeightFilterGUID,					0,	'Height',					@PeopleCategoryGUID		
EXEC [Temp_Insert_MetadataSearchFilter] @GeoCoordinatesFilterGUID,      	0,	'Geo coordinates',          @LocationCategoryGUID	

--Object definitions
INSERT INTO [dbo].[MetadataSearchObjectDefinitions] (
	Id, 
	ObjectRoot
)
VALUES 
	(
		NEWID(),
		@ObjectRoot1
	),
	(
		NEWID(),
		@ObjectRoot2
	),
	(
		NEWID(),
		@ObjectRoot3
	),
	(
		NEWID(),
		@ObjectRoot4
	),
	(
		NEWID(),
		@ObjectRoot5
	);

--Paths
EXEC [Temp_Insert_MetadataSearchPath] @ObjectRoot1, 'Class',		'./tt:Extension/Properties/Property[@name=''Class'']'
EXEC [Temp_Insert_MetadataSearchPath] @ObjectRoot1, 'Class',		'./tt:Appearance/tt:Class/tt:ClassCandidate/tt:Type[text()!=''Face'']'
EXEC [Temp_Insert_MetadataSearchPath] @ObjectRoot2, 'Class',		'./ns0:Extension/Properties/Property[@name=''Class'']'
EXEC [Temp_Insert_MetadataSearchPath] @ObjectRoot2, 'Class',		'./ns0:Class/tt:ClassCandidate/tt:Type[text()]'

EXEC [Temp_Insert_MetadataSearchPath] @ObjectRoot1, 'LicensePlate',	'./tt:Extension/Properties/Property[@name=''LicensePlate'']'
EXEC [Temp_Insert_MetadataSearchPath] @ObjectRoot1, 'LicensePlate',	'./tt:Appearance/tt:LicensePlateInfo/tt:PlateNumber[text()]'
EXEC [Temp_Insert_MetadataSearchPath] @ObjectRoot2, 'LicensePlate',	'./ns0:Extension/Properties/Property[@name=''LicensePlate'']'
EXEC [Temp_Insert_MetadataSearchPath] @ObjectRoot2, 'LicensePlate',	'./ns0:LicensePlateInfo/ns0:PlateNumber[text()]'
EXEC [Temp_Insert_MetadataSearchPath] @ObjectRoot3, 'LicensePlate',	'./tt:SimpleItem[@Name=''text'']/@Value'

EXEC [Temp_Insert_MetadataSearchPath]  @ObjectRoot1, 'CountryCode',	'./tt:Appearance/tt:LicensePlateInfo/tt:CountryCode[text()]'
EXEC [Temp_Insert_MetadataSearchPath]  @ObjectRoot3, 'CountryCode',	'./tt:SimpleItem[@Name=''country'']/@Value'

EXEC [Temp_Insert_MetadataSearchPath]  @ObjectRoot1, 'VehicleType',	'./tt:Appearance/tt:VehicleInfo/tt:Type[text()]'
EXEC [Temp_Insert_MetadataSearchPath]  @ObjectRoot1, 'VehicleColor','./tt:Extension/Properties/Property[@name=''Color'']'

EXEC [Temp_Insert_MetadataSearchPath]  @ObjectRoot1, 'Speed',		'./tt:Behaviour/tt:Speed[text()]'

EXEC [Temp_Insert_MetadataSearchPath]  @ObjectRoot1, 'Latitude',	'./tt:Appearance/tt:GeoLocation/@lat'
EXEC [Temp_Insert_MetadataSearchPath]  @ObjectRoot5, 'Latitude',	'./Latitude[text()]'

EXEC [Temp_Insert_MetadataSearchPath]  @ObjectRoot1, 'Longitude',	'./tt:Appearance/tt:GeoLocation/@lon'
EXEC [Temp_Insert_MetadataSearchPath]  @ObjectRoot5, 'Longitude',	'./Longitude[text()]'

EXEC [Temp_Insert_MetadataSearchPath]  @ObjectRoot1, 'Age',			'./tt:Extension/Properties/Property[@name=''Age'']'

EXEC [Temp_Insert_MetadataSearchPath]  @ObjectRoot1, 'Gender',		'./tt:Extension/Properties/Property[@name=''Gender'']'

EXEC [Temp_Insert_MetadataSearchPath]  @ObjectRoot1, 'Height',		'./tt:Extension/Properties/Property[@name=''Height'']'

EXEC [Temp_Insert_MetadataSearchPath]  @ObjectRoot1, 'Face',		'./tt:Appearance/tt:Class/tt:ClassCandidate/tt:Type[text()=''Face'']'

--Namespaces
INSERT INTO [dbo].[MetadataSearchPathNamespaces] (
	[Id],	
	[Prefix], 
	[Uri]
)
VALUES 
	(
		NEWID(),
		'tt',
		'http://www.onvif.org/ver10/schema'
	),
	(
		NEWID(),
		'wsnt',
		'http://docs.oasis-open.org/wsn/b-2'
	),
	(
		NEWID(),
		'ns0',
		'http://www.onvif.org/ver10/schema'
	);

GO

--Cleanup helper stored procedures
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Temp_Insert_MetadataSearchFilter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [Temp_Insert_MetadataSearchFilter]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Temp_Insert_MetadataSearchPath]') AND type in (N'P', N'PC'))
DROP PROCEDURE [Temp_Insert_MetadataSearchPath]
GO