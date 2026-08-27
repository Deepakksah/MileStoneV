-- This header is used by the dbupdate7 tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[XmlToStringTable]') AND type in (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [XmlToStringTable]
GO

CREATE FUNCTION [XmlToStringTable] (@IDs XML)
RETURNS TABLE 
AS
RETURN 
(
	SELECT [Result] = idnode.value('.', 'nvarchar(256)')
	FROM @IDs.nodes('Ids/Id') AS IdCollection(idnode)
)
GO