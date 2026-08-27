-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[restricted_media_query_by_id]') AND [type] IN (N'P', N'PC'))
DROP PROCEDURE [restricted_media_query_by_id]
GO

CREATE PROCEDURE [restricted_media_query_by_id]
    @IDRestrictedMedia  UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT	rm.[IDRestrictedMedia]
            , (SELECT rmd.[IDDevice] AS [Id] 
               FROM   [RestrictedMediaDevice] rmd
               WHERE rmd.[IDRestrictedMedia] = rm.IDRestrictedMedia
               FOR XML PATH(''), ROOT('Ids'), Elements) AS IDDevices
            , rm.[Header]
            , rm.[Description]
            , rm.[UserName]
            , rm.[StartTime]
            , rm.[EndTime]
            , rm.[LastModified]
    FROM	[RestrictedMedia] rm
    WHERE	rm.[IDRestrictedMedia] = @IDRestrictedMedia
END
GO
