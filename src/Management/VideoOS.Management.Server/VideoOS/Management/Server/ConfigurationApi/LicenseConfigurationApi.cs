using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Xml.Linq;
using VideoOS.Common.Crypto;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.LicenseInfo;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Licensing;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Platform.License;
using VideoOS.Utilities.Sorting;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPILicenseInformationFolder")]
[EntityName("ConfigAPILicenseInformation")]
[EntityName("ConfigAPILicenseInstalledProductFolder")]
[EntityName("ConfigAPILicenseOverviewAllFolder")]
[EntityName("ConfigAPILicenseDetailFolder")]
public class LicenseConfigurationApi : LicenseConfigurationApiObjectBase
{
	public LicenseConfigurationApi(ILicenseDataRepository repository)
		: base(repository)
	{
	}

	public LicenseConfigurationApi()
		: this(null)
	{
	}

	public override ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
	}

	public override ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
	}

	public override ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (parentPath == null)
		{
			throw new ArgumentNullException("parentPath");
		}
		if (methodId == null)
		{
			throw new ArgumentNullException("methodId");
		}
		base.Repository.HasPermission(requestContext, "GENERIC_WRITE");
		switch (methodId)
		{
		case "ActivateLicense":
			if (string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal))
			{
				return ActivateLicense(requestContext, item);
			}
			return CreateItemActivateLicense(requestContext, item, methodId);
		case "RequestLicense":
			if (string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal))
			{
				return RequestLicense(item);
			}
			return CreateItemRequestLicense(item, methodId);
		case "ChangeLicense":
			if (string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal))
			{
				return ApplyLicense(requestContext, item, newSlc: true);
			}
			return CreateItemApplyLicense(item, methodId, newSlc: true);
		case "UpdateLicense":
			if (string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal))
			{
				return ApplyLicense(requestContext, item, newSlc: false);
			}
			return CreateItemApplyLicense(item, methodId, newSlc: false);
		case "StopAutomaticLicenseActivation":
			return StopAutomaticLicenseActivation(requestContext, item);
		default:
			throw new ArgumentException("Unknown methodId:" + methodId);
		}
	}

	public override ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		base.Repository.HasPermission(requestContext, "GENERIC_READ");
		if (path.IsFolder)
		{
			if (path.ItemType == "LicenseInformationFolder")
			{
				return GroupConfigurationItemFactory.CreateFolderItem("LicenseInformationFolder", ConfigurationItemPath.CreateSystemPath());
			}
			string serverId = base.Repository.GetServerId(requestContext);
			if (string.Compare(path.Id, serverId, StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new PathInvalidException("Id must match server id");
			}
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath("LicenseInformation", serverId);
			if (path.ItemType == "LicenseInstalledProductFolder")
			{
				return GroupConfigurationItemFactory.CreateFolderItem("LicenseInstalledProductFolder", parentPath);
			}
			if (path.ItemType == "LicenseOverviewAllFolder")
			{
				return GroupConfigurationItemFactory.CreateFolderItem("LicenseOverviewAllFolder", parentPath);
			}
			if (path.ItemType == "LicenseDetailFolder")
			{
				return GroupConfigurationItemFactory.CreateFolderItem("LicenseDetailFolder", parentPath);
			}
		}
		if (!path.IsFolder && path.ItemType == "LicenseInformation")
		{
			return CreateLicenseInformation(requestContext);
		}
		throw new PathNotFoundException(path.Text);
	}

	public override ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		base.Repository.HasPermission(requestContext, "GENERIC_READ");
		string id = path.Id;
		if (path.IsFolder)
		{
			if (path.ItemType == "LicenseInformationFolder")
			{
				ConfigurationItem configurationItem = CreateLicenseInformation(requestContext);
				return new ConfigurationItem[1] { configurationItem };
			}
			if (path.Id != id)
			{
				throw new PathInvalidException("Id must match server id");
			}
			if (path.ItemType == "LicenseInstalledProductFolder")
			{
				return CreateInstallProducts(requestContext, path);
			}
			if (path.ItemType == "LicenseOverviewAllFolder")
			{
				List<ConfigurationItem> list = new List<ConfigurationItem>();
				List<ConfigurationItem> details = new List<ConfigurationItem>();
				CreateLicenseInformation(requestContext, path, list, details);
				return list.ToArray();
			}
			if (path.ItemType == "LicenseDetailFolder")
			{
				List<ConfigurationItem> overview = new List<ConfigurationItem>();
				List<ConfigurationItem> list2 = new List<ConfigurationItem>();
				CreateLicenseInformation(requestContext, path, overview, list2);
				return list2.ToArray();
			}
			throw new PathNotFoundException(path.Text);
		}
		if (path.ItemType == "LicenseInformation")
		{
			return new List<ConfigurationItem>
			{
				GroupConfigurationItemFactory.CreateFolderItem("LicenseInstalledProductFolder", path),
				GroupConfigurationItemFactory.CreateFolderItem("LicenseOverviewAllFolder", path),
				GroupConfigurationItemFactory.CreateFolderItem("LicenseDetailFolder", path)
			}.ToArray();
		}
		throw new PathNotFoundException(path.Text);
	}

	private VideoOS.Common.Integration.LicenseInfo.LicenseInformation GetLicenseInformation()
	{
		string licenseInformationXml = base.Repository.GetLicenseInformationXml();
		if (licenseInformationXml == null)
		{
			return null;
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add(LicenseDisplayNameKeys.ApplicationNameKey("SmartWall"), OemResources.Instance.VideoWall.ApplicationName);
		dictionary.Add(LicenseDisplayNameKeys.SLCKey("SmartWall"), ConfigurationApiResources.SLCHeaderLabel);
		return VideoOS.Common.Integration.LicenseInfo.LicenseInformation.FromXml(licenseInformationXml, dictionary);
	}

	private ConfigurationItem CreateLicenseInformation(IGeminiRequestContext requestContext)
	{
		ILicenseDocument licenseDocument = base.Repository.QueryLicenseDocument(requestContext);
		VideoOS.Common.Integration.LicenseInfo.LicenseInformation licenseInformation = GetLicenseInformation();
		string serverId = base.Repository.GetServerId(requestContext);
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateSystemFolderPath("LicenseInformationFolder");
		ConfigurationItemPath configurationItemPath2 = ConfigurationItemPath.CreateItemPath("LicenseInformation", serverId);
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.ItemType = "LicenseInformation";
		configurationItem.UIHint = "DirectionHorizontal";
		bool result = false;
		string text = base.Repository.QuerySystemSetting(requestContext, SupportedSystemSettings.EnableAutomaticActivation);
		if (text != null)
		{
			bool.TryParse(text, out result);
		}
		List<Property> list = new List<Property>();
		list.Add(LicensePropertyDefinition.SlcProperty(licenseDocument.SoftwareLicenseCode));
		if (!string.IsNullOrEmpty(licenseDocument.SoftwareLicenseCodeEnvironment))
		{
			list.Add(LicensePropertyDefinition.SlcEnvironmentProperty(licenseDocument.SoftwareLicenseCodeEnvironment));
		}
		list.Add(LicensePropertyDefinition.SkuProperty(licenseDocument.ProductSku));
		list.Add(LicensePropertyDefinition.CareLevelProperty(licenseInformation?.Owner.CareLevel ?? ""));
		list.Add(LicensePropertyDefinition.CareIdProperty(licenseInformation?.Owner.CareId ?? ""));
		list.Add(LicensePropertyDefinition.ActivationAutomaticProperty(result, canEdit: false));
		configurationItem.DisplayName = licenseDocument.ProductName;
		configurationItem.Properties = list.ToArray();
		configurationItem.Path = configurationItemPath2.Text;
		configurationItem.ParentPath = configurationItemPath.Text;
		configurationItem.ChildrenFilled = true;
		List<ConfigurationItem> list2 = new List<ConfigurationItem>
		{
			GroupConfigurationItemFactory.CreateFolderItem("LicenseInstalledProductFolder", configurationItemPath2),
			GroupConfigurationItemFactory.CreateFolderItem("LicenseOverviewAllFolder", configurationItemPath2),
			GroupConfigurationItemFactory.CreateFolderItem("LicenseDetailFolder", configurationItemPath2)
		};
		list2.AddRange(CreateFeatureItems(licenseDocument, configurationItemPath2.Text));
		configurationItem.Children = list2.ToArray();
		configurationItem.MethodIds = new string[5] { "ActivateLicense", "RequestLicense", "ChangeLicense", "UpdateLicense", "StopAutomaticLicenseActivation" };
		return configurationItem;
	}

	private ConfigurationItem[] CreateFeatureItems(ILicenseDocument licenseDocument, string parentPath)
	{
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		foreach (string allFeatureName in licenseDocument.AllFeatureNames)
		{
			ConfigurationItem item = new ConfigurationItem
			{
				ItemCategory = ItemCategories.ChildItem,
				ItemType = "LicenseFeature",
				UIHint = "",
				DisplayName = allFeatureName,
				Path = parentPath,
				ParentPath = parentPath,
				ChildrenFilled = true,
				Children = null,
				Properties = CreateFeatureProperties(licenseDocument, allFeatureName),
				MethodIds = new string[0]
			};
			list.Add(item);
		}
		return list.ToArray();
	}

	private static Property[] CreateFeatureProperties(ILicenseDocument licenseDocument, string featureName)
	{
		List<string> list = new List<string>();
		foreach (string featureParameterName in licenseDocument.GetFeatureParameterNames(featureName))
		{
			string featureParameterValue = licenseDocument.GetFeatureParameterValue(featureName, featureParameterName);
			string item = $"{featureParameterName};{featureParameterValue}";
			list.Add(item);
		}
		if (list.Count == 0)
		{
			return null;
		}
		return new Property[1]
		{
			new Property
			{
				Key = "Parameters",
				ValueArray = list.ToArray(),
				DisplayName = ConfigurationApiResources.PropertyLicenseFeatureParameters,
				IsSettable = false,
				ValueType = "Dictionary",
				ValueTypeInfos = new ValueTypeInfo[1]
				{
					new ValueTypeInfo
					{
						Name = "ArrayValueType",
						Value = "String"
					}
				}
			}
		};
	}

	private ConfigurationItem[] CreateInstallProducts(IGeminiRequestContext requestContext, ConfigurationItemPath parentPath)
	{
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		VideoOS.Common.Integration.LicenseInfo.LicenseInformation licenseInformation = GetLicenseInformation();
		if (licenseInformation == null)
		{
			return list.ToArray();
		}
		LicenseOwnerInformation owner = licenseInformation.Owner;
		if (!string.IsNullOrEmpty(licenseInformation.ProductName))
		{
			list.Add(CreateProductInfo(1, licenseInformation.ProductName, Guid.Empty, licenseInformation.Slc, (licenseInformation.LicenseExpirationDate == DateTime.MaxValue) ? ConfigurationApiResources.LicenseInfoTableUnlimitedLicenses : PropertyTypesHelper.GetValue(licenseInformation.LicenseExpirationDate), (owner.SupportPlanDate == DateTime.MaxValue || owner.SupportPlanDate == DateTime.MinValue) ? ConfigurationApiResources.LicenseDateNotApplicable : PropertyTypesHelper.GetValue(owner.SupportPlanDate), (owner.CarePremiumExpiryDate == DateTime.MaxValue || owner.CarePremiumExpiryDate == DateTime.MinValue) ? ConfigurationApiResources.LicenseDateNotApplicable : PropertyTypesHelper.GetValue(owner.CarePremiumExpiryDate), null, parentPath));
		}
		int num = 50;
		foreach (FeatureInformation item in licenseInformation.Features.OrderBy((FeatureInformation f) => f.DisplayName, new AlphanumericComparatorFast()))
		{
			if (!item.Visible)
			{
				continue;
			}
			foreach (FeatureParameterInformation item2 in item.Parameters.OrderBy((FeatureParameterInformation p) => p.DisplayName, new AlphanumericComparatorFast()))
			{
				list.Add(CreateProductInfo(num++, item.DisplayName, Guid.Empty, item2.Value, (item.ExpiryDate == DateTime.MaxValue) ? ConfigurationApiResources.LicenseInfoTableUnlimitedLicenses : PropertyTypesHelper.GetValue(item.ExpiryDate), (item.ExpiryDate == DateTime.MaxValue) ? ConfigurationApiResources.LicenseInfoTableUnlimitedLicenses : PropertyTypesHelper.GetValue(item.ExpiryDate), "", null, parentPath));
			}
		}
		XDocument xDocument = XDocument.Parse(base.Repository.QueryLicenseDocument(requestContext).MipPluginLicenseXml);
		num = 100;
		foreach (XElement item3 in xDocument.Root.Descendants("plugin"))
		{
			LicenseResponse licenseResponse = CreateLicenseResponse(item3);
			string text = ((licenseResponse.Expire.Year == DateTime.MaxValue.Year) ? ConfigurationApiResources.LicenseInfoTableUnlimitedLicenses : PropertyTypesHelper.GetValue(licenseResponse.Expire));
			list.Add(CreateProductInfo(num++, licenseResponse.Name, licenseResponse.PluginId, licenseResponse.Slc, text, text, "", (licenseResponse.PuAc.Year == DateTime.MaxValue.Year) ? ConfigurationApiResources.LicenseInfoTableUnlimitedLicenses : licenseResponse.PuAc.ToString("yyyy-MM-dd"), parentPath));
		}
		return list.ToArray();
	}

	private static ConfigurationItem CreateProductInfo(int ix, string displayName, Guid pluginId, string slc, string expire, string carePlus, string carePremium, string puac, ConfigurationItemPath parentPath)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath("LicenseInstalledProduct", ix.ToString(CultureInfo.InvariantCulture));
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemCategory = ItemCategories.ChildItem,
			ItemType = "LicenseInstalledProduct",
			UIHint = "DirectionHorizontal",
			DisplayName = displayName,
			Path = configurationItemPath.Text,
			ParentPath = parentPath.Text
		};
		List<Property> list = new List<Property>
		{
			LicensePropertyDefinition.ProductDisplayNameProperty(displayName),
			LicensePropertyDefinition.PluginIdProperty(pluginId),
			LicensePropertyDefinition.SlcProperty(slc ?? ""),
			LicensePropertyDefinition.ExpireDateProperty(expire),
			LicensePropertyDefinition.CarePlusProperty(carePlus),
			LicensePropertyDefinition.CarePremiumProperty(carePremium)
		};
		if (puac != null)
		{
			list.Add(LicensePropertyDefinition.PurchaseActivationDateProperty(puac));
		}
		obj.Properties = list.ToArray();
		obj.ChildrenFilled = true;
		return obj;
	}

	private void CreateLicenseInformation(IGeminiRequestContext requestContext, ConfigurationItemPath parentPath, List<ConfigurationItem> overview, List<ConfigurationItem> details)
	{
		VideoOS.Common.Integration.LicenseInfo.LicenseInformation licenseInformation = GetLicenseInformation();
		if (licenseInformation != null)
		{
			switch (licenseInformation.LicenseType)
			{
			case LicenseType.Hardware:
				AddHardwareLicenseInformation(licenseInformation, parentPath, overview, details);
				AddFeedLicenseInformation(licenseInformation, parentPath, details);
				break;
			default:
				AddChannelLicenseInformation(licenseInformation, parentPath, overview, details);
				break;
			}
			if (licenseInformation.Features.Any((FeatureInformation f) => string.Equals(f.Name, "InterconnectedProducts", StringComparison.Ordinal)))
			{
				AddICPFeedLicenseInformation(licenseInformation, parentPath, overview, details);
			}
			AddMIPLicenseInformation(requestContext, parentPath, overview, details);
		}
	}

	private void AddMIPLicenseInformation(IGeminiRequestContext requestContext, ConfigurationItemPath parentPath, List<ConfigurationItem> overview, List<ConfigurationItem> details)
	{
		string text = base.Repository.QueryMIPLicenseItem(requestContext);
		if (text == null)
		{
			return;
		}
		XDocument xDocument = XDocument.Parse(base.Repository.QueryLicenseDocument(requestContext).MipPluginLicenseXml);
		int ix = 100;
		foreach (XElement item in XDocument.Parse(text).Root.Descendants("plugin"))
		{
			VideoOS.Platform.License.LicenseInformation licenseInformation = CreateLicenseInformation(item);
			LicenseResponse licenseResponse = CreateLicenseResponse((from x in xDocument.Root.Descendants("plugin")
				where x.Element("pluginid").Value == licenseInformation.PluginId.ToString() && x.Element("licensetype").Value == licenseInformation.LicenseType
				select x).FirstOrDefault());
			licenseInformation.TrialMode = licenseResponse == null;
			ResolvedLicenseCount resolvedLicenseCount = new ResolvedLicenseCount(licenseResponse, licenseInformation);
			details.Add(CreateSiteDetailItem(parentPath, ix, licenseInformation.PluginId, licenseInformation.LicenseType, resolvedLicenseCount.Activated.ToString(CultureInfo.InvariantCulture), ConfigurationApiResources.LicenseInfoNotApplicable, resolvedLicenseCount.Temporary.ToString(NumberFormatInfo.CurrentInfo), resolvedLicenseCount.Expired.ToString(NumberFormatInfo.CurrentInfo), resolvedLicenseCount.Missing.ToString(CultureInfo.InvariantCulture), "", ""));
			string activated = string.Format(CultureInfo.CurrentCulture, ConfigurationApiResources.XOutOfYPattern, resolvedLicenseCount.TotalActivated.ToString(NumberFormatInfo.CurrentInfo), resolvedLicenseCount.Available.HasValue ? resolvedLicenseCount.Available.Value.ToString(NumberFormatInfo.CurrentInfo) : ConfigurationApiResources.LicenseInfoNotApplicable);
			overview.Add(CreateSiteOverviewItem(parentPath, ix++, licenseInformation.PluginId, licenseInformation.LicenseType, activated));
		}
	}

	private static VideoOS.Platform.License.LicenseInformation CreateLicenseInformation(XElement pluginElement)
	{
		string input = pluginElement.Element("pluginid")?.Value;
		string licenseType = pluginElement.Element("licensetype")?.Value;
		string name = pluginElement.Element("name")?.Value;
		string s = pluginElement.Element("count")?.Value;
		string s2 = pluginElement.Element("expire")?.Value;
		string customData = pluginElement.Element("customdata")?.Value;
		Collection<LicenseItem> itemIdentifications = LoadMipLicenseItems(pluginElement.Element("items"));
		if (!int.TryParse(s, out var result))
		{
			result = 0;
		}
		if (!DateTime.TryParse(s2, out var result2))
		{
			result2 = DateTime.MaxValue;
		}
		Guid pluginId = Guid.Parse(input);
		return new VideoOS.Platform.License.LicenseInformation
		{
			Counter = result,
			CounterMethod = VideoOS.Platform.License.LicenseInformation.CounterMethodEnum.Auto,
			CustomData = customData,
			Expire = result2,
			ItemIdentifications = itemIdentifications,
			LicenseType = licenseType,
			Name = name,
			PluginId = pluginId
		};
	}

	private static LicenseResponse CreateLicenseResponse(XElement pluginElement)
	{
		if (pluginElement == null)
		{
			return null;
		}
		string input = pluginElement.Element("pluginid")?.Value;
		string licenseType = pluginElement.Element("licensetype")?.Value;
		string name = pluginElement.Element("name")?.Value;
		string s = pluginElement.Element("count")?.Value;
		string s2 = pluginElement.Element("expire")?.Value;
		string customData = pluginElement.Element("customdata")?.Value;
		string slc = pluginElement.Element("slc")?.Value;
		string s3 = pluginElement.Element("activated")?.Value;
		string s4 = pluginElement.Element("totalactivated")?.Value;
		string s5 = pluginElement.Element("puac")?.Value;
		Collection<LicenseItem> itemIdentifications = LoadMipLicenseItems(pluginElement.Element("items"));
		if (!int.TryParse(s, out var result))
		{
			result = 0;
		}
		if (!DateTime.TryParseExact(s2, "yyyy-MM-dd", null, DateTimeStyles.None, out var result2))
		{
			result2 = DateTime.MaxValue;
		}
		result2 = new DateTime(result2.Ticks, DateTimeKind.Utc);
		Guid pluginId = Guid.Parse(input);
		if (!int.TryParse(s3, out var result3))
		{
			result3 = 0;
		}
		if (!int.TryParse(s4, out var result4))
		{
			result4 = 0;
		}
		if (!DateTime.TryParseExact(s5, "yyyy-MM-dd", null, DateTimeStyles.None, out var result5))
		{
			result5 = DateTime.MaxValue;
		}
		result5 = new DateTime(result5.Ticks, DateTimeKind.Utc);
		return new LicenseResponse
		{
			Counter = result,
			CounterMethod = VideoOS.Platform.License.LicenseInformation.CounterMethodEnum.Auto,
			CustomData = customData,
			Expire = result2,
			ItemIdentifications = itemIdentifications,
			LicenseType = licenseType,
			Name = name,
			PluginId = pluginId,
			Activated = result3,
			Slc = slc,
			TotalActivated = result4,
			PuAc = result5
		};
	}

	private static Collection<LicenseItem> LoadMipLicenseItems(XElement xElement)
	{
		Collection<LicenseItem> collection = new Collection<LicenseItem>();
		if (xElement != null)
		{
			foreach (XElement item in xElement.Elements())
			{
				string id = item.Element("id")?.Value;
				string value = item.Element("itemTrial")?.Value;
				string s = item.Element("itemTrialEnd")?.Value;
				string name = item.Element("name")?.Value;
				if (!bool.TryParse(value, out var result))
				{
					result = false;
				}
				if (!DateTime.TryParse(s, out var result2))
				{
					result2 = DateTime.MaxValue;
				}
				collection.Add(new LicenseItem
				{
					Id = id,
					ItemTrial = result,
					ItemTrialEnd = result2,
					Name = name
				});
			}
		}
		return collection;
	}

	private static void AddHardwareLicenseInformation(VideoOS.Common.Integration.LicenseInfo.LicenseInformation licenseInfo, ConfigurationItemPath parentPath, List<ConfigurationItem> overview, List<ConfigurationItem> details)
	{
		HardwareLicenseInformation hardwareLicenses = licenseInfo.HardwareLicenses;
		string changesWithout = FormatReplacedLicenses(hardwareLicenses);
		details.Add(CreateSiteDetailItem(parentPath, 1, Guid.Empty, ConfigurationApiResources.LicenseType_Hardware, hardwareLicenses.Used.ToString(CultureInfo.InvariantCulture), changesWithout, hardwareLicenses.Temporary.ToString(CultureInfo.InvariantCulture), hardwareLicenses.Expired.ToString(CultureInfo.InvariantCulture), hardwareLicenses.Missing.ToString(CultureInfo.InvariantCulture), "", ""));
		string activated = string.Format(CultureInfo.CurrentCulture, ConfigurationApiResources.XOutOfYPattern, LicenseCountToDisplayString(licenseInfo.TotalActivated.HardwareCount), AvailableLicensesDisplayString(hardwareLicenses.Available));
		overview.Add(CreateSiteOverviewItem(parentPath, 1, Guid.Empty, ConfigurationApiResources.LicenseType_Hardware, activated));
	}

	private static void AddChannelLicenseInformation(VideoOS.Common.Integration.LicenseInfo.LicenseInformation licenseInfo, ConfigurationItemPath parentPath, List<ConfigurationItem> overview, List<ConfigurationItem> details)
	{
		HardwareLicenseInformation hardwareLicenses = licenseInfo.HardwareLicenses;
		string changesWithout = FormatReplacedLicenses(hardwareLicenses);
		details.Add(CreateSiteDetailItem(parentPath, 1, Guid.Empty, ConfigurationApiResources.LicenseType_Channel, hardwareLicenses.Used.ToString(CultureInfo.InvariantCulture), changesWithout, hardwareLicenses.Temporary.ToString(CultureInfo.InvariantCulture), hardwareLicenses.Expired.ToString(CultureInfo.InvariantCulture), hardwareLicenses.Missing.ToString(CultureInfo.InvariantCulture), (hardwareLicenses.FirstGraceExpirationDate == DateTime.MaxValue) ? ConfigurationApiResources.LicenseInfoTableUnlimitedLicenses : PropertyTypesHelper.GetValue(hardwareLicenses.FirstGraceExpirationDate), ""));
		string activated = string.Format(CultureInfo.CurrentCulture, ConfigurationApiResources.XOutOfYPattern, LicenseCountToDisplayString(licenseInfo.TotalActivated.HardwareCount), AvailableLicensesDisplayString(hardwareLicenses.Available));
		overview.Add(CreateSiteOverviewItem(parentPath, 1, Guid.Empty, ConfigurationApiResources.LicenseType_Channel, activated));
	}

	private static string FormatReplacedLicenses(HardwareLicenseInformation hardwareLicenseInfo)
	{
		string empty = string.Empty;
		if (hardwareLicenseInfo.TotalReplacementLimit == int.MaxValue)
		{
			return hardwareLicenseInfo.Replaced.ToString(CultureInfo.CurrentCulture);
		}
		return string.Format(CultureInfo.CurrentCulture, ConfigurationApiResources.XOutOfYPattern, hardwareLicenseInfo.Replaced, hardwareLicenseInfo.TotalReplacementLimit);
	}

	private static void AddFeedLicenseInformation(VideoOS.Common.Integration.LicenseInfo.LicenseInformation licenseInfo, ConfigurationItemPath parentPath, List<ConfigurationItem> details)
	{
		int num = 2;
		foreach (FeedLicenseInformation feedLicense in licenseInfo.FeedLicenses)
		{
			if (feedLicense.Available == int.MaxValue && feedLicense.Expired == 0 && feedLicense.Missing == 0)
			{
				break;
			}
			string text = ConfigurationApiResources.ResourceManager.GetString("FeedType_" + feedLicense.TypeName, CultureInfo.CurrentUICulture);
			if (text == null)
			{
				text = feedLicense.TypeName;
			}
			details.Add(CreateSiteDetailItem(parentPath, num++, Guid.Empty, text, feedLicense.Used.ToString(CultureInfo.InvariantCulture), ConfigurationApiResources.LicenseInfoNotApplicable, feedLicense.Temporary.ToString(CultureInfo.InvariantCulture), feedLicense.Expired.ToString(CultureInfo.InvariantCulture), feedLicense.Missing.ToString(CultureInfo.InvariantCulture), "", ""));
		}
	}

	private static void AddICPFeedLicenseInformation(VideoOS.Common.Integration.LicenseInfo.LicenseInformation licenseInfo, ConfigurationItemPath parentPath, List<ConfigurationItem> overview, List<ConfigurationItem> details)
	{
		int ix = 50;
		foreach (FeedLicenseInformation iCPFeedLicense in licenseInfo.ICPFeedLicenses)
		{
			if ((iCPFeedLicense.Available == int.MaxValue || iCPFeedLicense.Available == 0) && iCPFeedLicense.Expired == 0 && iCPFeedLicense.Missing == 0)
			{
				break;
			}
			string text = ConfigurationApiResources.ResourceManager.GetString("Feature_FeedType_" + iCPFeedLicense.TypeName, CultureInfo.CurrentUICulture);
			if (text == null)
			{
				text = iCPFeedLicense.TypeName;
			}
			string licenseType = string.Format(CultureInfo.CurrentCulture, text, OemResources.Instance.Server.ICPApplicationName);
			details.Add(CreateSiteDetailItem(parentPath, ix, Guid.Empty, licenseType, iCPFeedLicense.Used.ToString(CultureInfo.InvariantCulture), ConfigurationApiResources.LicenseInfoNotApplicable, iCPFeedLicense.Temporary.ToString(CultureInfo.InvariantCulture), iCPFeedLicense.Expired.ToString(CultureInfo.InvariantCulture), iCPFeedLicense.Missing.ToString(CultureInfo.InvariantCulture), "", ""));
			string activated = string.Format(CultureInfo.CurrentCulture, ConfigurationApiResources.XOutOfYPattern, LicenseCountToDisplayString(licenseInfo.TotalActivated.ICPCameraCount), AvailableLicensesDisplayString(iCPFeedLicense.Available));
			overview.Add(CreateSiteOverviewItem(parentPath, ix++, Guid.Empty, licenseType, activated));
		}
	}

	private static ConfigurationItem CreateSiteOverviewItem(ConfigurationItemPath parentPath, int ix, Guid pluginId, string licenseType, string activated)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath("LicenseOverviewAll", ix.ToString(CultureInfo.InvariantCulture));
		return new ConfigurationItem
		{
			ItemCategory = ItemCategories.ChildItem,
			ItemType = "LicenseOverviewAll",
			UIHint = "DirectionHorizontal",
			DisplayName = licenseType,
			Path = configurationItemPath.Text,
			ParentPath = parentPath.Text,
			Properties = new List<Property>
			{
				LicensePropertyDefinition.PluginIdProperty(pluginId),
				LicensePropertyDefinition.LicenseTypeProperty(licenseType),
				LicensePropertyDefinition.ActivatedProperty(activated)
			}.ToArray(),
			ChildrenFilled = true
		};
	}

	private static ConfigurationItem CreateSiteDetailItem(ConfigurationItemPath parentPath, int ix, Guid pluginId, string licenseType, string activated, string changesWithout, string inGraceCount, string graceExpiredCount, string notLicensedCount, string firstGraceExpirationDate, string note)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath("LicenseDetail", ix.ToString(CultureInfo.InvariantCulture));
		return new ConfigurationItem
		{
			ItemCategory = ItemCategories.ChildItem,
			ItemType = "LicenseDetail",
			UIHint = "DirectionHorizontal",
			DisplayName = licenseType,
			Path = configurationItemPath.Text,
			ParentPath = parentPath.Text,
			Properties = new List<Property>
			{
				LicensePropertyDefinition.PluginIdProperty(pluginId),
				LicensePropertyDefinition.LicenseTypeProperty(licenseType),
				LicensePropertyDefinition.ActivatedProperty(activated),
				LicensePropertyDefinition.ChangesWithoutActivationProperty(changesWithout),
				LicensePropertyDefinition.InGraceProperty(inGraceCount),
				LicensePropertyDefinition.GraceExpiredProperty(graceExpiredCount),
				LicensePropertyDefinition.NotLicensedProperty(notLicensedCount),
				LicensePropertyDefinition.FirstGraceExpirationDateProperty(firstGraceExpirationDate),
				LicensePropertyDefinition.NoteProperty(note)
			}.ToArray(),
			ChildrenFilled = true
		};
	}

	private static string AvailableLicensesDisplayString(int available)
	{
		if (available != int.MaxValue)
		{
			return available.ToString(CultureInfo.InvariantCulture);
		}
		return ConfigurationApiResources.LicenseInfoTableUnlimitedLicenses;
	}

	private static string LicenseCountToDisplayString(int count)
	{
		if (count != int.MaxValue)
		{
			return count.ToString(CultureInfo.InvariantCulture);
		}
		return ConfigurationApiResources.LicenseInfoTableUnlimitedLicenses;
	}

	public ConfigurationItem CreateItemActivateLicense(IGeminiRequestContext requestContext, ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		bool result = false;
		string text = base.Repository.QuerySystemSetting(requestContext, SupportedSystemSettings.EnableAutomaticActivation);
		if (text != null)
		{
			bool.TryParse(text, out result);
		}
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardActivateLicense;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		List<Property> list = new List<Property>();
		configurationItem.MethodIds = new string[1] { methodId };
		list.Add(LicensePropertyDefinition.ActivationUserNameProperty());
		list.Add(LicensePropertyDefinition.ActivationPasswordProperty());
		list.Add(LicensePropertyDefinition.ActivationAutomaticProperty(result, canEdit: true));
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private ConfigurationItem ActivateLicense(IGeminiRequestContext requestContext, ConfigurationItem configurationItem)
	{
		string value = null;
		string value2 = null;
		((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("UserName", out value);
		((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("Password", out value2);
		ValidateInternal(configurationItem, out var auto);
		string text = base.Repository.QuerySystemSetting(requestContext, SupportedSystemSettings.EnableAutomaticActivation);
		if ((text != null && bool.Parse(text)) != auto)
		{
			base.Repository.SetSystemSetting(requestContext, SupportedSystemSettings.EnableAutomaticActivation, auto.ToString(CultureInfo.InvariantCulture));
		}
		if (!auto)
		{
			base.Repository.SetSystemSetting(requestContext, SupportedSystemSettings.ActivationCredentials, string.Empty);
		}
		if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(value2))
		{
			string text2 = base.Repository.QuerySystemSetting(requestContext, SupportedSystemSettings.ActivationCredentials);
			if (string.IsNullOrWhiteSpace(text2))
			{
				throw new PropertyInvalidException("Username and password is required when not already saved previously");
			}
			ActivationCredentials activationCredentials = ActivationCredentials.Parse(text2);
			value = activationCredentials.UserName;
			value2 = activationCredentials.Password;
		}
		else if (auto)
		{
			ActivationCredentials activationCredentials = new ActivationCredentials(value, value2);
			base.Repository.SetSystemSetting(requestContext, SupportedSystemSettings.ActivationCredentials, activationCredentials.Xml);
		}
		return TaskHelperFunctions.CreateTaskInvokeResultConfigurationItem(base.Repository.CreateLicenseActivateTask(requestContext, value, value2), configurationItem);
	}

	private static void ValidateInternal(ConfigurationItem configurationItem, out bool auto)
	{
		string value = null;
		((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("ActivationAutomatic", out value);
		if (!bool.TryParse(value, out auto))
		{
			throw new PropertyInvalidException("Expected True or False value");
		}
	}

	public static ConfigurationItem CreateItemRequestLicense(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardRequestLicense;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		List<Property> list = new List<Property>();
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	public static ConfigurationItem CreateItemApplyLicense(ConfigurationItem item, string methodId, bool newSlc)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = (newSlc ? ConfigurationApiResources.WizardChangeLicense : ConfigurationApiResources.WizardUpdateLicense);
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		List<Property> list = new List<Property>();
		configurationItem.MethodIds = new string[1] { methodId };
		list.Add(LicensePropertyDefinition.LicenseProperty(""));
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private ConfigurationItem RequestLicense(ConfigurationItem configurationItem)
	{
		byte[] inArray = base.Repository.RequestLicense();
		ConfigurationItem configurationItem2 = CreateInvokeResult(configurationItem, ConfigurationApiResources.LicenseRequest);
		configurationItem2.Properties = new List<Property> { LicensePropertyDefinition.LicenseProperty(Convert.ToBase64String(inArray)) }.ToArray();
		return configurationItem2;
	}

	private ConfigurationItem ApplyLicense(IGeminiRequestContext requestContext, ConfigurationItem configurationItem, bool newSlc)
	{
		if (configurationItem.Properties == null)
		{
			throw new ArgumentNullException("configurationItem", "Properties cannot be null");
		}
		Property property = configurationItem.Properties.FirstOrDefault((Property p) => p.Key == "License");
		if (property == null && string.IsNullOrEmpty(property.Value))
		{
			throw new PropertyInvalidException("No license content available");
		}
		VideoOS.Common.Crypto.Keys.SiteSpecificPrivatePublicKey = base.Repository.QuerySystemSetting(requestContext, "LicenseRsaKey");
		try
		{
			byte[] array = Convert.FromBase64String(property.Value);
			ILicenseDocument licenseDocument = LicenseFactory.CreateLicenseLoader().ImportLicenseFile(array);
			string softwareLicenseCode = licenseDocument.SoftwareLicenseCode;
			Version version = base.Repository.GetVersion(requestContext);
			softwareLicenseCode = softwareLicenseCode.Trim().ToUpperInvariant();
			SLC.Validate(softwareLicenseCode, OemResources.Instance.App.VendorCode, version.Major, version.Minor);
			string licenseInformationXml = base.Repository.GetLicenseInformationXml();
			Dictionary<string, string> displayNames = new Dictionary<string, string>();
			VideoOS.Common.Integration.LicenseInfo.LicenseInformation licenseInformation = VideoOS.Common.Integration.LicenseInfo.LicenseInformation.FromXml(licenseInformationXml, displayNames);
			if (licenseInformation.LicenseState == LicenseState.Valid && !string.Equals(licenseInformation.Slc, softwareLicenseCode, StringComparison.OrdinalIgnoreCase) && !newSlc)
			{
				throw new PropertyInvalidException("UpdateLicense method expect same SLC");
			}
			string customerData = licenseDocument.CustomerData;
			string sites = licenseDocument.Sites;
			base.Repository.UpdateLicense(requestContext, array, customerData, sites);
			return MethodTypesHelper.CreateInvokeResult(configurationItem, null, "Result", "Success", true);
		}
		catch (PropertyInvalidException)
		{
			throw;
		}
		catch (FaultException ex2)
		{
			Property[] resultProperties = new Property[2]
			{
				new Property("ErrorText", ex2.Message),
				new Property("ErrorCode", ex2.Code.Name)
			};
			return MethodTypesHelper.CreateInvokeResult(configurationItem, null, "Result", "Error", includeItemPropertiesAndChildren: true, resultProperties);
		}
		catch (Exception ex3)
		{
			Property[] resultProperties2 = new Property[1]
			{
				new Property("ErrorText", ex3.Message)
			};
			return MethodTypesHelper.CreateInvokeResult(configurationItem, null, "Result", "Error", includeItemPropertiesAndChildren: true, resultProperties2);
		}
	}

	private ConfigurationItem StopAutomaticLicenseActivation(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		base.Repository.HasPermission(requestContext, "GENERIC_WRITE");
		bool result = false;
		string text = base.Repository.QuerySystemSetting(requestContext, SupportedSystemSettings.EnableAutomaticActivation);
		if (text != null && bool.TryParse(text, out result) && result)
		{
			base.Repository.SetSystemSetting(requestContext, SupportedSystemSettings.EnableAutomaticActivation, false.ToString(CultureInfo.InvariantCulture));
		}
		return MethodTypesHelper.CreateInvokeResult(item, null, ConfigurationApiResources.MethodResultStopAutomaticLicenseActivation, "Success", true);
	}

	private static ConfigurationItem CreateInvokeResult(ConfigurationItem item, string displayName)
	{
		return new ConfigurationItem
		{
			ItemType = "InvokeResult",
			DisplayName = displayName,
			UIHint = "",
			ItemCategory = ItemCategories.Item,
			ParentPath = item.ParentPath,
			Path = item.Path,
			MethodIds = new string[0]
		};
	}
}
