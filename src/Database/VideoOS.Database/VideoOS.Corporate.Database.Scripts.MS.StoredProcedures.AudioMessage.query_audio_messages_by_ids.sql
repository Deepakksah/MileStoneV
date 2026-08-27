-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_audio_messages_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_audio_messages_by_ids]
GO

CREATE PROCEDURE [query_audio_messages_by_ids]
    @ids tvp_StringTableGuidType READONLY
AS
BEGIN
    SET NOCOUNT ON

    SELECT  a.[ID],
            [Name],
            [Description],
            [LastModified],
            [OriginalFileName]
    FROM    [AudioMessage] a
            INNER JOIN @ids ids ON a.[ID] = ids.[ID]

END
GO