-- This header is used by the dbupdate tool.
-- The TARGET poperty is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- The new filters

-- Categories
-- Need to match categories in XPCO\MetadataSearch\Common\Categories.cs or existing DB (should be the same)
DECLARE @VehiclesCategoryGUID [GuidType] = 'DE799EAA-7564-4882-8E86-B0FA85919559';

-- Filters
-- The GUIDs need to match the ones in XPCO\MetadataSearch\Common\Filters.cs
DECLARE @VehicleTypeFilterGUID [GuidType] = 'E07EF69F-F064-48FA-B2EA-2C0C6F1F19FC';

-- Object definitions
-- We are doing a lookup for these because their IDs are generated dynamically
DECLARE @ObjectRoot1 [nvarchar](1024) =	'/*[local-name()=''MetadataStream'']/*[local-name()=''VideoAnalytics'']/*[local-name()=''Frame'']/*[local-name()=''Object'']';

INSERT INTO [dbo].MetadataSearchClassCandidates 
					([Id]
					,[Name],
					[CategoryId])
VALUES 
					(NEWID()
					,'Bike'
					,@VehiclesCategoryGUID);

-- Already existing properties
EXEC insert_new_metadata_search_path_for_existing_property 'VehicleType', @ObjectRoot1, './*[local-name()=''Appearance'']/*[local-name()=''Class'']/*[local-name()=''Type''][text()]'

GO