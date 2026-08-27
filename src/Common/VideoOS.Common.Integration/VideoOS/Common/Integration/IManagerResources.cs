using System;
using System.Drawing;

namespace VideoOS.Common.Integration;

public interface IManagerResources
{
	string ConfigFileName { get; }

	string ConfigFilePath { get; }

	string CommonApplicationDataFolder { get; }

	string LocalApplicationDataFolder { get; }

	Image LoginLogo { get; }

	string LoginBarColorName1 { get; }

	string LoginBarColorName2 { get; }

	string LoginServerTypeName { get; }

	Color GridSeparatorBackgroundColor { get; }

	Color GridSeparatorForegroundColor { get; }

	Color PrimaryColor { get; }

	Icon ApplicationIcon { get; }

	Image ApplicationImageSmall { get; }

	string ApplicationName { get; }

	string ApplicationNameShort { get; }

	Image BannerLeft { get; }

	Image BannerMiddle { get; }

	Image BannerRight { get; }

	Image ConfigReportFrontPageLogo { get; }

	Image ConfigReportTopBanner { get; }

	Image LogOnBackground { get; }

	Image LogOnProductImage { get; }

	Image WizardMainLogoTop { get; }

	Image WizardMainLogoMiddle { get; }

	Image WizardMainLogoBottom { get; }

	Image AboutMainLogoTop { get; }

	Image AboutMainLogoMiddle { get; }

	Image AboutMainLogoBottom { get; }

	Image PoweredByLogo { get; }

	bool ShowPoweredByLogo { get; }

	Image WizardMainLogo { get; }

	Image WizardSmallLogo { get; }

	Image WizardSmallTopBar { get; }

	Color WizardSmallTopBarTextColor { get; }

	string OwnerInformationUrl { get; }

	string OnlineActivationCreateUserUrl { get; }

	bool ShowOnlineActivationUserPrivacyLink { get; }

	bool OnlineActivationEnabled { get; }

	string OnlineEulaUrl { get; }

	bool ShowOnlineEulaLink { get; }

	bool ShowEditOwnerLink { get; }

	string OnlineActivationPrivacyStatement { get; }

	string OemClientName { get; }

	string ViewerClientApplicationName { get; }

	Image ViewerClientApplicationImageSmall { get; }

	string MatrixApplicationName { get; }

	string StartMenuShortcutDisplayName { get; }

	string DesktopShortcutDisplayName { get; }

	string AddRemoveProgramComment { get; }

	string DefaultInstallDirectory { get; }

	string FullName { get; }

	string ApplicationNameSimple { get; }

	string ApplicationNameSimpleWithVersion { get; }

	string ApplicationRegistryName { get; }

	string CurrentUserApplicationRegistryPath { get; }

	string VendorPrimaryUrl { get; }

	string SupportProgramName { get; }

	string SupportProgramLevel1 { get; }

	string SupportProgramPage { get; }

	string SupportPortalUrl { get; }

	string SupportExpiryReminderMessage { get; }

	string SupportExpiryReminderMessageCaption { get; }

	string ApplicationHelpUrl { get; }

	string ProductVersion { get; }

	Uri CustomerExperienceProgramUrl { get; }
}
