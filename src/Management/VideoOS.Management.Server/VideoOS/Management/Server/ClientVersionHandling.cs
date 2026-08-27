using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using VideoOS.Common.Integration;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

public class ClientVersionHandling
{
	private IOemResources _oemResources;

	public ClientVersionHandling()
		: this(OemResources.Instance)
	{
	}

	public ClientVersionHandling(IOemResources oemResources)
	{
		_oemResources = oemResources ?? throw new ArgumentNullException("oemResources");
	}

	public SmartClientVersion CheckSmartClientVersion(IGeminiRequestContext requestContext, int major, int minor, string revision)
	{
		ClientVersion clientVersion = GetClientVersion(requestContext);
		ClientProfile clientProfile = ClientProfile.QueryActiveSmartClientProfile(requestContext);
		bool allowOnlyFipsAlgorithms = CryptoConfig.AllowOnlyFipsAlgorithms;
		return CheckSmartClientVersion(major, minor, revision, allowOnlyFipsAlgorithms, clientVersion, clientProfile);
	}

	public static SmartClientVersion CheckSmartClientVersion(int major, int minor, string revision, bool fipsEnabled, ClientVersion clientVersion, ClientProfile clientProfile)
	{
		SmartClientUpgrade smartClientUpgrade = clientVersion.SmartClientUpgrade;
		SmartClientVersion smartClientVersion = clientVersion.SmartClientVersion;
		SCVersion sCVersion = new SCVersion(major, minor, revision);
		foreach (ForceUpgrade version in smartClientUpgrade.VersionList)
		{
			if (version.Type == ForceType.Specific && sCVersion == version.Version)
			{
				smartClientVersion.ForceUpgrade = true;
				break;
			}
			if (version.Type == ForceType.Below && sCVersion <= version.Version)
			{
				smartClientVersion.ForceUpgrade = true;
				break;
			}
		}
		if (sCVersion.Major == 1 && sCVersion.Minor == 0)
		{
			smartClientVersion.ForceUpgrade = false;
		}
		XmlDocument xDoc = LoadClientProfileSettingsXml(clientProfile.SettingsXml);
		if (GetProfileSetting(xDoc, "ApplicationNewSCVersionText", out var value))
		{
			smartClientVersion.UpgradeInfo.Message = value;
		}
		if (fipsEnabled && sCVersion < new SCVersion(20, 3, "") && (sCVersion.Major != 1 || sCVersion.Minor != 0))
		{
			smartClientVersion.ForceUpgrade = true;
		}
		if (new SCVersion(smartClientVersion.Major, smartClientVersion.Minor, smartClientVersion.Revision) > sCVersion)
		{
			GetProfileSetting(xDoc, "ApplicationNewSCVersionWindow", out var value2);
			if (!smartClientVersion.ForceUpgrade && !StringComparer.Ordinal.Equals("Show", value2))
			{
				smartClientVersion.Major = 0;
				smartClientVersion.Minor = 0;
				smartClientVersion.Revision = string.Empty;
			}
		}
		return smartClientVersion;
	}

	private ClientVersion GetClientVersion(IGeminiRequestContext requestContext)
	{
		string systemSetting;
		using (IManagementServerComponent managementServerComponent = requestContext.CreateManagementServerComponent())
		{
			systemSetting = managementServerComponent.GetSystemSetting("ClientVersion");
		}
		return ClientVersionConfig.GetSmartClientVersion(systemSetting, _oemResources);
	}

	public SmartClientVersion GetSmartClientVersion(IGeminiRequestContext requestContext)
	{
		ClientProfile activeClientProfile = ClientProfile.QueryActiveSmartClientProfile(requestContext);
		bool allowOnlyFipsAlgorithms = CryptoConfig.AllowOnlyFipsAlgorithms;
		return GetSmartClientVersion(requestContext, activeClientProfile, allowOnlyFipsAlgorithms);
	}

	public SmartClientVersion GetSmartClientVersion(IGeminiRequestContext requestContext, ClientProfile activeClientProfile, bool fipsEnabled)
	{
		ClientVersion clientVersion = GetClientVersion(requestContext);
		XmlDocument xDoc = LoadClientProfileSettingsXml(activeClientProfile.SettingsXml);
		if (GetProfileSetting(xDoc, "ApplicationNewSCVersionText", out var value))
		{
			clientVersion.SmartClientVersion.UpgradeInfo.Message = value;
		}
		if (fipsEnabled && new SCVersion(clientVersion.SmartClientVersion.Major, clientVersion.SmartClientVersion.Minor) < new SCVersion(20, 3))
		{
			clientVersion.SmartClientVersion.ForceUpgrade = true;
		}
		if (clientVersion.SmartClientVersion.ForceUpgrade)
		{
			GetProfileSetting(xDoc, "ApplicationNewSCVersionWindow", out var value2);
			if (!StringComparer.Ordinal.Equals("Show", value2))
			{
				clientVersion.SmartClientVersion.Major = 0;
				clientVersion.SmartClientVersion.Minor = 0;
				clientVersion.SmartClientVersion.Revision = string.Empty;
			}
		}
		return clientVersion.SmartClientVersion;
	}

	private static XmlDocument LoadClientProfileSettingsXml(string clientProfileSettingsXml)
	{
		using StringReader input = new StringReader(clientProfileSettingsXml);
		using XmlReader reader = XmlReader.Create(input, new XmlReaderSettings
		{
			DtdProcessing = DtdProcessing.Prohibit
		});
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(reader);
		return xmlDocument;
	}

	private static bool GetProfileSetting(XmlDocument xDoc, string settingName, out string value)
	{
		XmlNode xmlNode = xDoc.SelectSingleNode("//Settings/Setting[@key='" + settingName + "']/Value");
		if (xmlNode == null)
		{
			value = null;
			return false;
		}
		value = xmlNode.InnerText;
		return true;
	}
}
