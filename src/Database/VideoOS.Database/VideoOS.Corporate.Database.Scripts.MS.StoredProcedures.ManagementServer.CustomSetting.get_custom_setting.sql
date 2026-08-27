-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[get_custom_setting]') AND type in (N'P', N'PC'))
DROP PROCEDURE [get_custom_setting]
GO

CREATE PROCEDURE [get_custom_setting]
	@user_sid [SidType],
	@setting_id [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		[IDCustomSetting],
		[ModifiedUser],
		[ModifiedDate],
		[DataVersion],
		[SettingDataXml]
	FROM [CustomSettings] WITH (ROWLOCK)
	WHERE [OwnerSID] = @user_sid
		AND [IDCustomSetting] = @setting_id
END
GO
