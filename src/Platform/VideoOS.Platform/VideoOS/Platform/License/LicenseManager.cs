using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Xml;
using VideoOS.Platform.Login;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.License;

public class LicenseManager
{
	private class LicenseInformationInternal
	{
		public LicenseInformation LicenseInformation { get; set; }

		public bool SavedByAdmin { get; set; }
	}

	private ReservedLicenseManager _reservedLicenseManager;

	private ConcurrentLicenseManager _concurrentLicenseManager;

	private OemResourceManager _oemResourceManager;

	private Dictionary<string, DateTime> _expirationDates;

	private string _cudaPublicKey;

	public Configuration _configuration;

	private static Guid _licenseKind = new Guid("E145597A-ECAF-40A1-99E6-14BB9D45A09E");

	private static Guid _licensePluginDefinitionId = new Guid("E145597A-ECAF-40A1-99E6-14BB9D45A09E");

	private Dictionary<Guid, Dictionary<string, LicenseResponse>> _activatedLicenses = new Dictionary<Guid, Dictionary<string, LicenseResponse>>();

	private Dictionary<Guid, Dictionary<string, LicenseInformationInternal>> _pluginLicensesInformations = new Dictionary<Guid, Dictionary<string, LicenseInformationInternal>>();

	private bool _licenseLoaded;

	private bool _registrationItemDirty;

	private object _lockObject = new object();

	private Item _licenseItem;

	private Item _registrationItem;

	private List<string> _features = new List<string>();

	private string _SLC = "";

	private string _productionEnvironment = "";

	private DateTime _expire = DateTime.MinValue;

	private int _productCode;

	private static string SavedByAdminConfigItemId = new Guid("8C983864-1FC1-4C85-8E4C-01BBC1E9EF38").ToString();

	public List<string> Features
	{
		get
		{
			if (!_licenseLoaded)
			{
				LoadLicenseInformation();
			}
			return _features;
		}
	}

	public string SLC
	{
		get
		{
			if (!_licenseLoaded)
			{
				LoadLicenseInformation();
			}
			return _SLC;
		}
	}

	public string ProductionEnvironment
	{
		get
		{
			if (!_licenseLoaded)
			{
				LoadLicenseInformation();
			}
			return _productionEnvironment;
		}
	}

	public DateTime Expire
	{
		get
		{
			if (!_licenseLoaded)
			{
				LoadLicenseInformation();
			}
			return _expire;
		}
	}

	public Dictionary<string, DateTime> GetExpirationDateTimes => _expirationDates;

	public int ProductCode
	{
		get
		{
			if (!_licenseLoaded)
			{
				LoadLicenseInformation();
			}
			return _productCode;
		}
	}

	public string CudaPublicKey
	{
		get
		{
			if (!_licenseLoaded)
			{
				LoadLicenseInformation();
			}
			return _cudaPublicKey;
		}
	}

	public ConcurrentLicenseManager ConcurrentLicenseManager => _concurrentLicenseManager;

	public ReservedLicenseManager ReservedLicenseManager => _reservedLicenseManager;

	public OemResourceManager OemResourceManager => _oemResourceManager;

	public bool Dirty
	{
		get
		{
			return _registrationItemDirty;
		}
		set
		{
			_registrationItemDirty = value;
		}
	}

	public LicenseManager()
	{
		_reservedLicenseManager = new ReservedLicenseManager();
		_concurrentLicenseManager = new ConcurrentLicenseManager();
		_oemResourceManager = new OemResourceManager();
		_expirationDates = new Dictionary<string, DateTime>();
		_configuration = Configuration.Instance;
	}

	public LicenseManager(UserContext userContext)
	{
		_reservedLicenseManager = new ReservedLicenseManager();
		_concurrentLicenseManager = new ConcurrentLicenseManager();
		_oemResourceManager = new OemResourceManager();
		_expirationDates = new Dictionary<string, DateTime>();
		_configuration = userContext.Configuration;
	}

