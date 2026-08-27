-- This header is used by the dbupdate7 tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[encodeXml]') AND type in (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [encodeXml]
GO

CREATE FUNCTION [encodeXml]( @txt NVARCHAR(MAX) )
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @xmlText NVARCHAR(MAX)

	SET @xmlText = REPLACE(@txt, '&', '&amp;')
	SET @xmlText = REPLACE(@xmlText, '<', '&lt;')
	SET @xmlText = REPLACE(@xmlText, '>', '&gt;')
	SET @xmlText = REPLACE(@xmlText, '"', '&quot;')
	SET @xmlText = REPLACE(@xmlText, '''', '&apos;')

	RETURN @xmlText
END
GO