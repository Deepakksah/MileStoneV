-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- QueryItemsByParent - gets the MIP items of a specific parent
IF OBJECT_ID ( '[Mip].[QueryItemsByParent]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Mip].[QueryItemsByParent];
GO

CREATE PROCEDURE [Mip].[QueryItemsByParent]
	@ParentId UNIQUEIDENTIFIER
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
	WHERE ParentId = @ParentId
END
GO