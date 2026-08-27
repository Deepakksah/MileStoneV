-- This header is used by the dbupdate tool.
-- The TARGET poperty is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MetadataSearchProperties]') AND type = 'U')
CREATE TABLE [dbo].[MetadataSearchProperties](
	[Id] [GuidType] NOT NULL,
	[PropertyName] [nvarchar](256) NOT NULL,
	[FilterId] [GuidType] NULL
 CONSTRAINT [PK_MetadataSearchProperties] PRIMARY KEY ( [Id] ),
 CONSTRAINT [FK_MetadataSearchProperties_MetadataSearchFilters_FilterId] FOREIGN KEY ([FilterId]) REFERENCES [dbo].[MetadataSearchFilters] ([Id]));
GO


--Manually insert the default values

--Filters
DECLARE @LicensePlateFilterGUID [GuidType] =			'489A1205-5119-40B7-B1BA-C9A3F6DC2E92';
DECLARE @SpeedFilterGUID [GuidType] =					'621FEB37-B0A1-4C63-9F0A-E78B1AC1966D';
DECLARE @VehicleTypeFilterGUID [GuidType] =				'E07EF69F-F064-48FA-B2EA-2C0C6F1F19FC';
DECLARE @VehicleColorFilterGUID [GuidType] =			'2D812F0D-ED3E-4553-B3FA-642BDC95D886';
DECLARE @AgeFilterGUID [GuidType] =						'59FE8653-4335-497B-8BF2-490867DD6973';
DECLARE @FaceFilterGUID [GuidType] =					'4C6AEECB-D705-44BF-8BEA-2D866FBD3BA8';
DECLARE @GenderFilterGUID [GuidType] =					'C592F9F6-4CE7-4CEE-B7C3-FC18B58E8B36';
DECLARE @HeightFilterGUID [GuidType] =					'ECDCB7FF-0A4B-4B4C-9272-A2FD23639070';
DECLARE @GeoCoordinatesFilterGUID [GuidType] =			'97B0BD8A-719D-4235-959A-76B7CB8FFAE2';

INSERT INTO [dbo].[MetadataSearchProperties] (
	Id,
	PropertyName,
	FilterId 
)
VALUES 
	(
		NEWID(),
		'LicensePlate',
		@LicensePlateFilterGUID
	),
	(
		NEWID(),
		'Speed',
		@SpeedFilterGUID
	),
	(
		NEWID(),
		'VehicleType',
		@VehicleTypeFilterGUID
	),
	(
		NEWID(),
		'VehicleColor',
		@VehicleColorFilterGUID
	),
	(
		NEWID(),
		'Age',
		@AgeFilterGUID
	),
	(
		NEWID(),
		'Face',
		@FaceFilterGUID
	),
	(
		NEWID(),
		'Gender',
		@GenderFilterGUID
	),
	(
		NEWID(),
		'Height',
		@HeightFilterGUID
	),
	(
		NEWID(),
		'Latitude',
		@GeoCoordinatesFilterGUID
	),
	(
		NEWID(),
		'Longitude',
		@GeoCoordinatesFilterGUID
	);
GO
