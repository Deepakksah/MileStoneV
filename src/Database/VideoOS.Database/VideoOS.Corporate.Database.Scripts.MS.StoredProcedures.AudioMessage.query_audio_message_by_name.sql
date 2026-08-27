-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_audio_message_by_name]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_audio_message_by_name]
GO

CREATE PROCEDURE [query_audio_message_by_name]
    @Name [NameType]
AS
BEGIN
    SET NOCOUNT ON

    SELECT  [ID],
            [Name],
            [Description],
            [LastModified],
            [OriginalFileName]
    FROM    [AudioMessage]
    WHERE   [Name] = @Name
END
GO