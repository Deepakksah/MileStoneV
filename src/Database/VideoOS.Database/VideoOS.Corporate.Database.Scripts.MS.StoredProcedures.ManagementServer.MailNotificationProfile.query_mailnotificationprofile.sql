-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_mailnotificationprofile]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_mailnotificationprofile]
GO

CREATE PROCEDURE [query_mailnotificationprofile]
	@IDNotificationProfile [GuidType]
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @MailNotification GuidType
	SET @MailNotification = '74e649d7-d6f6-47db-8828-9a6ccfe95204'

	SELECT
		np.[LastModified],
		np.[Name],
		np.[Description],
		np.[IDNotificationProfile],
		ns.[Settings]
	FROM [NotificationProfiles] AS np 
	LEFT JOIN [NotificationSettings] AS ns ON ns.[IDNotificationProfile] = np.[IDNotificationProfile] AND ns.[IDNotificationType] = @MailNotification 
		WHERE np.[IDNotificationProfile] = @IDNotificationProfile

END
GO
