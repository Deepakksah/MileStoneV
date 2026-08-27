-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- QueryKindsVisibility - gets visibility for specified MIP kinds
IF OBJECT_ID ( '[Mip].[QueryKindsVisibility]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Mip].[QueryKindsVisibility];
GO

CREATE PROCEDURE [Mip].[QueryKindsVisibility]
	@KindIds IdTable READONLY
AS
BEGIN
	SET NOCOUNT ON

	SELECT KindId, DisplayOnGisMap
	FROM [Mip].[Kinds]  
	INNER JOIN @KindIds ON KindId = Id
END
GO