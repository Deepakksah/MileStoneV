-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_mailnotificationprofile]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_mailnotificationprofile]
GO

CREATE PROCEDURE [delete_mailnotificationprofile]
	@IDNotificationProfile [GuidType]
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @MailNotification GuidType
	SET @MailNotification = '74e649d7-d6f6-47db-8828-9a6ccfe95204'

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()

	BEGIN TRY
		BEGIN TRANSACTION

		DELETE [NotificationSettings]
			WHERE [IDNotificationProfile] = @IDNotificationProfile AND [IDNotificationType] = @MailNotification

		DELETE [NotificationProfiles]
			WHERE [IDNotificationProfile] = @IDNotificationProfile;

		COMMIT TRAN
	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		DECLARE @shouldReThrowError BIT = dbo.[ShouldReThrowError](ERROR_NUMBER(), ERROR_SEVERITY())
		IF(@shouldReThrowError = 1)
			THROW;
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE())
		;THROW 60000, @ErrorArgs, 1;
	END CATCH;

END
GO
