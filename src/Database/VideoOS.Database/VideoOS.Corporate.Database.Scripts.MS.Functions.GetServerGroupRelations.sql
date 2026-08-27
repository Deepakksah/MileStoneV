-- This header is used by the dbupdate7 tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[GetServerGroupRelations]') AND type in (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [GetServerGroupRelations]
GO

CREATE FUNCTION [GetServerGroupRelations] (@IDServerGroup GuidType)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @Ids NVARCHAR(MAX)

	IF(@IDServerGroup IS NULL OR @IDServerGroup = '')
	BEGIN
		RETURN ''
	END

	;WITH CTERelations AS
	(
		SELECT @IDServerGroup AS ID
		UNION ALL
						 
		SELECT sg.IDParentGroup AS ID
		FROM [ServerGroup] sg
		INNER JOIN CTERelations cte ON cte.ID = sg.IDServerGroup
	)
	SELECT @Ids = 
	STUFF(( 
		SELECT '; ' + ID 
		FROM CTERelations
		FOR XML PATH('')), 1,1, '')

	RETURN @Ids
END
GO