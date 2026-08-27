-- This header is used by the dbupdate tool.
-- The TARGET poperty is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


IF NOT EXISTS (SELECT PropertyName FROM [dbo].[MetadataSearchProperties] WHERE PropertyName = 'Class')
BEGIN
	INSERT INTO [dbo].[MetadataSearchProperties] (Id, PropertyName, FilterId)
	VALUES (NEWID(), 'Class', NULL)
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[MetadataSearchPaths]') AND NAME = N'PropertyId')
BEGIN
	ALTER TABLE [dbo].[MetadataSearchPaths]
	ADD PropertyId [GuidType] NULL
END
GO

IF EXISTS (SELECT * FROM [dbo].[MetadataSearchPaths] WHERE PropertyId IS NULL)
BEGIN
	UPDATE [dbo].[MetadataSearchPaths]
	SET [PropertyId] = Properties.Id
	FROM [dbo].[MetadataSearchPaths] AS SearchPaths
	INNER JOIN [dbo].[MetadataSearchProperties] AS Properties
	ON SearchPaths.PropertyName = Properties.PropertyName
END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[MetadataSearchPaths]') AND NAME = N'PropertyId' AND is_nullable = 1)
BEGIN
	ALTER TABLE [dbo].[MetadataSearchPaths]
	ALTER COLUMN [PropertyId] [GuidType] NOT NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE parent_object_id = OBJECT_ID(N'[MetadataSearchPaths]') AND NAME = 'FK_MetadataSearchPaths_MetadataSearchProperties')
BEGIN
ALTER TABLE [dbo].[MetadataSearchPaths]
ADD CONSTRAINT FK_MetadataSearchPaths_MetadataSearchProperties FOREIGN KEY (PropertyId)
REFERENCES MetadataSearchProperties(Id)
END
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[MetadataSearchPaths]') AND NAME = N'PropertyName')
BEGIN
	ALTER TABLE [dbo].[MetadataSearchPaths]
	DROP COLUMN [PropertyName]
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_metadata_search_property_definition_by_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_metadata_search_property_definition_by_id]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_metadata_search_property_definition_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_metadata_search_property_definition_ids]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_metadata_search_property_definitions_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_metadata_search_property_definitions_by_ids]
GO