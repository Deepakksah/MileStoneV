-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- QueryKind - gets the MIP Kind matching the specified id
IF OBJECT_ID ( '[Mip].[QueryKind]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Mip].[QueryKind];
GO

CREATE PROCEDURE [Mip].[QueryKind]
	@KindId UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON

	SELECT *
	FROM [Mip].[Kinds] 
	WHERE KindId = @KindId
END
GO