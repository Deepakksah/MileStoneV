-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- QueryItemsByKindAndParent - gets the MIP items of a specific kind and parent
IF OBJECT_ID ( '[Mip].[QueryItemsByKindAndParent]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Mip].[QueryItemsByKindAndParent];
GO

CREATE PROCEDURE [Mip].[QueryItemsByKindAndParent]
	@Kind UNIQUEIDENTIFIER,
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
	WHERE Kind = @Kind AND ParentId = @ParentId
END
GO