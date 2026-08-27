-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- QueryItem - gets the MIP item matching the specified id
IF OBJECT_ID ( '[Mip].[QueryItem]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Mip].[QueryItem];
GO

CREATE PROCEDURE [Mip].[QueryItem]
	@ItemId UNIQUEIDENTIFIER
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
	WHERE ItemId = @ItemId
END
GO