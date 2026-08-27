#define TRACE
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Security.Principal;
using System.Xml;
using System.Xml.XPath;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.LicenseInfo;
using VideoOS.Common.Integration.ServerAPI;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Utilities.Security;

namespace VideoOS.Licensing;

public class LicenseDocument : ILicenseDocument
{
	private class ActivatedHardwareInfo
	{
		public string Key { get; private set; }

		public string DLK { get; private set; }

		public int CameraCount { get; private set; }

		public bool IsInterconnectedProduct { get; private set; }

		public int[] ActivatedCameraIndexes { get; private set; }

		public ActivatedHardwareInfo(string key, string dlk, int cameraCount, bool isInterconnected, int[] activatedCameraDevices)
		{
			Key = key;
			DLK = dlk;
			CameraCount = cameraCount;
			IsInterconnectedProduct = isInterconnected;
			if (activatedCameraDevices.Length != cameraCount)
			{
				throw new InvalidOperationException("Invalid activated camera devices, must contain the same number of entries as camera count");
			}
			ActivatedCameraIndexes = activatedCameraDevices;
		}
	}

	private Dictionary<string, ActivatedHardwareInfo> _hardwareInfoDictionary;

	private Dictionary<string, IFeature> _features;

	private readonly XmlDocument _xmlLicense;

	private int? _channelLicenseCount;

	private Dictionary<string, ActivatedHardwareInfo> HardwareInfoList
	{
		get
		{
			if (_hardwareInfoDictionary == null)
			{
				_hardwareInfoDictionary = new Dictionary<string, ActivatedHardwareInfo>();
				AddKeysToHardwareInfoList(_xmlLicense.SelectNodes("License/HardwareCollection/Hardware"), _hardwareInfoDictionary, isInterconnectedProduct: false);
				AddKeysToHardwareInfoList(_xmlLicense.SelectNodes("License/ICPCollection/ICP"), _hardwareInfoDictionary, isInterconnectedProduct: true);
			}
			return _hardwareInfoDictionary;
		}
	}

	private Dictionary<string, IFeature> Features
	{
		get
		{
			if (_features != null)
			{
				return _features;
			}
			Dictionary<string, IFeature> dictionary = new Dictionary<string, IFeature>();
			foreach (XmlNode item in _xmlLicense.SelectNodes("License/Software/Features/Feature"))
			{
				IFeature feature = LicenseFactory.CreateNewFeature(item.Attributes["Name"].Value);
				foreach (XmlNode item2 in item.SelectNodes("Parameters/Parameter"))
				{
					string value = item2.Attributes["Name"].Value;
					string innerText = item2.InnerText;
					feature.Parameters.Add(value, innerText);
				}
				dictionary.Add(feature.Name, feature);
			}
			_features = dictionary;
			return _features;
		}
	}

	public bool IsTrialLicense => SLC.IsTrialLicense(SoftwareLicenseCode);

	public LicenseType LicenseType { get; private set; }

	public string MipPluginLicenseXml => (_xmlLicense.SelectSingleNode("License/MIPPlugins") ?? new XmlDocument().CreateElement("MIPPlugins")).OuterXml;

	public string ProductName
	{
		get
		{
			XmlNode xmlNode = _xmlLicense.SelectSingleNode("License/Software/Product");
			if (xmlNode != null && xmlNode.Attributes != null)
			{
				return xmlNode.Attributes["Name"].InnerText;
			}
			return string.Empty;
		}
	}

	public string ProductVersion
	{
		get
		{
			XmlNode xmlNode = _xmlLicense.SelectSingleNode("License/Software/Product");
			if (xmlNode != null && xmlNode.Attributes != null)
			{
				return xmlNode.Attributes["Version"].InnerText;
			}
			return string.Empty;
		}
	}

	public string CustomerName
	{
		get
		{
			XmlNode xmlNode = _xmlLicense.SelectSingleNode("License/Customer");
			if (xmlNode != null && xmlNode.Attributes != null)
			{
				XmlNode xmlNode2 = xmlNode.Attributes["Name"];
				if (xmlNode2 == null)
				{
					return string.Empty;
				}
				return xmlNode2.InnerText;
			}
			return string.Empty;
		}
	}

	public XmlNode WhitelistAsXmlNode => _xmlLicense.SelectSingleNode("/License/Whitelist");

