-- This header is used by the dbupdate7 tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[GetDeviceGroupRelations]') AND type in (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [GetDeviceGroupRelations]
GO

CREATE FUNCTION [GetDeviceGroupRelations] (@IDDeviceGroup GuidType)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	IF(@IDDeviceGroup IS NULL OR @IDDeviceGroup = '')
	BEGIN
		RETURN ''
	END

	DECLARE @Ids NVARCHAR(MAX)
	;WITH CTERelations AS
	(
		SELECT @IDDeviceGroup AS ID

		UNION ALL
				 
		SELECT dg.IDParentDeviceGroup AS ID
		FROM [DeviceGroups] dg
		INNER JOIN CTERelations cte ON cte.ID = dg.IDDeviceGroup	 
	)
	SELECT @Ids = 
	STUFF(( 
		SELECT '; ' + ID 
		FROM CTERelations
		FOR XML PATH('')), 1,1, '')
	RETURN @Ids
END
GO