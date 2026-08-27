-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_valid_point]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_valid_point]
GO

CREATE PROCEDURE [query_valid_point]
	@valid_point NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON
	
	BEGIN TRY  
		DECLARE @g geography;  
		SET @g = geography::STPointFromText(@valid_point, 4326);  
		RETURN(1);
	END TRY  
	BEGIN CATCH  
		RETURN(0);
	END CATCH; 
END
GO