	public Collection<LicenseInformation> GetLicense(Guid pluginid)
	{
		Collection<LicenseInformation> collection = new Collection<LicenseInformation>();
		try
		{
			LoadLicenseInformation();
			if (_activatedLicenses.ContainsKey(pluginid))
			{
				foreach (LicenseResponse value in _activatedLicenses[pluginid].Values)
				{
					collection.Add(value.Clone());
					EnvironmentManager.Instance.Log(error: false, "LicenseManager", string.Format(CultureInfo.InvariantCulture, "GetLicense - Name={0}, Count={1}, Type={2}", value.Name, value.Counter, value.LicenseType));
				}
			}
			return collection;
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionDialog("FindPluginLicense", exception);
			return collection;
		}
	}

	public LicenseInformation GetLicense(Guid pluginid, string licenseType)
	{
		try
		{
			LoadLicenseInformation();
			if (_activatedLicenses.ContainsKey(pluginid) && _activatedLicenses[pluginid].ContainsKey(licenseType))
			{
				LicenseInformation licenseInformation = _activatedLicenses[pluginid][licenseType].Clone();
				EnvironmentManager.Instance.Log(error: false, "LicenseManager", string.Format(CultureInfo.InvariantCulture, "GetLicense - Name={0}, Count={1}, Type={2}", licenseInformation.Name, licenseInformation.Counter, licenseInformation.LicenseType));
				return licenseInformation;
			}
			return null;
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionDialog("FindPluginLicense", exception);
		}
		return null;
	}

