-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- QueryItemsByKind - gets the MIP items of a specific kind
IF OBJECT_ID ( '[Mip].[QueryItemsByKind]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Mip].[QueryItemsByKind];
GO

CREATE PROCEDURE [Mip].[QueryItemsByKind]
	@Kind UNIQUEIDENTIFIER
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
	WHERE Kind = @Kind
END
GO