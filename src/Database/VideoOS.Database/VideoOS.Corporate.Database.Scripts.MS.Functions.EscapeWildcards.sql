-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[EscapeWildcards]') AND type in (N'FN'))
DROP FUNCTION [EscapeWildcards]
GO

-- Function to use in a WHERE like method where value can contain wildcards ('%', '[', '_')
--
-- E.g.
-- UPDATE [table] 
-- SET col1 = @col1
-- WHERE coll LIKE dbo.EscapeWildcards(@col1) + '%' ESCAPE '\'

CREATE FUNCTION [EscapeWildcards]
(
	@input AS NVARCHAR(MAX)
)
RETURNS NVARCHAR(MAX)
AS BEGIN
	DECLARE @result NVARCHAR(MAX)
	SET @result =  REPLACE(@input, '\','\\' )
	SET @result =  REPLACE(@result, '[', '\[')
	SET @result =  REPLACE(@result, '%', '\%')
	SET @result =  REPLACE(@result, '_', '\_')
	
	RETURN @result
END
GO