	public List<LicenseInformation> GetLicenses()
	{
		List<LicenseInformation> list = new List<LicenseInformation>();
		try
		{
			LoadLicenseInformation();
			foreach (Guid key in _activatedLicenses.Keys)
			{
				list.AddRange(_activatedLicenses[key].Values.Select((LicenseResponse li) => li.Clone()));
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionDialog("FindPluginLicense", exception);
		}
		return list;
	}

	public DateTime GetExpirationDate(string feature)
	{
		if (_expirationDates.ContainsKey(feature))
		{
			return new DateTime(_expirationDates[feature].Ticks, _expirationDates[feature].Kind);
		}
		return DateTime.MaxValue;
	}

	public LicenseManager(UserContext userContext, Item licenseItem, Item registrationItem)
	{
		_reservedLicenseManager = new ReservedLicenseManager();
		_concurrentLicenseManager = new ConcurrentLicenseManager();
		_oemResourceManager = new OemResourceManager();
		_expirationDates = new Dictionary<string, DateTime>();
		_configuration = userContext.Configuration;
		UpdateTables(licenseItem, registrationItem, GetCompatibilityHandler());
	}

	private void LoadLicenseInformation()
	{
		lock (_lockObject)
		{
			if (_licenseLoaded)
			{
				return;
			}
			try
			{
				if (_registrationItemDirty && _registrationItem != null)
				{
					Save();
				}
				if (_configuration == null)
				{
					_configuration = Configuration.Instance;
				}
				ILicenseCompatibilityHandler compatibilityHandler = GetCompatibilityHandler();
				Item itemConfiguration = _configuration.GetItemConfiguration(_licensePluginDefinitionId, _licenseKind, compatibilityHandler.LicensesObjectId);
				Item itemConfiguration2 = _configuration.GetItemConfiguration(_licensePluginDefinitionId, _licenseKind, compatibilityHandler.RegistrationsObjectId);
				if (itemConfiguration == null && itemConfiguration2 == null)
				{
					_configuration.RefreshConfiguration(_licensePluginDefinitionId);
					itemConfiguration = _configuration.GetItemConfiguration(_licensePluginDefinitionId, _licenseKind, compatibilityHandler.LicensesObjectId);
					itemConfiguration2 = _configuration.GetItemConfiguration(_licensePluginDefinitionId, _licenseKind, compatibilityHandler.RegistrationsObjectId);
				}
				UpdateTables(itemConfiguration, itemConfiguration2, compatibilityHandler);
				if (itemConfiguration == null && itemConfiguration2 == null)
				{
					return;
				}
			}
			catch (Exception exception)
			{
				EnvironmentManager.Instance.ExceptionHandler("License Load", exception);
			}
			_licenseLoaded = true;
		}
	}

	private void UpdateTables(Item licenseItem, Item registrationItem, ILicenseCompatibilityHandler compatibilityHandler)
	{
		_registrationItem = registrationItem;
		_licenseItem = licenseItem;
		_activatedLicenses = new Dictionary<Guid, Dictionary<string, LicenseResponse>>();
		_pluginLicensesInformations = new Dictionary<Guid, Dictionary<string, LicenseInformationInternal>>();
		_features = new List<string>();
		if (_licenseItem != null)
		{
			XmlDocument xmlDocument = new XmlDocument();
			if (_licenseItem.Properties.ContainsKey("licenses"))
			{
				string cipherText = _licenseItem.Properties["licenses"];
				cipherText = compatibilityHandler.Decrypt(cipherText);
				xmlDocument.LoadXml(cipherText);
				XmlNode xmlNode = xmlDocument.SelectSingleNode("MIPPlugins");
				if (xmlNode != null)
				{
					XmlNodeList xmlNodeList = xmlNode.SelectNodes("plugin");
					if (xmlNodeList != null)
					{
						foreach (XmlNode item2 in xmlNodeList)
						{
							XmlNode xmlNode3 = item2.SelectSingleNode("pluginid");
							Guid guid = new Guid(xmlNode3.InnerText);
							LicenseResponse licenseResponseFromXml = GetLicenseResponseFromXml(guid, item2);
							if (!_activatedLicenses.ContainsKey(guid))
							{
								_activatedLicenses.Add(guid, new Dictionary<string, LicenseResponse>());
							}
							_activatedLicenses[guid][licenseResponseFromXml.LicenseType] = licenseResponseFromXml;
							EnvironmentManager.Instance.Log(error: false, "LicenseManager", string.Format(CultureInfo.InvariantCulture, "Licenses: Name={0}, Type={1}, Counter={2}", licenseResponseFromXml.Name, licenseResponseFromXml.LicenseType, licenseResponseFromXml.Counter));
						}
					}
				}
			}
		}
		if (_registrationItem != null)
		{
			if (_registrationItem.Properties.ContainsKey("registrations"))
			{
				XmlDocument xmlDocument2 = new XmlDocument();
				string cipherText2 = _registrationItem.Properties["registrations"];
				cipherText2 = compatibilityHandler.Decrypt(cipherText2);
				xmlDocument2.LoadXml(cipherText2);
				XmlNode xmlNode4 = xmlDocument2.SelectSingleNode("MIPPlugins");
				if (xmlNode4 != null)
				{
					XmlNodeList xmlNodeList2 = xmlNode4.SelectNodes("plugin");
					if (xmlNodeList2 != null)
					{
						foreach (XmlNode item3 in xmlNodeList2)
						{
							Guid guid2 = new Guid(item3.Attributes["id"].InnerText);
							LicenseInformationInternal registrationFromXml = GetRegistrationFromXml(guid2, item3);
							if (!_pluginLicensesInformations.ContainsKey(guid2))
							{
								_pluginLicensesInformations.Add(guid2, new Dictionary<string, LicenseInformationInternal>());
							}
							_pluginLicensesInformations[guid2][registrationFromXml.LicenseInformation.LicenseType] = registrationFromXml;
							EnvironmentManager.Instance.Log(error: false, "LicenseManager", string.Format(CultureInfo.InvariantCulture, "Registrations: Name={0}, Type={1}, Counter={2}", registrationFromXml.LicenseInformation.Name, registrationFromXml.LicenseInformation.LicenseType, registrationFromXml.LicenseInformation.Counter));
						}
					}
				}
			}
			if (_registrationItem.Properties.ContainsKey("Features"))
			{
				string[] array = _registrationItem.Properties["Features"].Split(',');
				foreach (string item in array)
				{
					_features.Add(item);
				}
			}
			if (_registrationItem.Properties.ContainsKey("ExpirationDates"))
			{
				string[] array2 = _registrationItem.Properties["ExpirationDates"].Split(',');
				_expirationDates.Clear();
				string[] array = array2;
				foreach (string text in array)
				{
					if (!string.IsNullOrEmpty(text))
					{
						string[] array3 = text.Split('=');
						DateTime result;
						if (array3.Length != 2)
						{
							EnvironmentManager.Instance.Log(error: true, "LicenseManager", "Incorrect formatting of expiration dates:" + text);
						}
						else if (DateTime.TryParse(array3[1], CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out result))
						{
							_expirationDates[array3[0]] = result;
						}
					}
				}
			}
			if (_registrationItem.Properties.ContainsKey("SLC"))
			{
				_SLC = _registrationItem.Properties["SLC"];
			}
			if (_registrationItem.Properties.ContainsKey("ProductionEnvironment"))
			{
				_productionEnvironment = _registrationItem.Properties["ProductionEnvironment"];
			}
			if (_registrationItem.Properties.ContainsKey("Expire") && !DateTime.TryParse(_registrationItem.Properties["Expire"], CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out _expire))
			{
				_expire = DateTime.MinValue;
			}
			if (_registrationItem.Properties.ContainsKey("ProductCode"))
			{
				int.TryParse(_registrationItem.Properties["ProductCode"], out _productCode);
			}
			else if (Configuration.Instance.ServerFQID.ServerId.ServerType == "XPCO")
			{
				_productCode = 600;
			}
			else
			{
				_productCode = 400;
			}
			if (_registrationItem.Properties.ContainsKey("CudaPublicKey"))
			{
				_cudaPublicKey = registrationItem.Properties["CudaPublicKey"];
			}
		}
		else
		{
			FQID fqid = new FQID(Configuration.Instance.ServerFQID.ServerId, Guid.Empty, compatibilityHandler.RegistrationsObjectId, FolderType.No, _licensePluginDefinitionId);
			_registrationItem = new Item(fqid, "Registrations");
			string value = compatibilityHandler.Encrypt("<MIPPlugins></MIPPlugins>");
			_registrationItem.Properties["registrations"] = value;
			if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Administration)
			{
				_registrationItemDirty = true;
			}
			if (EnvironmentManager.Instance.SmartClientInOfflineMode)
			{
				_features.Add("MIPUseExternally");
				_features.Add("MIPUseInternally");
				_features.Add("ManageAlarms");
				_features.Add("Bookmarking");
			}
		}
	}

