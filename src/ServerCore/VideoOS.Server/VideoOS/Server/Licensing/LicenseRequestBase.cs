using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.LicenseInfo;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Server.DataComponents;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Utilities.Security;

namespace VideoOS.Server.Licensing;

public abstract class LicenseRequestBase
{
	public sealed class FeatureCollection : Dictionary<string, IFeature>
	{
	}

	public Guid SiteId { get; set; }

	public int HardwareGracePeriod { get; set; }

	public string ExpiryDate { get; set; }

	public int Recorders { get; set; }

	public IFeedDictionary Feeds { get; private set; }

	public IFeedDictionary ICPFeeds { get; private set; }

	public IHardwareDictionary HardwareList { get; private set; }

	public FeatureCollection Features { get; private set; }

	public bool IncludeHardwareIds { get; private set; }

	public Dictionary<string, string> Properties { get; private set; }

	public string MIPLicenseRequest { get; set; }

	public SystemBuilderLicenseRequest SystemBuilderRequest { get; set; }

	protected ILicenseManagerComponentsProvider LicenseManagerComponentsProvider { get; private set; }

	protected ILicenseFeatureProvider LicenseFeatureProvider { get; private set; }

	protected ILicenseDocument LicenseDocument { get; private set; }

	protected IDataComponentFactory DataComponentFactory { get; private set; }

	protected bool LicenseExpires
	{
		get
		{
			if (LicenseDocument != null)
			{
				return LicenseDocument.ExpiryDate != DateTime.MinValue;
			}
			return false;
		}
	}

	protected string ProductName
	{
		get
		{
			if (LicenseDocument != null)
			{
				return LicenseDocument.ProductName;
			}
			return "Invalid license";
		}
	}

	protected string ProductSku
	{
		get
		{
			if (LicenseDocument != null)
			{
				return LicenseDocument.ProductSku;
			}
			return "XPET";
		}
	}

	protected DateTime LicenseExpiryDate
	{
		get
		{
			if (!HasLicense())
			{
				throw new InvalidOperationException();
			}
			return LicenseDocument.ExpiryDate;
		}
	}

	protected IServerLogManager ServerLogManager { get; private set; }

	protected ILicenseDataSetComponent LicenseDataSetComponent { get; private set; }

	public string SLC { get; set; }

	public string SLCEnviroment { get; set; }

	protected IOemResources OemResourcesWrapper { get; set; }

	public LicenseRequestBase(ILicenseManagerComponentsProvider licenseManagerComponentsProvider, ILicenseFeatureProvider licenseFeatureProvider, ILicenseDocument licenseDocument, IDataComponentFactory dataComponentFactory, IServerLogManager serverLogManager, ILicenseDataSetComponent licenseDataSetComponent, bool includeHardwareIds, IOemResources oemResources)
	{
		if (licenseManagerComponentsProvider == null)
		{
			throw new ArgumentNullException("licenseManagerComponentsProvider");
		}
		if (licenseFeatureProvider == null)
		{
			throw new ArgumentNullException("licenseFeatureProvider");
		}
		if (dataComponentFactory == null)
		{
			throw new ArgumentNullException("dataComponentFactory");
		}
		if (licenseDataSetComponent == null)
		{
			throw new ArgumentNullException("licenseDataSetComponent");
		}
		LicenseFeatureProvider = licenseFeatureProvider;
		LicenseManagerComponentsProvider = licenseManagerComponentsProvider;
		DataComponentFactory = dataComponentFactory;
		LicenseDataSetComponent = licenseDataSetComponent;
		LicenseDocument = licenseDocument;
		ServerLogManager = serverLogManager;
		IncludeHardwareIds = includeHardwareIds;
		ExpiryDate = string.Empty;
		HardwareGracePeriod = 0;
		Recorders = 0;
		Feeds = LicenseFactory.CreateNewFeedDictionary();
		ICPFeeds = LicenseFactory.CreateNewFeedDictionary();
		HardwareList = LicenseFactory.CreateNewHardwareDictionary();
		Features = new FeatureCollection();
		Properties = new Dictionary<string, string>();
		OemResourcesWrapper = oemResources;
	}

