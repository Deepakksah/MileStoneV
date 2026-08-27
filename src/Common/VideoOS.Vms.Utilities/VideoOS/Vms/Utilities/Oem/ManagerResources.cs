using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using VideoOS.Utilities.OemResource;

namespace VideoOS.Vms.Utilities.Oem;

public class ManagerResources : ApplicationResources, IManagerResources
{
	public string ConfigFileName => "AdminConfig.xml";

	public string ConfigFilePath => Path.GetFullPath(Path.Combine(CommonApplicationDataFolder, ConfigFileName));

	public string CommonApplicationDataFolder => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), OemResources.Instance.App.CompanyName), OemResources.Instance.Manager.DefaultInstallDirectory);

	public string LocalApplicationDataFolder => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), OemResources.Instance.App.CompanyName), OemResources.Instance.Manager.DefaultInstallDirectory);

	public Image LoginLogo => GetResourceImage("LoginLogo");

	public string LoginBarColorName1 => GetResourceString("LoginBarColorName1");

	public string LoginBarColorName2 => GetResourceString("LoginBarColorName2");

	public string LoginServerTypeName => GetResourceString("ManagerLoginServerTypeName");

	public Color GridSeparatorBackgroundColor => Color.FromName(GetResourceString("GridSeparatorBackgroundColorName"));

	public Color GridSeparatorForegroundColor => Color.FromName(GetResourceString("GridSeparatorForegroundColorName"));

	public Icon ApplicationIcon => GetResourceIcon("ManagerApplicationIcon");

	public Image ApplicationImageSmall => GetResourceImage("ManagerApplicationImageSmall");

	public string ApplicationName => GetResourceString("ManagerApplicationName");

	public string ApplicationNameShort => GetResourceString("ManagerApplicationNameShort");

	public Image BannerLeft => GetResourceImage("BannerLeft");

	public Image BannerMiddle => GetResourceImage("BannerMiddle");

	public Image BannerRight => GetResourceImage("BannerRight");

	public Image ConfigReportFrontPageLogo => GetResourceImage("ConfigReportFrontPageLogo");

	public Image ConfigReportTopBanner => GetResourceImage("ConfigReportTopBanner");

	public Image LogOnBackground => GetResourceImage("LoginBackground");

	public Image LogOnProductImage => GetResourceImage("LoginProductImage");

	public Image WizardMainLogoTop => GetResourceImage("WizardMainLogoTop");

	public Image WizardMainLogoMiddle => GetResourceImage("WizardMainLogoMiddle");

	public Image WizardMainLogoBottom => GetResourceImage("WizardMainLogoBottom");

	public Image AboutMainLogoTop => GetResourceImage("AboutMainLogoTop");

	public Image AboutMainLogoMiddle => GetResourceImage("AboutMainLogoMiddle");

	public Image AboutMainLogoBottom => GetResourceImage("AboutMainLogoBottom");

	public Image PoweredByLogo => GetResourceImage("PoweredByLogo");

	public bool ShowPoweredByLogo => bool.Parse(GetResourceString("PoweredByFlag"));

	public Image WizardMainLogo => GetResourceImage("WizardMainLogo");

	public Image WizardSmallLogo => GetResourceImage("WizardSmallLogo");

	public Image WizardSmallTopBar => GetResourceImage("WizardSmallTopBar");

	public Color WizardSmallTopBarTextColor => Color.FromName(GetResourceString("WizardSmallTopBarTextColorName"));

	public string OwnerInformationUrl => GetResourceString("ManagerOwnerInformationUrl");

	public string OnlineActivationCreateUserUrl => GetResourceString("ManagerOnlineActivationCreateUserUrl");

	public bool ShowOnlineActivationUserPrivacyLink => GetResourceInt32("ShowOnlineActivationUserPrivacyLink") == 1;

	public bool OnlineActivationEnabled => GetResourceInt32("OnlineActivationEnabled") == 1;

	public string OnlineActivationPrivacyStatement
	{
		get
		{
			string resourceString = GetResourceString("ManagerOnlineActivationPrivacyStatement", CultureInfo.CurrentUICulture, new CultureInfo("en-US", useUserOverride: false), OemResourceCultureLookupOption.ExactThenParent);
			if (resourceString == null)
			{
				return string.Empty;
			}
			return resourceString;
		}
	}

	public bool ShowOnlineEulaLink => GetResourceInt32("ShowOnlineEulaLink") == 1;

	public bool ShowEditOwnerLink => GetResourceInt32("ShowEditOwnerLink") == 1;

	public string OnlineEulaUrl
	{
		get
		{
			string resourceString = GetResourceString("ManagerOnlineEulaStatement", CultureInfo.CurrentUICulture, new CultureInfo("en-US", useUserOverride: false), OemResourceCultureLookupOption.ExactThenParent);
			if (resourceString == null)
			{
				return string.Empty;
			}
			return resourceString;
		}
	}

	public string OemClientName => GetResourceString("OemClientName");

	public string ViewerClientApplicationName => GetResourceString("ViewerClientApplicationName");

	public Image ViewerClientApplicationImageSmall => GetResourceImage("ViewerClientApplicationImageSmall");

	public string MatrixApplicationName => GetResourceString("MatrixBasedApplicationName");

	public string StartMenuShortcutDisplayName => GetResourceString("ManagerStartMenuShortcutDisplayName");

	public string DesktopShortcutDisplayName => GetResourceString("ManagerDesktopShortcutDisplayName");

	public string AddRemoveProgramComment => GetResourceString("ManagerAddRemoveProgramComment");

	public string DefaultInstallDirectory => GetResourceString("ManagerInstallDir");

	public string FullName => GetResourceString("ManagerFullName").Replace("VERSION", Version);

	public string ApplicationNameSimple => GetResourceString("ManagerApplicationNameSimple");

	public string ApplicationNameSimpleWithVersion => GetResourceString("ManagerApplicationNameSimpleWithVersion");

	public string ApplicationRegistryName => GetResourceString("ManagerApplicationRegistryName");

	public string CurrentUserApplicationRegistryPath => string.Format(CultureInfo.InvariantCulture, "HKEY_CURRENT_USER\\Software\\{0}\\{1}", OemResources.Instance.App.CompanyName, ApplicationRegistryName);

	public string VendorPrimaryUrl => GetResourceString("VendorPrimaryUrl");

	public string SupportProgramName => GetResourceString("SupportProgramName");

	public string SupportProgramLevel1 => GetResourceString("SupportProgramLevel1");

	public string SupportProgramPage => GetResourceString("SupportProgramPage");

	public string SupportPortalUrl => GetResourceString("SupportPortalUrl");

	public Uri CustomerExperienceProgramUrl
	{
		get
		{
			string resourceString = GetResourceString("ManagerCustomerExpirProgrUrl");
			if (string.IsNullOrEmpty(resourceString))
			{
				return null;
			}
			try
			{
				return new Uri(resourceString);
			}
			catch (UriFormatException)
			{
				return null;
			}
		}
	}

	public ManagerResources(OemResourcePackage resourcePackage)
		: base(resourcePackage)
	{
	}
}