	public void SaveLicenseInformation(LicenseInformation licenseInformation)
	{
		SaveLicenseInformation(licenseInformation, IsSavedByAdmin(licenseInformation));
	}

	private static bool IsSavedByAdmin(LicenseInformation licenseInformation)
	{
		LicenseItem configuredLicenseItem = licenseInformation.GetConfiguredLicenseItem(SavedByAdminConfigItemId);
		if (configuredLicenseItem != null)
		{
			licenseInformation.ItemIdentifications.Remove(configuredLicenseItem);
			return true;
		}
		return false;
	}

	private void SaveLicenseInformation(LicenseInformation licenseInformation, bool savedByAdmin)
	{
		if (EnvironmentManager.Instance.Mode != Mode.ServerSetup)
		{
			throw new ImplementationMIPException("Only supported in ServerSetup mode");
		}
		LoadLicenseInformation();
		if (!_pluginLicensesInformations.ContainsKey(licenseInformation.PluginId))
		{
			_pluginLicensesInformations.Add(licenseInformation.PluginId, new Dictionary<string, LicenseInformationInternal>());
		}
		else if (_pluginLicensesInformations[licenseInformation.PluginId].ContainsKey(licenseInformation.LicenseType) && !_pluginLicensesInformations[licenseInformation.PluginId][licenseInformation.LicenseType].SavedByAdmin && savedByAdmin)
		{
			return;
		}
		_pluginLicensesInformations[licenseInformation.PluginId][licenseInformation.LicenseType] = new LicenseInformationInternal
		{
			LicenseInformation = licenseInformation,
			SavedByAdmin = savedByAdmin
		};
		_registrationItemDirty = true;
	}

	public LicenseInformation GetLicenseInformation(Guid pluginId, string licenseType)
	{
		LoadLicenseInformation();
		if (_pluginLicensesInformations.ContainsKey(pluginId) && _pluginLicensesInformations[pluginId].ContainsKey(licenseType))
		{
			return _pluginLicensesInformations[pluginId][licenseType].LicenseInformation;
		}
		return null;
	}

