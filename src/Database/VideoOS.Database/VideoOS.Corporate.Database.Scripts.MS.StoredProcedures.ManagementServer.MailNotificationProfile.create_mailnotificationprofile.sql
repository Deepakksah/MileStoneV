-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_mailnotificationprofile]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_mailnotificationprofile]
GO

CREATE PROCEDURE [create_mailnotificationprofile]
	@IDNotificationProfile [GuidType],
	@Name VARCHAR(256),
	@Description VARCHAR(max),
	@SettingsXML VARCHAR(max),
	@IDNotificationSetting GuidType
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
		
		DECLARE @tempName NVARCHAR(256)
		SELECT @tempName = [Name] FROM [NotificationProfiles] WHERE [IDNotificationProfile] = @IDNotificationProfile
		IF @tempName IS NOT NULL 
		BEGIN
			SET @ErrorArgs = @procedureName + N': %%Name=' + @Name + N'%%'
			;THROW 60014, @ErrorArgs, 1;
			--TODO Fix log messages with new id
		END

		INSERT [NotificationProfiles] (
				[Name], 
				[Description], 
				[LastModified],
				[IDNotificationProfile]
			) VALUES (
				@Name,
				@Description,
				@CurrentUtcTime,
				@IDNotificationProfile
			)

		INSERT [NotificationSettings] (
				[IDNotificationProfile],
				[Settings],
				[LastModified],
				[IDNotificationType],
				[IDNotificationSetting]
			) VALUES (
				@IDNotificationProfile,
				@SettingsXML,
				@CurrentUtcTime,
				@MailNotification,
				@IDNotificationSetting
			)

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
