-- This header is used by the dbupdate tool.
-- The TARGET poperty is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MetadataSearchClassCandidates]') AND type = 'U')
CREATE TABLE [dbo].[MetadataSearchClassCandidates](
	[Id] [GuidType] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[CategoryId] [GuidType] NOT NULL
 CONSTRAINT [PK_MetadataSearchClassCandidates] PRIMARY KEY ( [Id] ),
 CONSTRAINT [FK_MetadataSearchClassCandidates_MetadataSearchCategories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[MetadataSearchCategories] ([Id]));
GO

DECLARE @VehiclesCategoryGUID [GuidType] =		'DE799EAA-7564-4882-8E86-B0FA85919559';
DECLARE @PeopleCategoryGUID [GuidType] =		'7C4765C6-BD67-45A1-9E0B-84313207379C';
DECLARE @LocationCategoryGUID [GuidType] =	    '4E22B171-B878-4751-8184-DD87B2DD65F1';

INSERT INTO [dbo].[MetadataSearchClassCandidates] (
	Id,
	Name,
	CategoryId
)
VALUES 
(
	NEWID(),
	'Human',
	@PeopleCategoryGUID
),
(
	NEWID(),
	'Person',
	@PeopleCategoryGUID
),
(
	NEWID(),
	'People',
	@PeopleCategoryGUID
),
(
	NEWID(),
	'Vehicle',
	@VehiclesCategoryGUID
),
(
	NEWID(),
	'Vehical',
	@VehiclesCategoryGUID
);
GO