	private static string GetComputerName()
	{
		return Computer.GetName();
	}

	private static string GetComputerSID()
	{
		try
		{
			return Computer.GetSecurityIdentifier().ToString();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			return string.Empty;
		}
	}

	protected virtual string GetFirstAvailableNetworkAdapterMacAddress()
	{
		try
		{
			string[] networkAdapterMacAddresses = Computer.GetNetworkAdapterMacAddresses();
			if (networkAdapterMacAddresses.Length != 0)
			{
				return networkAdapterMacAddresses[0].ToUpperInvariant();
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
		return "";
	}

	protected virtual string GetFirstAvailableHarddriveSerialNumber()
	{
		return string.Empty;
	}

	protected bool HasLicense()
	{
		return LicenseDocument != null;
	}

	protected void UpdateLicenseRequestServerInfo()
	{
		Site site = Site.QueryHomeSite(DataComponentFactory);
		SiteId = site.Id;
		Properties.Add("SiteName", site.Name);
		if (!HasLicense())
		{
			Properties.Add("ProductName", "Invalid license");
			Properties.Add("ProductSku", "XPET");
			return;
		}
		Properties.Add("ProductName", ProductName);
		Properties.Add("ProductSku", ProductSku);
		if (LicenseExpires)
		{
			ExpiryDate = LicenseExpiryDate.ToString("u", CultureInfo.InvariantCulture);
		}
	}

	protected void UpdateLicenseRequestFeatureInfo()
	{
		if (!HasLicense())
		{
			return;
		}
		IEnumerable<IFeature> enumerable = FeatureInformation.QueryAllLicensedFeatures(LicenseDocument);
		foreach (IFeature item in enumerable)
		{
			IFeature feature = LicenseFactory.CreateNewFeature(item.Name);
			foreach (KeyValuePair<string, string> parameter in item.Parameters)
			{
				feature.Parameters[parameter.Key] = parameter.Value;
			}
			Features[feature.Name] = feature;
		}
	}

	protected void UpdateLicenseRequestSLCInfo()
	{
		if (HasLicense())
		{
			SLC = LicenseDocument.SoftwareLicenseCode;
			SLCEnviroment = LicenseDocument.SoftwareLicenseCodeEnvironment;
		}
	}

	protected void WriteSoftwareInformation(XmlTextWriter request)
	{
		request.WriteStartElement("Software");
		request.WriteStartElement("Product");
		request.WriteAttributeString("Name", Properties["ProductName"]);
		request.WriteAttributeString("Version", AssemblyProductInfo.ProductVersion);
		request.WriteAttributeString("Company", OemResourcesWrapper.App.CompanyName);
		request.WriteAttributeString("Family", "XPCO");
		request.WriteAttributeString("SKU", Properties["ProductSku"]);
		request.WriteEndElement();
		request.WriteStartElement("Features");
		foreach (IFeature value in Features.Values)
		{
			request.WriteStartElement("Feature");
			request.WriteAttributeString("Name", value.Name);
			if (value.Parameters.Count > 0)
			{
				request.WriteStartElement("Parameters");
				foreach (KeyValuePair<string, string> parameter in value.Parameters)
				{
					request.WriteStartElement("Parameter");
					request.WriteAttributeString("Name", parameter.Key);
					request.WriteCData(parameter.Value);
					request.WriteEndElement();
				}
				request.WriteEndElement();
			}
			request.WriteEndElement();
		}
		request.WriteEndElement();
		request.WriteEndElement();
	}

	protected void WriteHardwareGracePeriod(XmlTextWriter request)
	{
		request.WriteElementString("HardwareGracePeriod", HardwareGracePeriod.ToString(CultureInfo.InvariantCulture));
	}

	protected void WriteSLC(XmlTextWriter request)
	{
		request.WriteStartElement("SLC");
		if (!string.IsNullOrEmpty(SLCEnviroment))
		{
			request.WriteAttributeString("Environment", SLCEnviroment);
		}
		request.WriteString(SLC);
		request.WriteEndElement();
	}

	protected void WriteExpiresDate(XmlTextWriter request)
	{
		if (!string.IsNullOrEmpty(ExpiryDate))
		{
			request.WriteElementString("Expires", ExpiryDate);
		}
	}

	protected void WriteSiteInformation(XmlTextWriter request)
	{
		request.WriteStartElement("Site");
		request.WriteAttributeString("Id", SiteId.ToString());
		request.WriteEndElement();
	}

	protected void WriteComputerRequestInformation(XmlTextWriter request)
	{
		request.WriteStartElement("Computer");
		request.WriteElementString("Name", GetComputerName());
		request.WriteElementString("SID", GetComputerSID());
		request.WriteElementString("MAC", GetFirstAvailableNetworkAdapterMacAddress());
		request.WriteElementString("Drive", GetFirstAvailableHarddriveSerialNumber());
		request.WriteEndElement();
	}

	protected void WriteProperties(XmlTextWriter request)
	{
		request.WriteStartElement("Properties");
		foreach (KeyValuePair<string, string> property in Properties)
		{
			request.WriteStartElement("Property");
			request.WriteAttributeString("Name", property.Key);
			request.WriteCData(property.Value);
			request.WriteEndElement();
		}
		request.WriteEndElement();
	}

	protected void WriteRecorderRequestInformation(XmlTextWriter request)
	{
		request.WriteStartElement("Recorders");
		request.WriteAttributeString("Count", Recorders.ToString(CultureInfo.InvariantCulture));
		request.WriteEndElement();
	}

	protected void WriteFeedRequestInformation(XmlTextWriter request)
	{
		WriteFeedRequestInformation(request, Feeds);
	}

	protected void WriteICPFeedRequestInformation(XmlTextWriter request)
	{
		WriteICPFeedRequestInformation(request, ICPFeeds);
	}

	protected abstract void WriteChannelRequestInformation(XmlTextWriter request);

	protected abstract void WriteICPChannelRequestInformation(XmlTextWriter request);

	protected abstract void WriteHardwareRequestInformation(XmlTextWriter request);

	protected abstract void WriteICPRequestInformation(XmlTextWriter request);

	protected void WriteMIPPluginsInformation(XmlTextWriter request)
	{
		if (string.IsNullOrWhiteSpace(MIPLicenseRequest))
		{
			request.WriteElementString("MIPPlugins", string.Empty);
			return;
		}
		int num = MIPLicenseRequest.IndexOf("?>", StringComparison.OrdinalIgnoreCase);
		string data = MIPLicenseRequest;
		if (num != -1)
		{
			data = MIPLicenseRequest.Substring(num + 2);
		}
		request.WriteRaw(data);
	}

	protected string WriteIntegrationInformation()
	{
		return JsonConvert.SerializeObject(GetIntegrationRegistrations());
	}

	protected void WriteSystemBuilderInformation(XmlTextWriter request)
	{
		if (SystemBuilderRequest != null)
		{
			request.WriteStartElement("SystemBuilderManufacturer");
			request.WriteElementString("Name", SystemBuilderRequest.Name);
			request.WriteElementString("SKU", SystemBuilderRequest.Sku);
			request.WriteElementString("GUID", SystemBuilderRequest.PartnerGuid.ToString());
			request.WriteEndElement();
		}
	}

	protected virtual void WriteFeedRequestInformation(XmlTextWriter request, IFeedDictionary feeds)
	{
		WriteFeedsToRequestInformation(request, "Feeds", feeds);
	}

	protected virtual void WriteICPFeedRequestInformation(XmlTextWriter request, IFeedDictionary feeds)
	{
		WriteFeedsToRequestInformation(request, "ICPFeeds", feeds);
	}

	protected virtual void WriteFeedsToRequestInformation(XmlTextWriter request, string feedsName, IFeedDictionary feeds)
	{
		request.WriteStartElement(feedsName);
		foreach (ILicenseFeed value in feeds.Values)
		{
			request.WriteStartElement("Feed");
			request.WriteAttributeString("name", value.Name);
			request.WriteString(value.Count.ToString(CultureInfo.InvariantCulture));
			request.WriteEndElement();
		}
		request.WriteEndElement();
	}

	protected virtual void WritePortsToRequestInformation(XmlTextWriter request, IPortDictionary ports)
	{
		request.WriteStartElement("Ports");
		foreach (IPort value in ports.Values)
		{
			request.WriteStartElement("Port");
			request.WriteAttributeString("Type", value.Name);
			request.WriteAttributeString("Available", value.Count.ToString(CultureInfo.InvariantCulture));
			request.WriteEndElement();
		}
		request.WriteEndElement();
	}

	protected void WriteServerHardwareRequestInformation(XmlTextWriter request, string macAddress)
	{
		if (request == null)
		{
			throw new ArgumentNullException("request");
		}
		if (string.IsNullOrEmpty(macAddress))
		{
			throw new ArgumentNullException("macAddress");
		}
		List<IHardwareInfo> list = HardwareList[macAddress];
		request.WriteStartElement("Recorders");
		foreach (IHardwareInfo item in list)
		{
			request.WriteStartElement("Recorder");
			request.WriteAttributeString("id", item.RecorderId.ToString());
			foreach (KeyValuePair<string, ILicenseFeed> feed in item.Feeds)
			{
				request.WriteStartElement("Channel");
				request.WriteAttributeString("name", feed.Value.Name);
				if (feed.Key.Equals("Camera", StringComparison.Ordinal))
				{
					request.WriteString(string.Join(",", item.EnabledCameraIndexes));
				}
				else
				{
					request.WriteString(feed.Value.Count.ToString(CultureInfo.InvariantCulture));
				}
				request.WriteEndElement();
			}
			request.WriteEndElement();
		}
		request.WriteEndElement();
	}

	private IEnumerable<IntegrationRegistration> GetIntegrationRegistrations()
	{
		Dictionary<Guid, IntegrationRegistration> dictionary = new Dictionary<Guid, IntegrationRegistration>();
		using (IHardwareDriverComponent hardwareDriverComponent = DataComponentFactory.CreateHardwareDriverComponent())
		{
			string value = "Sample Manufacturer";
			HardwareDriverInfo[] source = hardwareDriverComponent.QueryActiveMIPDrivers();
			foreach (HardwareDriverInfo item in source.Where((HardwareDriverInfo d) => d.UseCount > 0))
			{
				if (!item.GroupName.Equals(value))
				{
					if (dictionary.TryGetValue(item.Id, out var value2))
					{
						value2.TypeToInstanceCount[IntegrationType.DeviceDriver] += item.UseCount;
						value2.Versions.Add(item.DriverVersion);
					}
					else
					{
						dictionary.Add(item.Id, new IntegrationRegistration(item.Id, item.Name, item.GroupName, item.DriverVersion, new Dictionary<IntegrationType, int> { 
						{
							IntegrationType.DeviceDriver,
							item.UseCount
						} }, allowIntegrationRegistration: false));
					}
				}
			}
		}
		List<IntegrationRegistration> list = dictionary.Values.ToList();
		using IIntegrationRegistrationsComponent integrationRegistrationsComponent = DataComponentFactory.CreateIntegrationRegistrationsComponent();
		integrationRegistrationsComponent.DeleteIntegrationRegistrationsOlderThan(365);
		list.AddRange(IntegrationRegistrationUtils.ParseRows(integrationRegistrationsComponent.QueryIntegrationRegistrations()));
		return list;
	}
}
