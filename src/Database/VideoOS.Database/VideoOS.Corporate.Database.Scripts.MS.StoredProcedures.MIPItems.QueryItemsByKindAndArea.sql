-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- QueryItemsByKindAndArea - gets the MIP items of a specific kind within a specific geographic area
-- ex: [Mip].[QueryItemsByKindAndArea] '57D0ED4B-3BAF-4FC0-AA1B-D333A82F2F13', 'POLYGON((-123 48, -123 47, -122 47, -122 48, -123 48))'
IF OBJECT_ID ( '[Mip].[QueryItemsByKindAndArea]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Mip].[QueryItemsByKindAndArea];
GO

CREATE PROCEDURE [Mip].[QueryItemsByKindAndArea]
	@Kind UNIQUEIDENTIFIER,
	@GisArea GEOGRAPHY
AS
BEGIN
	SET NOCOUNT ON

	SELECT 
		[ItemId],
		[Name],
		[Kind],
		[ParentId],
		[ServerId],
		[ServerScheme],
		[ServerHost],
		[ServerPort],
		[ServerType],
		[Enabled],
		[Properties],
		[GisPoint].AsTextZM() AS [GisPointString],
		[Encrypted],
		[LastModified]		
	FROM [Mip].[Items] 
	WHERE [Kind] = @Kind AND [GisPoint].STWithin(@GisArea) = 1
END
GO