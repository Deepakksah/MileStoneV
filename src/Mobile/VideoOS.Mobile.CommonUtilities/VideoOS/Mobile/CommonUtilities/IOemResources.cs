using System.Drawing;
using VideoOS.Utilities.OemResource;

namespace VideoOS.Mobile.CommonUtilities;

public interface IOemResources
{
	string MobileServerFullProductName { get; }

	string MobileServerProductNameWithoutCompany { get; }

	string AbsAddress { get; }

	string AbsApiAlias { get; }

	bool AbsIsEnabled { get; }

	int AbsPort { get; }

	string AbsRedirectServiceAppLink { get; }

	string AbsRedirectServiceStoreLink { get; }

	string AdminPluginLogPath { get; }

	string BrandNameDatabaseExportFormat { get; }

	string CommunicationAlias { get; }

	string CompanyFullNameTrayManagerAboutBox { get; }

	string CompanyFullNameUpnpDiscovery { get; }

	string CompanyLinkInUpnpDiscoveryService { get; }

	string CompanyLinkTrayManagerAboutBox { get; }

	string CompanyShortNameUpnpDiscovery { get; }

	string CompanyNameShort { get; }

	string DateTimePickerDocumentationLink { get; }

	string EmbeddedResourceXml { get; }

	bool EnableFootagesFeature { get; }

	bool EnableMapsFeature { get; }

	bool EnableTwoStepsVerification { get; }

	bool EnableUserTypesFeature { get; }

	bool IsOemed { get; }

	string ManagementClientDataFolder { get; }

	string MobileClientProductName { get; }

	string MobileProductName { get; }

	string MobileServerApplicationNameShort { get; }

	string MobileServerCurrentUserRegistryKey { get; }

	string MobileServerLocalMachineRegistryKey { get; }

	string MobileServerManagerName { get; }

	string MobileServerServiceDescription { get; }

	Icon MobileServerTrayControllerGatewayServerIcon { get; }

	Icon MobileServerTrayControllerMobile16X16Icon { get; }

	Icon MobileServerTrayControllerMobileIcon { get; }

	string MobileServerTrayControllerRegistryKey { get; }

	Icon MobileServerTrayControllerServiceDisconnectedIcon { get; }

	Icon MobileServerTrayControllerServiceMissingIcon { get; }

	Icon MobileServerTrayControllerServicePendingIcon { get; }

	Icon MobileServerTrayControllerServiceRunningIcon { get; }

	Icon MobileServerTrayControllerServiceStoppedIcon { get; }

	Icon MobileServerTrayControllerServiceStoppingIcon { get; }

	string MobileServerTrayLogPath { get; }

	string OemManifacturerProgramFilesFolderName { get; }

	string OemVendorProgramDataFolderName { get; }

	string PrivacyGuideUrl { get; }

	string ProductHelpLinkTrayManager { get; }

	string ProductHelpLinkUpnpDiscoveryService { get; }

	string ProductNameWithoutCompany { get; }

	string ProductVersion { get; }

	string PushNotificationsAddress { get; }

	string PushNotificationsApiAlias { get; }

	bool PushNotificationsEnabled { get; }

	int PushNotificationsPort { get; }

	string SurveillanceServerProductName { get; }

	string VideoPushChannelPassword { get; }

	int WebClientAccentColorBlue { get; }

	int WebClientAccentColorGreen { get; }

	int WebClientAccentColorRed { get; }

	Image WebClientBackgroundImage { get; }

	string WebClientBrandNameDatabaseExportPackage { get; }

	string WebClientBookmarkHelpLink { get; }

	string WebClientCompanyFullName { get; }

	string WebClientCompanyShortName { get; }

	Icon WebClientFavicon { get; }

	string WebClientHelpLink { get; }

	string SearchForCameraDocumentationLink { get; }

	string WhatIsNewDocumentationLink { get; }

	bool WebClientIsOemed { get; }

	string WebClientLegalLink { get; }

	VectorGraphicsResource WebClientLogoImage { get; }

	string WebClientName { get; }

	string WebClientNameAdminPlugin { get; }

	string WebClientNameTrayManager { get; }

	string WebClientProductName { get; }

	string WebClientProductNameVersioned { get; }

	string WebClientProductTitle { get; }

	VectorGraphicsResource WebClientSloganImage { get; }

	string CompanyTileServiceUrl { get; }

	string WebClientProductVersion { get; }
}
