-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[restricted_media_live_query_by_ids]') AND [type] IN (N'P', N'PC'))
DROP PROCEDURE [restricted_media_live_query_by_ids]
GO

CREATE PROCEDURE [restricted_media_live_query_by_ids]
	@IDDevices          [tvp_StringTableGuidType] READONLY
AS
BEGIN
	SET NOCOUNT ON
		
	SELECT	[LastModified], [IDDevice], [UserName] ,[StartTime]
	FROM	[RestrictedMediaLive]
	WHERE	[IDDevice] IN (SELECT ids.[ID] FROM @IDDevices ids)
END
GO
