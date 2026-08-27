using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security;
using System.Xml;
using VideoOS.Utilities.OemResource;

namespace VideoOS.Mobile.CommonUtilities;

public class OemResources : IOemResources
{
	private const string ResourceFilename = "resources.xml";

	private OemResourcePackage _oemResourcePackage;

	private static OemResources _instance;

	private static readonly object _instanceLocker = new object();

	public static Assembly EmbeddedResourceAssembly = Assembly.GetExecutingAssembly();

	public static string EmbeddedResourcePath = "VideoOS.Mobile.CommonUtilities";

	public string MobileServerFullProductName => CompanyNameShort + " " + MobileServerProductNameWithoutCompany;

	public string MobileServerProductNameWithoutCompany => ProductNameWithoutCompany + " Mobile Server";

	public string AbsAddress => GetResourceString("AbsString");

	public string AbsApiAlias => GetResourceString("AbsApiAlias");

	public bool AbsIsEnabled => GetResourceBool("AbsIsEnabled");

	public int AbsPort => GetResourceInt("AbsPort");

	public string AbsRedirectServiceAppLink => GetResourceString("AbsRedirectServiceAppLink");

	public string AbsRedirectServiceStoreLink => GetResourceString("AbsRedirectServiceStoreLink");

	public string AdminPluginLogPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Instance.OemVendorProgramDataFolderName, Instance.ManagementClientDataFolder, "Logs");

	public string BrandNameDatabaseExportFormat => GetResourceString("BrandNameDatabaseExportFormat");

	public string CompanyFullNameAdminPlugin => GetResourceString("CompanyFullNameAdminPlugin");

	public string CompanyFullNameTrayManagerAboutBox => GetResourceString("CompanyFullNameTrayManagerAboutBox");

	public string CompanyFullNameUpnpDiscovery => GetResourceString("CompanyFullNameUpnpDiscovery");

	public string CompanyTileServiceUrl => GetResourceString("CompanyGisMapServiceFullUrl");

	public string CompanyLinkInUpnpDiscoveryService => GetResourceString("CompanyLinkInUpnpDiscoveryService");

	public string CompanyLinkTrayManagerAboutBox => GetResourceString("CompanyLinkTrayManagerAboutBox");

	public string CompanyNameShort => GetResourceString("CompanyNameShort");

	public string CompanyShortNameUpnpDiscovery => GetResourceString("CompanyShortNameUpnpDiscovery");

	public string DateTimePickerDocumentationLink => GetResourceString("DateTimePickerDocumentationLink");

	public bool EnableFootagesFeature => GetResourceBool("EnableFootagesFeature");

	public bool EnableTwoStepsVerification => GetResourceBool("EnableTwoStepsVerification");

	public bool EnableUserTypesFeature => GetResourceBool("EnableUserTypesFeature");

	public bool EnableMapsFeature => GetResourceBool("EnableMapsFeature");

	public bool IsOemed => GetResourceBool("IsOemed");

	public string InstallerCaptionText => GetResourceString("InstallerCaptionText");

	public string CommunicationAlias => GetResourceString("CommunicationAlias");

	public string ManagementClientDataFolder => GetResourceString("ManagementClientDataFolder");

	public string MobileServerManagerName => GetResourceString("MobileServerManagerName");

	public string MobileServerCurrentUserRegistryKey => GetResourceString("MobileServerCurrentUserRegistryKey");

	public string MobileServerLocalMachineRegistryKey => GetResourceString("MobileServerLocalMachineRegistryKey");

	public string MobileServerServiceDescription => GetResourceString("MobileServerServiceDescription");

	public string MobileServerApplicationNameShort => GetResourceString("MobileServerApplicationNameShort");

	public string MobileClientProductName => GetResourceString("MobileClientProductName");

	public string MobileProductName => GetResourceString("MobileProductName");

	public string MobileServerTrayControllerRegistryKey => GetResourceString("MobileServerTrayControllerRegistryKey");

	public Icon MobileServerTrayControllerMobile16X16Icon => GetResourceIcon("TrayControllerMobile16X16Icon");

	public Icon MobileServerTrayControllerGatewayServerIcon => GetResourceIcon("TrayControllerGatewayServerIcon");

	public Icon MobileServerTrayControllerServiceStoppedIcon => GetResourceIcon("TrayControllerServiceStoppedIcon");

	public Icon MobileServerTrayControllerServiceRunningIcon => GetResourceIcon("TrayControllerServiceRunningIcon");

	public Icon MobileServerTrayControllerServicePendingIcon => GetResourceIcon("TrayControllerServicePendingIcon");

	public Icon MobileServerTrayControllerServiceStoppingIcon => GetResourceIcon("TrayControllerServiceStoppingIcon");

	public Icon MobileServerTrayControllerServiceDisconnectedIcon => GetResourceIcon("TrayControllerServiceDisconnectedIcon");

	public Icon MobileServerTrayControllerServiceMissingIcon => GetResourceIcon("TrayControllerServiceMissingIcon");

	public Icon MobileServerTrayControllerMobileIcon => GetResourceIcon("TrayControllerServiceMobileIcon");

	public string MobileServerTrayLogPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), OemVendorProgramDataFolderName, MobileServerProductNameWithoutCompany, "Logs");

	public string PrivacyGuideUrl => GetResourceString("PrivacyGuideUrl");

	public string ProductHelpLinkTrayManager => GetResourceString("ProductHelpLinkTrayManager");

	public string ProductHelpLinkUpnpDiscoveryService => GetResourceString("ProductHelpLinkUpnpDiscoveryService");

	public string ProductVersion => GetResourceString("ProductVersion");

	public string ProductNameWithoutCompany => GetResourceString("ProductNameWithoutCompany");

	public string PushNotificationsAddress => GetResourceString("PushNotificationsAddress");

	public string PushNotificationsApiAlias => GetResourceString("PushNotificationsApiAlias");

	public bool PushNotificationsEnabled => GetResourceBool("PushNotificationsEnabled");

	public int PushNotificationsPort => GetResourceInt("PushNotificationsPort");

	public string OemManifacturerProgramFilesFolderName => GetResourceString("OemManifacturerProgramFilesFolderName");

	public string OemVendorProgramDataFolderName => GetResourceString("OemVendorProgramDataFolderName");

	public string SurveillanceServerProductName => GetResourceString("SurveillanceServerProductName");

	public string VideoPushChannelPassword => GetResourceString("VideoPushChannelPassword");

	public int WebClientAccentColorBlue => GetResourceInt("WebClientAccentColorBlue");

	public int WebClientAccentColorGreen => GetResourceInt("WebClientAccentColorGreen");

	public int WebClientAccentColorRed => GetResourceInt("WebClientAccentColorRed");

	public string WebClientBrandNameDatabaseExportPackage => GetResourceString("WebClientBrandNameDatabaseExportPackage");

	public string WebClientBookmarkHelpLink => GetResourceString("WebClientBookmarkHelpLink");

	public string WebClientCompanyFullName => GetResourceString("WebClientCompanyFullName");

	public string WebClientCompanyShortName => GetResourceString("WebClientCompanyShortName");

	public string WebClientHelpLink => GetResourceString("WebClientHelpLink");

	public string SearchForCameraDocumentationLink => GetResourceString("SearchForCameraDocumentationLink");

	public string WhatIsNewDocumentationLink => GetResourceString("WhatIsNewDocumentationLink");

	public bool WebClientIsOemed => GetResourceBool("WebClientIsOemed");

	public string WebClientLegalLink => GetResourceString("WebClientLegalLink");

	public string WebClientNameAdminPlugin => GetResourceString("WebClientNameAdminPlugin");

	public string WebClientNameTrayManager => GetResourceString("WebClientNameTrayManager");

	public string WebClientName => GetResourceString("WebClientName");

	public string WebClientProductName => GetResourceString("WebClientProductName");

	public string WebClientProductNameVersioned => GetResourceString("WebClientProductNameVersioned");

	public string WebClientProductTitle => GetResourceString("WebClientProductTitle");

	public string WebClientProductVersion => GetResourceString("ProductVersion");

	public Image WebClientBackgroundImage => GetResourceImage("WebClientBackgroundImage");

	public Icon WebClientFavicon => GetResourceIcon("WebClientFavicon");

	public VectorGraphicsResource WebClientLogoImage => GetResourceVectorGraphics("WebClientLogoImage");

	public VectorGraphicsResource WebClientSloganImage => GetResourceVectorGraphics("WebClientSloganImage");

	public static IOemResources Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (_instanceLocker)
				{
					if (_instance == null)
					{
						_instance = new OemResources();
					}
				}
			}
			return _instance;
		}
	}

	public string EmbeddedResourceXml
	{
		get
		{
			string empty = string.Empty;
			using Stream stream = EmbeddedResourceAssembly.GetManifestResourceStream(EmbeddedResourcePath + ".resources.xml");
			using StreamReader streamReader = new StreamReader(stream);
			return streamReader.ReadToEnd();
		}
	}

	private static string ResourceFilePath
	{
		get
		{
			string text = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string text2 = Path.Combine(text, "resources.xml");
			if (!File.Exists(text2))
			{
				text = AppDomain.CurrentDomain.BaseDirectory;
				if (!string.IsNullOrEmpty(AppDomain.CurrentDomain.RelativeSearchPath))
				{
					text = ((!Path.IsPathRooted(AppDomain.CurrentDomain.RelativeSearchPath)) ? new DirectoryInfo(Path.Combine(text, AppDomain.CurrentDomain.RelativeSearchPath)).FullName : AppDomain.CurrentDomain.RelativeSearchPath);
				}
				text2 = Path.Combine(text, "resources.xml");
			}
			if (!File.Exists(text2))
			{
				string text3 = Directory.GetParent(text)?.FullName;
				if (!string.IsNullOrEmpty(text3))
				{
					text2 = Path.Combine(text3, "resources.xml");
				}
			}
			return text2;
		}
	}

	private OemResources()
	{
		LoadAndValidate();
	}

	private void LoadAndValidate()
	{
		bool flag = false;
		XmlDocument xmlDocument = new XmlDocument();
		try
		{
			if (!flag)
			{
				xmlDocument.Load(ResourceFilePath);
				flag = true;
			}
		}
		catch (Exception)
		{
		}
		try
		{
			if (!flag)
			{
				xmlDocument.LoadXml(EmbeddedResourceXml);
				flag = true;
			}
		}
		catch (Exception)
		{
		}
		if (!flag)
		{
			throw new FileNotFoundException("resources.xml not found neither in folder, or as embedded resource");
		}
		_oemResourcePackage = new OemResourcePackage(xmlDocument);
		if (!_oemResourcePackage.ValidateSignature())
		{
			throw new SecurityException("The OEM resource file is signed with an invalid signature.");
		}
	}

	private string GetResourceString(string name = "")
	{
		if (_oemResourcePackage == null)
		{
			return "[_oemResourcePackage is null ]";
		}
		OemResourceItemBase resourceItem = _oemResourcePackage.GetResourceItem(name);
		if (resourceItem == null)
		{
			return "[" + name + "]";
		}
		return resourceItem.ResourceValueAsObject as string;
	}

	private Icon GetResourceIcon(string name = "")
	{
		if (_oemResourcePackage == null)
		{
			return null;
		}
		OemResourceItemBase resourceItem = _oemResourcePackage.GetResourceItem(name);
		if (resourceItem == null)
		{
			return null;
		}
		return resourceItem.ResourceValueAsObject as Icon;
	}

	private Image GetResourceImage(string name = "")
	{
		if (_oemResourcePackage == null)
		{
			return null;
		}
		OemResourceItemBase resourceItem = _oemResourcePackage.GetResourceItem(name);
		if (resourceItem == null)
		{
			return null;
		}
		return resourceItem.ResourceValueAsObject as Image;
	}

	public VectorGraphicsResource GetResourceVectorGraphics(string name = "")
	{
		if (_oemResourcePackage == null)
		{
			return null;
		}
		OemResourceItemBase resourceItem = _oemResourcePackage.GetResourceItem(name);
		if (resourceItem == null)
		{
			return null;
		}
		return resourceItem.ResourceValueAsObject as VectorGraphicsResource;
	}

	private bool GetResourceBool(string name = "")
	{
		string resourceString = GetResourceString(name);
		return string.Compare(resourceString, "true", StringComparison.InvariantCultureIgnoreCase) == 0;
	}

	private int GetResourceInt(string name = "")
	{
		if (_oemResourcePackage == null)
		{
			return 0;
		}
		OemResourceItemBase resourceItem = _oemResourcePackage.GetResourceItem(name);
		if (resourceItem == null)
		{
			return 0;
		}
		return (int)resourceItem.ResourceValueAsObject;
	}
}