	public void Save()
	{
		if (!_registrationItemDirty || Configuration.Instance == null || EnvironmentManager.Instance.EnvironmentType != EnvironmentType.Administration)
		{
			return;
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml("<MIPPlugins></MIPPlugins>");
		XmlWriter xmlWriter = xmlDocument.CreateNavigator().SelectSingleNode("MIPPlugins").AppendChild();
		foreach (Guid key in _pluginLicensesInformations.Keys)
		{
			foreach (string key2 in _pluginLicensesInformations[key].Keys)
			{
				LicenseInformationInternal licenseInformationInternal = _pluginLicensesInformations[key][key2];
				LicenseInformation licenseInformation = licenseInformationInternal.LicenseInformation;
				xmlWriter.WriteStartElement("plugin");
				xmlWriter.WriteAttributeString("id", key.ToString());
				xmlWriter.WriteAttributeString("licensetype", key2);
				xmlWriter.WriteElementString("expire", DateTimeToString(licenseInformation.Expire));
				xmlWriter.WriteElementString("trial", licenseInformation.TrialMode ? "yes" : "no");
				xmlWriter.WriteElementString("name", licenseInformation.Name);
				xmlWriter.WriteElementString("counter", licenseInformation.Counter.ToString() ?? "");
				xmlWriter.WriteElementString("savedbyadmin", licenseInformationInternal.SavedByAdmin.ToString());
				xmlWriter.WriteElementString("customdata", licenseInformation.CustomData);
				xmlWriter.WriteStartElement("regs");
				foreach (LicenseItem itemIdentification in licenseInformation.ItemIdentifications)
				{
					xmlWriter.WriteStartElement("licid");
					xmlWriter.WriteAttributeString("id", itemIdentification.Id);
					if (itemIdentification.ItemTrial)
					{
						xmlWriter.WriteAttributeString("trialend", DateTimeToString(itemIdentification.ItemTrialEnd));
					}
					xmlWriter.WriteElementString("name", itemIdentification.Name);
					xmlWriter.WriteEndElement();
				}
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndElement();
				EnvironmentManager.Instance.Log(error: false, "LicenseManager", string.Format(CultureInfo.InvariantCulture, "Save Registration: Name={0}, Type={1}, Counter={2}", licenseInformation.Name, licenseInformation.LicenseType, licenseInformation.Counter));
			}
		}
		xmlWriter.Flush();
		xmlWriter.Close();
		ILicenseCompatibilityHandler compatibilityHandler = GetCompatibilityHandler();
		string value = compatibilityHandler.Encrypt(xmlDocument.OuterXml);
		if (_registrationItem == null)
		{
			FQID fqid = new FQID(Configuration.Instance.ServerFQID.ServerId, Guid.Empty, compatibilityHandler.RegistrationsObjectId, FolderType.No, _licenseKind);
			_registrationItem = new Item(fqid, "Registrations");
		}
		_registrationItem.Properties["registrations"] = value;
		_registrationItem.Properties["SLC"] = EnvironmentManager.Instance.SystemLicense.SLC;
		string text = "";
		foreach (string featureFlag in EnvironmentManager.Instance.SystemLicense.FeatureFlags)
		{
			text = text + featureFlag + ",";
		}
		_registrationItem.Properties["Features"] = text;
		DateTime dateTime = EnvironmentManager.Instance.SystemLicense.Expire;
		if (dateTime.Year == 9999)
		{
			dateTime = DateTime.MinValue;
		}
		_registrationItem.Properties["Expire"] = dateTime.ToString(CultureInfo.InvariantCulture);
		_registrationItem.Properties["ProductCode"] = EnvironmentManager.Instance.SystemLicense.ProductCode.ToString(CultureInfo.InvariantCulture);
		_registrationItem.Properties["CudaPublicKey"] = EnvironmentManager.Instance.SystemLicense.CudaPublicKey;
		string text2 = "";
		foreach (string key3 in EnvironmentManager.Instance.SystemLicense.ExpirationDateTimes.Keys)
		{
			text2 = text2 + key3 + "=" + TimeConvert.ToGmtString(EnvironmentManager.Instance.SystemLicense.ExpirationDateTimes[key3], includeMilliSeconds: false) + ",";
		}
		_registrationItem.Properties["ExpirationDates"] = text2;
		_registrationItem.Encrypt = true;
		Configuration.Instance.SaveItemConfiguration(_licensePluginDefinitionId, _registrationItem);
		_registrationItemDirty = false;
	}

	public void ReloadActivatedLicense()
	{
		_licenseLoaded = false;
	}

	private LicenseResponse GetLicenseResponseFromXml(Guid pluginId, XmlNode pluginLic)
	{
		string xmlString = GetXmlString(pluginLic, "name");
		string xmlString2 = GetXmlString(pluginLic, "licensetype");
		int xmlInt = GetXmlInt(pluginLic, "count");
		string xmlString3 = GetXmlString(pluginLic, "customdata");
		DateTime xmlDateTime = GetXmlDateTime(pluginLic, "expire", DateTime.MaxValue);
		bool trialMode = xmlDateTime.Year < 9999;
		string xmlString4 = GetXmlString(pluginLic, "slc");
		int xmlInt2 = GetXmlInt(pluginLic, "activated");
		int xmlInt3 = GetXmlInt(pluginLic, "totalactivated");
		DateTime xmlDateTime2 = GetXmlDateTime(pluginLic, "puac", DateTime.MaxValue);
		Collection<LicenseItem> collection = new Collection<LicenseItem>();
		XmlNodeList xmlNodeList = pluginLic.SelectNodes("items/item");
		if (xmlNodeList != null)
		{
			foreach (XmlNode item in xmlNodeList)
			{
				string xmlString5 = GetXmlString(item, "id");
				string xmlString6 = GetXmlString(item, "name");
				bool itemTrial = GetXmlString(item, "itemtrial") == "true";
				DateTime xmlDateTime3 = GetXmlDateTime(item, "itemtrialend", DateTime.UtcNow);
				collection.Add(new LicenseItem
				{
					Id = xmlString5,
					ItemTrial = itemTrial,
					ItemTrialEnd = xmlDateTime3,
					Name = xmlString6
				});
			}
		}
		return new LicenseResponse
		{
			PluginId = pluginId,
			Counter = xmlInt,
			Name = xmlString,
			LicenseType = xmlString2,
			CustomData = xmlString3,
			Expire = xmlDateTime,
			TrialMode = trialMode,
			ItemIdentifications = collection,
			Slc = xmlString4,
			Activated = xmlInt2,
			TotalActivated = xmlInt3,
			PuAc = xmlDateTime2
		};
	}

	private LicenseInformationInternal GetRegistrationFromXml(Guid pluginId, XmlNode pluginLic)
	{
		LicenseInformation licenseInformation = new LicenseInformation();
		licenseInformation.PluginId = pluginId;
		licenseInformation.LicenseType = pluginLic.Attributes["licensetype"].InnerText;
		licenseInformation.Name = GetXmlString(pluginLic, "name");
		licenseInformation.Counter = GetXmlInt(pluginLic, "counter");
		if (!bool.TryParse(GetXmlString(pluginLic, "savedbyadmin"), out var result))
		{
			result = true;
		}
		licenseInformation.CustomData = GetXmlString(pluginLic, "customdata");
		licenseInformation.Expire = GetXmlDateTime(pluginLic, "expire", DateTime.MaxValue);
		licenseInformation.TrialMode = licenseInformation.Expire.Year < 9999;
		XmlNodeList xmlNodeList = pluginLic.SelectNodes("regs/licid");
		licenseInformation.ItemIdentifications = new Collection<LicenseItem>();
		licenseInformation.PuAc = GetXmlDateTime(pluginLic, "puac", DateTime.MaxValue);
		if (xmlNodeList != null)
		{
			foreach (XmlNode item in xmlNodeList)
			{
				string xmlAttribute = GetXmlAttribute(item, "id");
				string xmlString = GetXmlString(item, "name");
				DateTime attributeDateTime = GetAttributeDateTime(item, "trialend", DateTime.MaxValue);
				bool itemTrial = attributeDateTime != DateTime.MaxValue;
				licenseInformation.ItemIdentifications.Add(new LicenseItem
				{
					Id = xmlAttribute,
					Name = xmlString,
					ItemTrial = itemTrial,
					ItemTrialEnd = attributeDateTime
				});
			}
		}
		return new LicenseInformationInternal
		{
			LicenseInformation = licenseInformation,
			SavedByAdmin = result
		};
	}

	private string GetXmlString(XmlNode node, string id)
	{
		XmlNode xmlNode = node.SelectSingleNode(id);
		if (xmlNode != null)
		{
			return xmlNode.InnerText;
		}
		return "";
	}

	private string GetXmlAttribute(XmlNode node, string id)
	{
		XmlAttribute xmlAttribute = node.Attributes[id];
		if (xmlAttribute != null)
		{
			return xmlAttribute.InnerText;
		}
		return "";
	}

	private int GetXmlInt(XmlNode node, string id)
	{
		XmlNode xmlNode = node.SelectSingleNode(id);
		if (xmlNode != null)
		{
			int result = 0;
			if (int.TryParse(xmlNode.InnerText, out result))
			{
				return result;
			}
		}
		return 0;
	}

	private DateTime GetXmlDateTime(XmlNode node, string id, DateTime defaultDateTime)
	{
		XmlNode xmlNode = node.SelectSingleNode(id);
		if (xmlNode != null)
		{
			try
			{
				return DateTime.ParseExact(xmlNode.InnerText, "yyyy-MM-dd", CultureInfo.InvariantCulture);
			}
			catch
			{
			}
		}
		return defaultDateTime;
	}

	private DateTime GetAttributeDateTime(XmlNode node, string id, DateTime defaultDateTime)
	{
		XmlAttribute xmlAttribute = node.Attributes[id];
		if (xmlAttribute != null)
		{
			try
			{
				return DateTime.ParseExact(xmlAttribute.InnerText, "yyyy-MM-dd", CultureInfo.InvariantCulture);
			}
			catch
			{
			}
		}
		return defaultDateTime;
	}

	private string DateTimeToString(DateTime dateTime)
	{
		return dateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
	}

	public ILicenseCompatibilityHandler GetCompatibilityHandler()
	{
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(EnvironmentManager.Instance.CurrentSite.ServerId);
		if ((loginSettings == null || loginSettings.ServerProductInfo == null || string.IsNullOrEmpty(loginSettings.ServerProductInfo.MajorVersion) || loginSettings.ServerProductInfo.MajorVersion == "0" || loginSettings.ServerProductInfo.MajorVersion == "1") && (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Administration || EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Service || (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient && EnvironmentManager.Instance.SmartClientInOfflineMode) || (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Standalone && EnvironmentManager.Instance.CurrentSite.ServerId.IsExportType)))
		{
			return new LicenseCompatibilityHandler();
		}
		if (loginSettings == null)
		{
			EnvironmentManager.Instance.Log(error: true, "LicenseManager", string.Format(CultureInfo.InvariantCulture, "Login settings not found. EnvironmentType: {0}", EnvironmentManager.Instance.EnvironmentType));
			throw new MIPException("Login settings not found.");
		}
		if (IsServerFipsCompatible(loginSettings.ServerProductInfo))
		{
			return new LicenseCompatibilityHandler();
		}
		return new NonFipsLicenseCompatibilityHandler();
	}

	private static bool IsServerFipsCompatible(LoginServerProductInfo productInfo)
	{
		int result = 0;
		int result2 = 0;
		if (!int.TryParse(productInfo.MajorVersion, out result) || !int.TryParse(productInfo.MinorVersion, out result2) || (result == 0 && result2 == 0))
		{
			throw new MIPException(string.Format(CultureInfo.InvariantCulture, "Could not resolve server version. EnvironmentType: {0}", EnvironmentManager.Instance.EnvironmentType));
		}
		if (result != 20 || result2 != 3)
		{
			return result >= 21;
		}
		return true;
	}
}