	public string SoftwareUpgradePlanExpireDate
	{
		get
		{
			XmlNode xmlNode = _xmlLicense.SelectSingleNode("License/Customer/PMA");
			if (xmlNode != null && xmlNode.Attributes != null)
			{
				return xmlNode.Attributes["Expire"].InnerText;
			}
			return string.Empty;
		}
	}

	public int TotalActivatedHardwareLicenses
	{
		get
		{
			XmlNode xmlNode = _xmlLicense.SelectSingleNode("License/ActivatedHardware");
			if (xmlNode != null)
			{
				return Convert.ToInt32(xmlNode.InnerText.Trim(), CultureInfo.InvariantCulture);
			}
			return 0;
		}
	}

	public int TotalActivatedCameraLicenses
	{
		get
		{
			XmlNode xmlNode = _xmlLicense.SelectSingleNode("License/ActivatedCameraFeeds");
			if (xmlNode != null)
			{
				return Convert.ToInt32(xmlNode.InnerText.Trim(), CultureInfo.InvariantCulture);
			}
			return 0;
		}
	}

	public int TotalActivatedArculesCameraLicenses
	{
		get
		{
			int num = 0;
			XmlNodeList xmlNodeList = _xmlLicense.SelectNodes($"License/HardwareCollection/Hardware[Driver/Number='{30005}']/Feeds/Feed[@name='Camera']");
			if (xmlNodeList != null)
			{
				foreach (XmlNode item in xmlNodeList)
				{
					if (item != null && int.TryParse(item.InnerText, out var result))
					{
						num += result;
					}
				}
			}
			return num;
		}
	}

	public int TotalActivatedICPCameraLicenses
	{
		get
		{
			XmlNode xmlNode = _xmlLicense.SelectSingleNode("License/ActivatedICPCameraFeeds");
			if (xmlNode != null)
			{
				return Convert.ToInt32(xmlNode.InnerText.Trim(), CultureInfo.InvariantCulture);
			}
			return 0;
		}
	}

	public string Sites
	{
		get
		{
			XmlNode xmlNode = _xmlLicense.SelectSingleNode("License/Sites");
			if (xmlNode != null)
			{
				return xmlNode.OuterXml.Trim();
			}
			return string.Empty;
		}
	}

	public string CustomerData
	{
		get
		{
			XmlNode xmlNode = _xmlLicense.SelectSingleNode("License/CustomerData");
			if (xmlNode != null)
			{
				XmlElement xmlElement = new XmlDocument().CreateElement("Customer");
				xmlElement.InnerXml = xmlNode.InnerXml;
				return xmlElement.OuterXml;
			}
			return string.Empty;
		}
	}

	public string ProductSku
	{
		get
		{
			XmlNode xmlNode = _xmlLicense.SelectSingleNode("License/Software/Product");
			if (xmlNode != null)
			{
				if (xmlNode.Attributes["SKU"] == null)
				{
					return "XPCO";
				}
				if (xmlNode.Attributes["SKU"].InnerText != null && xmlNode.Attributes["SKU"].InnerText.Equals("Xpes+", StringComparison.OrdinalIgnoreCase))
				{
					return "XpesPlus";
				}
				return xmlNode.Attributes["SKU"].InnerText;
			}
			return null;
		}
	}

	public string SoftwareLicenseCode => _xmlLicense.SelectSingleNode("License/SLC")?.InnerText.Trim();

	public string SoftwareLicenseCodeEnvironment
	{
		get
		{
			XmlNode xmlNode = _xmlLicense.SelectSingleNode("License/SLC");
			if (xmlNode != null && xmlNode.Attributes["Environment"] != null)
			{
				return xmlNode.Attributes["Environment"].InnerText;
			}
			return null;
		}
	}

