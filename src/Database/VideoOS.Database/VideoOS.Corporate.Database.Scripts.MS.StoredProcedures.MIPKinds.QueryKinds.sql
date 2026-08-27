-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- QueryKinds - gets all MIP kinds
IF OBJECT_ID ( '[Mip].[QueryKinds]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Mip].[QueryKinds];
GO

CREATE PROCEDURE [Mip].[QueryKinds]
AS
BEGIN
	SET NOCOUNT ON

	SELECT *
	FROM [Mip].[Kinds] 
END
GO