using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Installer.ServerPagesSetupPlugin.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("VideoOS.Installer.ServerPagesSetupPlugin.Properties.Resources", typeof(Resources).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	public static string ConfirmBackup_CheckBoxConfirm => ResourceManager.GetString("ConfirmBackup_CheckBoxConfirm", resourceCulture);

	public static string ConfirmBackup_Description => ResourceManager.GetString("ConfirmBackup_Description", resourceCulture);

	public static string ConfirmBackup_Headline => ResourceManager.GetString("ConfirmBackup_Headline", resourceCulture);

	public static string ConfirmBackup_Instructions => ResourceManager.GetString("ConfirmBackup_Instructions", resourceCulture);

	public static string ConfirmBackup_InstructionsHeader => ResourceManager.GetString("ConfirmBackup_InstructionsHeader", resourceCulture);

	public static string ConfirmBackup_LabelMoreInformation => ResourceManager.GetString("ConfirmBackup_LabelMoreInformation", resourceCulture);

	public static string Database_ConnectionStringFoundCaption => ResourceManager.GetString("Database_ConnectionStringFoundCaption", resourceCulture);

	public static string Database_ConnectionStringFoundText => ResourceManager.GetString("Database_ConnectionStringFoundText", resourceCulture);

	public static string Database_DBCreateFailure => ResourceManager.GetString("Database_DBCreateFailure", resourceCulture);

	public static string Database_DuplicateDevices_Found => ResourceManager.GetString("Database_DuplicateDevices_Found", resourceCulture);

	public static string Database_ErrorOccurred => ResourceManager.GetString("Database_ErrorOccurred", resourceCulture);

	public static string Database_NoRightToCreateDB_ => ResourceManager.GetString("Database_NoRightToCreateDB_", resourceCulture);

	public static string Database_UpdateDatabaseTypeFailed => ResourceManager.GetString("Database_UpdateDatabaseTypeFailed", resourceCulture);

	public static string Database_UpdateFailed_ => ResourceManager.GetString("Database_UpdateFailed_", resourceCulture);

	public static string Database_UpdateFailed_DuplicateDevicesFound => ResourceManager.GetString("Database_UpdateFailed_DuplicateDevicesFound", resourceCulture);

	public static string Database_UpdateFailed_DuplicateDevicesFound_NoSupportUrl => ResourceManager.GetString("Database_UpdateFailed_DuplicateDevicesFound_NoSupportUrl", resourceCulture);

	public static string Database_UpdateFailed_DuplicateDevicesFound_QuietInstall => ResourceManager.GetString("Database_UpdateFailed_DuplicateDevicesFound_QuietInstall", resourceCulture);

	public static string Database_UpdateFailed_DuplicateDevicesFound_QuietInstall_NoSupportUrl => ResourceManager.GetString("Database_UpdateFailed_DuplicateDevicesFound_QuietInstall_NoSupportUrl", resourceCulture);

	public static string Database_UpdateFailed_SupportLink_Here => ResourceManager.GetString("Database_UpdateFailed_SupportLink_Here", resourceCulture);

	public static string ErrorMessage_FipsEnabledOnSQLUpgrade => ResourceManager.GetString("ErrorMessage_FipsEnabledOnSQLUpgrade", resourceCulture);

	public static string ErrorMessage_FipsEnabledOnSQLUpgradeLinkText => ResourceManager.GetString("ErrorMessage_FipsEnabledOnSQLUpgradeLinkText", resourceCulture);

	public static string ErrorMessage_FipsEnabledOnSQLUpgradeTitle => ResourceManager.GetString("ErrorMessage_FipsEnabledOnSQLUpgradeTitle", resourceCulture);

	public static string LicenseUseageHeading => ResourceManager.GetString("LicenseUseageHeading", resourceCulture);

	public static string LicenseWarning_BodyLinkText => ResourceManager.GetString("LicenseWarning_BodyLinkText", resourceCulture);

	public static string LicenseWarning_Detail => ResourceManager.GetString("LicenseWarning_Detail", resourceCulture);

	public static string LicenseWarning_DetailLinkText => ResourceManager.GetString("LicenseWarning_DetailLinkText", resourceCulture);

	public static string Logging_ServiceAccountAddRightGranted => ResourceManager.GetString("Logging_ServiceAccountAddRightGranted", resourceCulture);

	public static string PrivacySettingsConfirmationAccept => ResourceManager.GetString("PrivacySettingsConfirmationAccept", resourceCulture);

	public static string PrivacySettingsConfirmationCancel => ResourceManager.GetString("PrivacySettingsConfirmationCancel", resourceCulture);

	public static string PrivacySettingsConfirmationQuestion => ResourceManager.GetString("PrivacySettingsConfirmationQuestion", resourceCulture);

	public static string PrivacySettingsConfirmationTitle => ResourceManager.GetString("PrivacySettingsConfirmationTitle", resourceCulture);

	public static string PrivacySettingsPageDoNotShareUsageData => ResourceManager.GetString("PrivacySettingsPageDoNotShareUsageData", resourceCulture);

	public static string PrivacySettingsPageHeader => ResourceManager.GetString("PrivacySettingsPageHeader", resourceCulture);

	public static string PrivacySettingsPageInfo => ResourceManager.GetString("PrivacySettingsPageInfo", resourceCulture);

	public static string PrivacySettingsPageShareUsageData => ResourceManager.GetString("PrivacySettingsPageShareUsageData", resourceCulture);

	public static string PrivacySettingsPageShareUsageDataInfo => ResourceManager.GetString("PrivacySettingsPageShareUsageDataInfo", resourceCulture);

	public static string PrivacySettingsPageShareUsageDataLegalInfo => ResourceManager.GetString("PrivacySettingsPageShareUsageDataLegalInfo", resourceCulture);

	public static string VideoLoss_Body => ResourceManager.GetString("VideoLoss_Body", resourceCulture);

	public static string VideoLoss_Heading => ResourceManager.GetString("VideoLoss_Heading", resourceCulture);

	public static Bitmap warning => (Bitmap)ResourceManager.GetObject("warning", resourceCulture);

	public static string WhatToDo_Subheading => ResourceManager.GetString("WhatToDo_Subheading", resourceCulture);

	[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
	public Resources()
	{
	}
}