	public DateTime Issued
	{
		get
		{
			XmlNode xmlNode = _xmlLicense.SelectSingleNode("License/Issued");
			if (xmlNode != null && DateTime.TryParseExact(xmlNode.InnerText, "u", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public Collection<ILicenseProperty> AllLicenseProperties
	{
		get
		{
			Collection<ILicenseProperty> collection = new Collection<ILicenseProperty>();
			XmlNode xmlNode = _xmlLicense.SelectSingleNode("License/Properties");
			if (xmlNode != null)
			{
				XmlNodeList xmlNodeList = xmlNode.SelectNodes("Property");
				if (xmlNodeList != null)
				{
					foreach (XmlNode item in xmlNodeList)
					{
						if (item.Attributes != null)
						{
							string text = null;
							string propertyType = "";
							string innerText = item.InnerText;
							XmlAttribute xmlAttribute = item.Attributes["Name"];
							XmlAttribute xmlAttribute2 = item.Attributes["Type"];
							if (xmlAttribute != null)
							{
								text = xmlAttribute.InnerText;
							}
							if (xmlAttribute2 != null)
							{
								propertyType = xmlAttribute2.InnerText;
							}
							if (text != null)
							{
								collection.Add(LicenseFactory.CreateNewLicenseProperty(propertyType, text, innerText));
							}
							else
							{
								Trace.WriteLine("LicenseDocument: Ignoring incorrect formatted license property nodes");
							}
						}
					}
				}
			}
			return collection;
		}
	}

	public ReadOnlyCollection<string> AllFeatureNames
	{
		get
		{
			List<string> list = new List<string>();
			foreach (string key in Features.Keys)
			{
				list.Add(key);
			}
			return list.AsReadOnly();
		}
	}

	public ReadOnlyCollection<string> FeedTypes => (from XmlAttribute a in _xmlLicense.SelectNodes("License/Feeds/Feed/@name")
		select a.Value).ToList().AsReadOnly();

	public ReadOnlyCollection<string> ICPFeedTypes => (from XmlAttribute a in _xmlLicense.SelectNodes("License/ICPFeeds/Feed/@name")
		select a.Value).ToList().AsReadOnly();

	public int RecorderLicenseCount
	{
		get
		{
			XmlNode xmlNode = _xmlLicense.SelectSingleNode("/License/Recorders/@Count");
			if (xmlNode != null)
			{
				return Convert.ToInt32(xmlNode.InnerText, CultureInfo.InvariantCulture);
			}
			return 0;
		}
	}

	public int ChannelLicenseCount
	{
		get
		{
			if (!_channelLicenseCount.HasValue)
			{
				XmlAttribute xmlAttribute = null;
				if (!(_xmlLicense.SelectSingleNode("License/DeviceChannels/@Count") is XmlAttribute xmlAttribute2))
				{
					return 0;
				}
				_channelLicenseCount = Convert.ToInt32(xmlAttribute2.InnerText, CultureInfo.InvariantCulture);
			}
			return _channelLicenseCount.Value;
		}
	}

	public int HardwareLicenseCount
	{
		get
		{
			XmlAttribute xmlAttribute = null;
			if (!(_xmlLicense.SelectSingleNode("License/HardwareLicense/@Count") is XmlAttribute xmlAttribute2))
			{
				return 0;
			}
			return Convert.ToInt32(xmlAttribute2.InnerText, CultureInfo.InvariantCulture);
		}
	}

	public string EncryptedSlc
	{
		get
		{
			string result = "";
			XmlNode xmlNode = _xmlLicense.DocumentElement.SelectSingleNode("Customer/AdditionalInfo");
			if (xmlNode != null && xmlNode.Attributes["online"] != null && xmlNode.Attributes["online"].Value == "yes")
			{
				XmlNode xmlNode2 = xmlNode.SelectSingleNode("Authentication");
				result = ((xmlNode2 == null) ? "" : xmlNode2.InnerText);
			}
			return result;
		}
	}

	public int HardwareGracePeriod
	{
		get
		{
			XmlNode xmlNode = _xmlLicense.SelectSingleNode("License/HardwareGracePeriod");
			if (xmlNode == null || xmlNode.InnerText.Length == 0)
			{
				return 0;
			}
			return int.Parse(xmlNode.InnerText, CultureInfo.InvariantCulture);
		}
	}

	public IReplacementLimit ReplacementLimit
	{
		get
		{
			IReplacementLimit replacementLimit = LicenseFactory.CreateNewReplacementLimit();
			XmlNode xmlNode = _xmlLicense.SelectSingleNode("License/ReplacementCount");
			if (xmlNode != null)
			{
				int result = 0;
				int result2 = 0;
				int result3 = 0;
				XmlNode xmlNode2 = xmlNode.SelectSingleNode("Max");
				if (xmlNode2 != null && !int.TryParse(xmlNode2.InnerText, out result))
				{
					return replacementLimit;
				}
				XmlNode xmlNode3 = xmlNode.SelectSingleNode("Min");
				if (xmlNode3 != null && !int.TryParse(xmlNode3.InnerText, out result2))
				{
					return replacementLimit;
				}
				XmlNode xmlNode4 = xmlNode.SelectSingleNode("Percentage");
				if (xmlNode4 != null && !int.TryParse(xmlNode4.InnerText, out result3))
				{
					return replacementLimit;
				}
				replacementLimit.Percentage = result3;
				replacementLimit.Maximum = result;
				replacementLimit.Minimum = result2;
			}
			return replacementLimit;
		}
	}

	public DateTime ActivatedDate
	{
		get
		{
			if (_xmlLicense != null)
			{
				XmlNode xmlNode = _xmlLicense.SelectSingleNode("License/Activated");
				if (xmlNode == null)
				{
					return DateTime.MinValue;
				}
				try
				{
					return DateTime.ParseExact(xmlNode.InnerText, "u", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
				}
				catch (ArgumentNullException)
				{
				}
				catch (FormatException)
				{
				}
			}
			return DateTime.MinValue;
		}
	}

	public bool IsLicenseExpired
	{
		get
		{
			if (_xmlLicense == null)
			{
				return false;
			}
			try
			{
				VerifyExpiryDate();
			}
			catch (LicenseException)
			{
				return true;
			}
			return false;
		}
	}

	public DateTime ExpiryDate
	{
		get
		{
			if (_xmlLicense != null)
			{
				XmlNode xmlNode = _xmlLicense.SelectSingleNode("License/Expires");
				if (xmlNode == null)
				{
					return DateTime.MinValue;
				}
				try
				{
					return DateTime.ParseExact(xmlNode.InnerText, "u", CultureInfo.InvariantCulture);
				}
				catch (ArgumentNullException)
				{
				}
				catch (FormatException)
				{
				}
			}
			return DateTime.MinValue;
		}
	}

	private static void AddKeysToHardwareInfoList(XmlNodeList hardwareNodes, Dictionary<string, ActivatedHardwareInfo> infoDictionary, bool isInterconnectedProduct)
	{
		if (hardwareNodes == null)
		{
			return;
		}
		foreach (XmlNode hardwareNode in hardwareNodes)
		{
			XmlNode xmlNode2 = hardwareNode["Key"];
			if (xmlNode2 != null)
			{
				XmlNode xmlNode3 = hardwareNode["DLK"];
				XmlNode xmlNode4 = hardwareNode.SelectSingleNode("Feeds/Feed[@name='Camera']");
				XmlAttribute xmlAttribute = xmlNode4?.Attributes["channels"];
				int num = ((xmlNode4 != null) ? Convert.ToInt32(xmlNode4.InnerText, CultureInfo.InvariantCulture) : 0);
				ActivatedHardwareInfo activatedHardwareInfo = new ActivatedHardwareInfo(activatedCameraDevices: (xmlAttribute != null) ? xmlAttribute.InnerText.Split(',').Select(int.Parse).ToArray() : new int[num], key: xmlNode2.InnerText, dlk: (xmlNode3 == null) ? string.Empty : xmlNode3.InnerText, cameraCount: num, isInterconnected: isInterconnectedProduct);
				infoDictionary[activatedHardwareInfo.Key] = activatedHardwareInfo;
			}
		}
	}

	private LicenseDocument()
	{
		LicenseType = LicenseType.Unknown;
	}

	public LicenseDocument(string xmlLicense)
		: this()
	{
		if (string.IsNullOrEmpty(xmlLicense))
		{
			throw new ArgumentNullException("xmlLicense");
		}
		_xmlLicense = LoadLicense(xmlLicense);
	}

	public bool IsFeatureLicensed(string featureName)
	{
		if (string.IsNullOrEmpty(featureName))
		{
			throw new ArgumentException("Feature name is null or empty", "featureName");
		}
		if (!Features.TryGetValue(featureName, out var value))
		{
			return false;
		}
		if (value.Parameters.TryGetValue("Expires", out var value2) && DateTime.TryParseExact(value2, "u", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result) && DateTime.UtcNow.Date > result.Date)
		{
			return false;
		}
		return true;
	}

	public bool FeatureExists(string featureName)
	{
		return Features.ContainsKey(featureName);
	}

	public bool TryGetFeatureExpiryDate(string featureName, out DateTime expiryDate)
	{
		if (string.IsNullOrEmpty(featureName))
		{
			throw new ArgumentException("Feature name is null or empty", "featureName");
		}
		expiryDate = DateTime.MaxValue;
		if (!Features.TryGetValue(featureName, out var value))
		{
			return false;
		}
		if (value.Parameters.TryGetValue("Expires", out var value2) && DateTime.TryParseExact(value2, "u", CultureInfo.InvariantCulture, DateTimeStyles.None, out expiryDate))
		{
			expiryDate = expiryDate.Date;
			return true;
		}
		return false;
	}

	public string GetFeatureParameterValue(string featureName, string featureParameterName)
	{
		string parameterValue = null;
		if (!TryGetFeatureParameterValue(featureName, featureParameterName, out parameterValue))
		{
			throw new ArgumentException("Parameter not found", "featureParameterName");
		}
		return parameterValue;
	}

	public bool FeatureParameterExist(string featureName, string parameterName)
	{
		if (string.IsNullOrEmpty(featureName))
		{
			throw new ArgumentException("Feature name is null or empty", "featureName");
		}
		if (string.IsNullOrEmpty(parameterName))
		{
			throw new ArgumentException("Feature parameter name is null or empty", "parameterName");
		}
		if (!Features.TryGetValue(featureName, out var value))
		{
			return false;
		}
		return value.Parameters.ContainsKey(parameterName);
	}

	public Collection<string> GetFeatureParameterNames(string featureName)
	{
		Collection<string> collection = new Collection<string>();
		if (string.IsNullOrEmpty(featureName))
		{
			throw new ArgumentException("Feature name is null or empty", "featureName");
		}
		if (Features.TryGetValue(featureName, out var value))
		{
			foreach (KeyValuePair<string, string> parameter in value.Parameters)
			{
				collection.Add(parameter.Key);
			}
		}
		return collection;
	}

	public int FeatureParameterCount(string featureName)
	{
		if (string.IsNullOrEmpty(featureName))
		{
			throw new ArgumentException("Feature name is null or empty", "featureName");
		}
		if (!Features.TryGetValue(featureName, out var value))
		{
			return 0;
		}
		return value.Parameters.Count;
	}

	public bool TryGetFeatureParameterValue(string featureName, string parameterName, out string parameterValue)
	{
		if (string.IsNullOrEmpty(featureName))
		{
			throw new ArgumentException("Feature name is null or empty", "featureName");
		}
		if (string.IsNullOrEmpty(parameterName))
		{
			throw new ArgumentException("Feature parameter name is null or empty", "parameterName");
		}
		parameterValue = null;
		if (!Features.TryGetValue(featureName, out var value))
		{
			return false;
		}
		if (!value.Parameters.TryGetValue(parameterName, out parameterValue))
		{
			return false;
		}
		return true;
	}

	public bool IsMacAddressLicensed(string macAddress)
	{
		string macAddressHash = MacAddressUtilities.CreateMacAddressHashString(macAddress);
		return IsMacAddressLicensedHash(macAddressHash);
	}

	public bool IsMacAddressLicensedHash(string macAddressHash)
	{
		return HardwareInfoList.ContainsKey(macAddressHash);
	}

	public bool IsIcpDeviceLicensed(IDeviceInfo deviceInfo, int currentlyAllocated)
	{
		if (deviceInfo == null)
		{
			throw new ArgumentNullException("deviceInfo");
		}
		if (!deviceInfo.HardwareInfo.IsInterconnectedProduct)
		{
			throw new InvalidOperationException("Can only validate ICP devices");
		}
		if (!HardwareInfoList.ContainsKey(deviceInfo.HardwareInfo.MacAddressHashed))
		{
			return false;
		}
		ActivatedHardwareInfo activatedHardwareInfo = HardwareInfoList[deviceInfo.HardwareInfo.MacAddressHashed];
		if (activatedHardwareInfo != null && activatedHardwareInfo.IsInterconnectedProduct)
		{
			return currentlyAllocated < activatedHardwareInfo.CameraCount;
		}
		return false;
	}

	public bool IsDeviceLicensed(IDeviceInfo deviceInfo)
	{
		if (deviceInfo == null)
		{
			throw new ArgumentNullException("deviceInfo");
		}
		if (deviceInfo.HardwareInfo.IsInterconnectedProduct)
		{
			throw new InvalidOperationException("Can only validate non ICP devices");
		}
		if (!HardwareInfoList.ContainsKey(deviceInfo.HardwareInfo.MacAddressHashed))
		{
			return false;
		}
		return HardwareInfoList[deviceInfo.HardwareInfo.MacAddressHashed]?.ActivatedCameraIndexes.Contains(deviceInfo.DeviceIndex) ?? false;
	}

	public int GetHardwareCount(bool isInterconnected)
	{
		return HardwareInfoList.Count((KeyValuePair<string, ActivatedHardwareInfo> x) => x.Value.IsInterconnectedProduct == isInterconnected);
	}

	public int GetHardwareCamerasCount(string macAddress)
	{
		string key = MacAddressUtilities.CreateMacAddressHashString(macAddress);
		if (HardwareInfoList.ContainsKey(key) && HardwareInfoList.TryGetValue(key, out var value))
		{
			return value.CameraCount;
		}
		return 0;
	}

	public string GetDeviceLicenseKey(string macAddress)
	{
		string key = MacAddressUtilities.CreateMacAddressHashString(macAddress);
		if (HardwareInfoList.TryGetValue(key, out var value))
		{
			return value.DLK;
		}
		return string.Empty;
	}

	public int GetICPFeedLicenses(string feedType)
	{
		try
		{
			if (!(_xmlLicense.SelectSingleNode(string.Format(CultureInfo.InvariantCulture, "License/ICPFeeds/Feed[@name='{0}']", feedType)) is XmlElement xmlElement))
			{
				return 0;
			}
			return Convert.ToInt32(xmlElement.InnerText, CultureInfo.InvariantCulture);
		}
		catch (XPathException)
		{
			return 0;
		}
	}

	public int GetFeedLicenses(string feedType, bool isICPFeed)
	{
		if (!isICPFeed)
		{
			return GetFeedLicenses(feedType);
		}
		return GetICPFeedLicenses(feedType);
	}

	public int GetFeedLicenses(string feedType)
	{
		if (!(_xmlLicense.SelectSingleNode(string.Format(CultureInfo.InvariantCulture, "License/Feeds/Feed[@name='{0}']", feedType)) is XmlElement xmlElement))
		{
			return 0;
		}
		return Convert.ToInt32(xmlElement.InnerText, CultureInfo.InvariantCulture);
	}

	private static string GetComputerSID()
	{
		try
		{
			return Computer.GetSecurityIdentifier().ToString();
		}
		catch (Win32Exception)
		{
			return string.Empty;
		}
	}

	public void VerifyLicense()
	{
		VerifyExpiryDate();
		VerifySLC();
		if (!string.IsNullOrEmpty(GetComputerSID()))
		{
			VerifyComputerSID();
			VerifyServerMac();
			VerifyServerHarddrive();
			VerifyServerComputerName();
		}
	}

	private static void VerifyFeatureFlagCombinations(XmlDocument xmldoc)
	{
		if (xmldoc.SelectSingleNode(FormattableString.Invariant(FormattableStringFactory.Create("License/Software/Features/Feature[@Name='{0}']", "LiveMultiStreaming"))) == null)
		{
			if (xmldoc.SelectSingleNode(FormattableString.Invariant(FormattableStringFactory.Create("License/Software/Features/Feature[@Name='{0}']", "AdaptiveStreaming"))) != null)
			{
				throw new LicenseException("Adaptive streaming is not supported without the live multistreaming feature.", LicenseErrorNumber.CustomerIsNotCdsValidated);
			}
			if (xmldoc.SelectSingleNode(FormattableString.Invariant(FormattableStringFactory.Create("License/Software/Features/Feature[@Name='{0}']", "MultistreamRecording"))) != null)
			{
				throw new LicenseException("Multistream recording is not supported without the live multistreaming feature.", LicenseErrorNumber.CustomerIsNotCdsValidated);
			}
		}
	}

	public Guid SystemBuilderManufacturerId()
	{
		string xpath = "/License/SystemBuilderManufacturer/GUID";
		XmlNode xmlNode = _xmlLicense.SelectSingleNode(xpath);
		if (xmlNode == null)
		{
			return Guid.Empty;
		}
		return new Guid(xmlNode.InnerText);
	}

	private XmlDocument LoadLicense(string licenseXmlDocument)
	{
		XmlDocument xmlDocument = new XmlDocument();
		try
		{
			xmlDocument.LoadXml(licenseXmlDocument);
			VerifyXmlDocumentSignature(xmlDocument);
			VerifyLicensingMode(xmlDocument);
			VerifyFeatureFlagCombinations(xmlDocument);
			return xmlDocument;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (LicenseException)
		{
			throw;
		}
		catch
		{
			throw new LicenseException("Invalid license", LicenseErrorNumber.InvalidLicenseFile);
		}
	}

	private void VerifyExpiryDate()
	{
		try
		{
			XmlNode xmlNode = _xmlLicense.SelectSingleNode("License/Expires");
			if (xmlNode != null)
			{
				DateTime dateTime = DateTime.ParseExact(xmlNode.InnerText, "u", CultureInfo.InvariantCulture);
				if (DateTime.UtcNow.Date > dateTime.Date)
				{
					throw new LicenseException("License is expired", LicenseErrorNumber.Expired);
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			throw new LicenseException("License is expired", LicenseErrorNumber.Expired);
		}
	}

	private void VerifySLC()
	{
		try
		{
			XmlNode xmlNode = _xmlLicense.SelectSingleNode("License/SLC");
			if (xmlNode != null)
			{
				Version version = new Version(ServerVersion.GetServerVersion(Assembly.GetExecutingAssembly()));
				SLC.Validate(xmlNode.InnerText.Trim(), OemResources.Instance.App.VendorCode, version.Major, version.Minor);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			throw new LicenseException("Invalid SLC", LicenseErrorNumber.InvalidSLC);
		}
	}

	private void VerifyComputerSID()
	{
		try
		{
			XmlNode xmlNode = _xmlLicense.SelectSingleNode("License/Computer/SID");
			if (xmlNode == null || xmlNode.InnerText.Length == 0)
			{
				return;
			}
			SecurityIdentifier securityIdentifier = new SecurityIdentifier(xmlNode.InnerText);
			SecurityIdentifier securityIdentifier2 = Computer.GetSecurityIdentifier();
			if (securityIdentifier == securityIdentifier2)
			{
				return;
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception inner)
		{
			throw new LicenseException("Invalid SID", LicenseErrorNumber.InvalidSid, inner);
		}
		throw new LicenseException("Invalid SID", LicenseErrorNumber.InvalidSid);
	}

	private void VerifyServerMac()
	{
		try
		{
			XmlNode xmlNode = _xmlLicense.SelectSingleNode("License/Computer/MAC");
			if (xmlNode == null || xmlNode.InnerText.Length == 0)
			{
				return;
			}
			string[] networkAdapterMacAddresses = Computer.GetNetworkAdapterMacAddresses();
			foreach (string b in networkAdapterMacAddresses)
			{
				if (string.Equals(xmlNode.InnerText.Trim(), b, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception inner)
		{
			throw new LicenseException("Invalid MAC address", LicenseErrorNumber.InvalidMacAddress, inner);
		}
		throw new LicenseException("Invalid MAC address", LicenseErrorNumber.InvalidMacAddress);
	}

	private void VerifyServerHarddrive()
	{
		try
		{
			XmlNode xmlNode = _xmlLicense.SelectSingleNode("License/Computer/Drive");
			if (xmlNode == null || xmlNode.InnerText.Length == 0)
			{
				return;
			}
			string[] hardDiskSerialNumbers = Computer.GetHardDiskSerialNumbers();
			if (hardDiskSerialNumbers.Length == 0)
			{
				return;
			}
			string[] array = hardDiskSerialNumbers;
			foreach (string b in array)
			{
				if (string.Equals(xmlNode.InnerText.Trim(), b, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception inner)
		{
			throw new LicenseException("Invalid HDD serial number", LicenseErrorNumber.InvalidHardDiskSerialNumber, inner);
		}
		throw new LicenseException("Invalid HDD serial number", LicenseErrorNumber.InvalidHardDiskSerialNumber);
	}

	private void VerifyServerComputerName()
	{
		try
		{
			XmlNode xmlNode = _xmlLicense.SelectSingleNode("License/Computer/Name");
			if (xmlNode == null)
			{
				return;
			}
			string name = Computer.GetName();
			if (string.Equals(xmlNode.InnerText.Trim(), name, StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
		}
		catch (InvalidOperationException)
		{
		}
		catch (ArgumentException)
		{
		}
		throw new LicenseException("Invalid server name", LicenseErrorNumber.InvalidServerName);
	}

	private static AsymmetricAlgorithm GetLicPublicKey()
	{
		string xmlString = "<RSAKeyValue><Modulus>zmwboAV9rGYf4pa2NW/Tfydh62vDm2YQcekVHPfph8n/Range44GtzFOr7gsSiSJ65ro1IwPJLisroXcF/Eg9RRxsPmh8v9gn6+OqEvDhITp07shmv5N2B1Do921ZnknjDBjxo7Fnki+upIJhScsbNyylF0kUkDMljXOIjis3A8=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
		AsymmetricAlgorithm asymmetricAlgorithm = AsymmetricAlgorithm.Create("RSA");
		asymmetricAlgorithm.FromXmlString(xmlString);
		return asymmetricAlgorithm;
	}

	private static void VerifyXmlDocumentSignature(XmlDocument xmldoc)
	{
		AsymmetricAlgorithm licPublicKey = GetLicPublicKey();
		SignedXml signedXml = new SignedXml(xmldoc);
		try
		{
			XmlNode xmlNode = xmldoc.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#")[0];
			signedXml.LoadXml((XmlElement)xmlNode);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			throw new LicenseException("Invalid Signature in XML", LicenseErrorNumber.InvalidSignatureXml);
		}
		if (!signedXml.CheckSignature(licPublicKey))
		{
			throw new LicenseException("Invalid Signature", LicenseErrorNumber.InvalidSignature);
		}
	}

	private void VerifyLicensingMode(XmlDocument xmldoc)
	{
		string xpath = FormattableString.Invariant(FormattableStringFactory.Create("License/Software/Features/Feature[@Name='{0}']", "ChannelLicensing"));
		XmlNode xmlNode = xmldoc.SelectSingleNode(xpath);
		string xpath2 = FormattableString.Invariant(FormattableStringFactory.Create("License/Software/Features/Feature[@Name='{0}']", "HardwareLicensing"));
		XmlNode xmlNode2 = xmldoc.SelectSingleNode(xpath2);
		if (xmlNode != null && xmlNode2 != null)
		{
			throw new LicenseException("Only one licensing mode is allowed", LicenseErrorNumber.UnsupportedLicensingMode);
		}
		if (xmlNode == null && xmlNode2 == null)
		{
			throw new LicenseException("There must be a licensing mode feature flag", LicenseErrorNumber.UnsupportedLicensingMode);
		}
		if (xmlNode != null)
		{
			XmlNode xmlNode3 = xmldoc.SelectSingleNode("License/Whitelist");
			if (xmlNode3 == null)
			{
				throw new LicenseException("Channel based license does not contain an approved device list", LicenseErrorNumber.UnsupportedLicensingMode);
			}
			if (xmlNode3.Attributes?["Version"] == null)
			{
				throw new LicenseException("Approved device list must have an attribute named Version", LicenseErrorNumber.UnsupportedLicensingMode);
			}
			XmlAttribute obj = xmlNode3.Attributes?["MaximumChannelCount"] ?? throw new LicenseException("Approved device list must have an attribute named MaximumChannelCount", LicenseErrorNumber.UnsupportedLicensingMode);
			int result = 1;
			if (!int.TryParse(obj?.Value, out result))
			{
				throw new LicenseException("The attribute MaximumChannelCount in the approved device list must be an integer greater than zero", LicenseErrorNumber.UnsupportedLicensingMode);
			}
			if (result <= 0)
			{
				throw new LicenseException("The attribute MaximumChannelCount in the approved device list must be greater than zero", LicenseErrorNumber.UnsupportedLicensingMode);
			}
			LicenseType = LicenseType.Channel;
		}
		else if (xmlNode2 != null)
		{
			LicenseType = LicenseType.Hardware;
		}
		else
		{
			LicenseType = LicenseType.Unknown;
		}
	}
}
