using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using VideoOS.Management.Server.Properties;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server.ConfigurationApi;

public class GenericEventDataSource
{
	private static Guid _guidCompatible = new Guid("B867DB0C-BE9E-422B-B934-6FC7FA98C5D8");

	private static Guid _guidInternational = new Guid("8607BCCC-2BB5-4B47-A7DE-8225D14C4213");

	private static GenericEventDataSource _factoryCompatible = new GenericEventDataSource(ConfigurationApiResources.GenericEventDataSourceNameCompatible, GuidCompatible, "Both", "InterNetwork", 1234, new byte[0], System.Text.Encoding.Default.CodePage, log: false, "Full", "", "", enabled: false);

	private static GenericEventDataSource _factoryInternational = new GenericEventDataSource(ConfigurationApiResources.GenericEventDataSourceNameInternational, GuidInternational, "Tcp", "Both", 1235, new byte[2] { 13, 10 }, System.Text.Encoding.UTF8.CodePage, log: false, "Statistics", "", "", enabled: false);

	public static Guid GenericEventsToolsOptionsPluginId = new Guid("2DCC9D9D-688E-468D-AF6F-C2A9ACD623FE");

	private static string NameKey = "Name";

	private static string IdKey = "Guid";

	private static string EnabledKey = "Enabled";

	private static string PortKey = "Port";

	private static string SeparatorKey = "Separator";

	private static string EncodingKey = "Encoding";

	private static string LogKey = "Log";

	private static string EchoKey = "Echo";

	private static string AllowedKey = "Allowed";

	private static string Allowed6Key = "Allowed6";

	private static string ProtocolKey = "Protocol";

	private static string AddressFamilyKey = "AddressFamily";

	public string Name { get; set; }

	public Guid Id { get; set; }

	public int Port { get; set; }

	public byte[] Separator { get; set; }

	public int Encoding { get; set; }

	public bool Log { get; set; }

	public string Echo { get; set; }

	public string Allowed { get; set; }

	public string Allowed6 { get; set; }

	public bool Enabled { get; set; }

	public string Protocol { get; set; }

	public string AddressFamily { get; set; }

	public static Guid GuidCompatible => _guidCompatible;

	public static Guid GuidInternational => _guidInternational;

	public GenericEventDataSource(string name, Guid guid, string protocol, string family, int port, byte[] separator, int encoding, bool log, string echo, string allowed, string allowed6, bool enabled)
	{
		Protocol = protocol;
		AddressFamily = family;
		Enabled = enabled;
		Name = name;
		Id = guid;
		Port = port;
		Separator = separator;
		Encoding = encoding;
		Log = log;
		Echo = echo;
		Allowed = allowed;
		Allowed6 = allowed6;
	}

	public GenericEventDataSource(XmlNode node)
	{
		Name = GetString(node, NameKey, "");
		Id = GetGuid(node, IdKey);
		Port = GetInt(node, PortKey, 1235);
		Separator = GetBytes(node, SeparatorKey);
		Encoding = GetInt(node, EncodingKey, 1252);
		Log = GetBool(node, LogKey, defaultValue: false);
		Echo = GetString(node, EchoKey, "Full");
		Allowed = GetString(node, AllowedKey, "");
		Allowed6 = GetString(node, Allowed6Key, "");
		Enabled = GetBool(node, EnabledKey, defaultValue: false);
		Protocol = GetString(node, ProtocolKey, "Both");
		AddressFamily = GetString(node, AddressFamilyKey, "InterNetwork");
	}

	public string Serialize()
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlTextWriter xmlTextWriter = null;
		try
		{
			xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder, CultureInfo.InvariantCulture));
			xmlTextWriter.WriteStartElement("DataSourceEntry");
			xmlTextWriter.WriteElementString(NameKey, Name);
			xmlTextWriter.WriteElementString(IdKey, Id.ToString());
			xmlTextWriter.WriteElementString(PortKey, Port.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteElementString(SeparatorKey, Convert.ToBase64String(Separator));
			xmlTextWriter.WriteElementString(EncodingKey, Encoding.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteElementString(LogKey, Log.ToString().ToLowerInvariant());
			xmlTextWriter.WriteElementString(EchoKey, Echo);
			xmlTextWriter.WriteElementString(AllowedKey, Allowed);
			xmlTextWriter.WriteElementString(Allowed6Key, Allowed6);
			xmlTextWriter.WriteElementString(EnabledKey, Enabled.ToString().ToLowerInvariant());
			xmlTextWriter.WriteElementString(ProtocolKey, Protocol.ToString(CultureInfo.InvariantCulture));
			xmlTextWriter.WriteElementString(AddressFamilyKey, AddressFamily);
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.Flush();
			xmlTextWriter.Dispose();
			xmlTextWriter = null;
			return stringBuilder.ToString();
		}
		finally
		{
			xmlTextWriter?.Dispose();
		}
	}

	private static string GetString(XmlNode node, string key, string defaultValue)
	{
		XmlNode xmlNode = node[key];
		if (xmlNode != null)
		{
			return xmlNode.InnerText;
		}
		return defaultValue;
	}

	private static byte[] GetBytes(XmlNode node, string key)
	{
		string text = GetString(node, key, "");
		if (!string.IsNullOrEmpty(text))
		{
			try
			{
				return Convert.FromBase64String(text);
			}
			catch (FormatException)
			{
			}
		}
		return new byte[0];
	}

	private static int GetInt(XmlNode node, string key, int defaultValue)
	{
		XmlNode xmlNode = node[key];
		int result = defaultValue;
		if (xmlNode != null)
		{
			int.TryParse(xmlNode.InnerText, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
			return result;
		}
		return result;
	}

	private static bool GetBool(XmlNode node, string key, bool defaultValue)
	{
		XmlNode xmlNode = node[key];
		bool result = defaultValue;
		if (xmlNode != null && bool.TryParse(xmlNode.InnerText, out var result2))
		{
			result = result2;
		}
		return result;
	}

	private static Guid GetGuid(XmlNode node, string key)
	{
		XmlNode xmlNode = node[key];
		if (xmlNode != null && Guid.TryParse(xmlNode.InnerText, out var result))
		{
			return result;
		}
		return Guid.Empty;
	}

	public static List<GenericEventDataSource> QueryDataSources(IGeminiRequestContext requestContext)
	{
		List<GenericEventDataSource> list = new List<GenericEventDataSource>();
		CustomSetting customSettingDataGlobal = CustomSetting.GetCustomSettingDataGlobal(requestContext, GenericEventsToolsOptionsPluginId);
		if (customSettingDataGlobal == null)
		{
			CreateDefaultDataSources(requestContext);
			customSettingDataGlobal = CustomSetting.GetCustomSettingDataGlobal(requestContext, GenericEventsToolsOptionsPluginId);
		}
		if (customSettingDataGlobal != null)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(customSettingDataGlobal.CustomSettingDataXml);
			foreach (XmlNode item in xmlDocument.SelectNodes("ArrayOfDataSourceEntry/DataSourceEntry"))
			{
				list.Add(new GenericEventDataSource(item));
			}
		}
		return list;
	}

	public static GenericEventDataSource QueryDataSource(IGeminiRequestContext requestContext, Guid id)
	{
		CustomSetting customSettingDataGlobal = CustomSetting.GetCustomSettingDataGlobal(requestContext, GenericEventsToolsOptionsPluginId);
		if (customSettingDataGlobal == null)
		{
			CreateDefaultDataSources(requestContext);
			customSettingDataGlobal = CustomSetting.GetCustomSettingDataGlobal(requestContext, GenericEventsToolsOptionsPluginId);
		}
		if (customSettingDataGlobal != null)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(customSettingDataGlobal.CustomSettingDataXml);
			foreach (XmlNode item in xmlDocument.SelectNodes("ArrayOfDataSourceEntry/DataSourceEntry"))
			{
				GenericEventDataSource genericEventDataSource = new GenericEventDataSource(item);
				if (genericEventDataSource.Id == id)
				{
					return genericEventDataSource;
				}
			}
		}
		return null;
	}

	public static void SaveDataSource(IGeminiRequestContext requestContext, GenericEventDataSource dataSource)
	{
		CustomSetting customSettingDataGlobal = CustomSetting.GetCustomSettingDataGlobal(requestContext, GenericEventsToolsOptionsPluginId);
		XmlDocument xmlDocument = new XmlDocument();
		if (customSettingDataGlobal != null)
		{
			xmlDocument.LoadXml(customSettingDataGlobal.CustomSettingDataXml);
		}
		else if (customSettingDataGlobal == null)
		{
			CreateDefaultDataSources(requestContext);
			customSettingDataGlobal = CustomSetting.GetCustomSettingDataGlobal(requestContext, GenericEventsToolsOptionsPluginId);
			xmlDocument.LoadXml(customSettingDataGlobal.CustomSettingDataXml);
		}
		XmlNode xmlNode = xmlDocument["ArrayOfDataSourceEntry"];
		XmlNodeList xmlNodeList = xmlNode.SelectNodes("DataSourceEntry");
		if (xmlNodeList != null)
		{
			foreach (XmlNode item in xmlNodeList)
			{
				if (new GenericEventDataSource(item).Id == dataSource.Id)
				{
					xmlNode.RemoveChild(item);
				}
			}
		}
		xmlNode.CreateNavigator().AppendChild(dataSource.Serialize());
		CustomSettingInternalData customSettingInternalData = new CustomSettingInternalData
		{
			CustomSettingDataXml = xmlDocument.OuterXml,
			DataVersion = customSettingDataGlobal.DataVersion + 1
		};
		CustomSetting.SetCustomSettingDataGlobal(requestContext, GenericEventsToolsOptionsPluginId, customSettingInternalData, forceOverride: false);
		UpdateKindVersion(requestContext, customSettingInternalData.DataVersion);
	}

	public static void DeleteDataSource(IGeminiRequestContext requestContext, Guid dataSourceId)
	{
		CustomSetting customSettingDataGlobal = CustomSetting.GetCustomSettingDataGlobal(requestContext, GenericEventsToolsOptionsPluginId);
		XmlDocument xmlDocument = new XmlDocument();
		if (customSettingDataGlobal == null)
		{
			throw new RemoveNonExistingMIPItemException();
		}
		xmlDocument.LoadXml(customSettingDataGlobal.CustomSettingDataXml);
		XmlNode xmlNode = xmlDocument["ArrayOfDataSourceEntry"];
		XmlNodeList xmlNodeList = xmlNode.SelectNodes("DataSourceEntry");
		bool flag = false;
		if (xmlNodeList != null)
		{
			foreach (XmlNode item in xmlNodeList)
			{
				if (new GenericEventDataSource(item).Id == dataSourceId)
				{
					xmlNode.RemoveChild(item);
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			throw new RemoveNonExistingMIPItemException();
		}
		CustomSettingInternalData customSettingInternalData = new CustomSettingInternalData
		{
			CustomSettingDataXml = xmlDocument.OuterXml,
			DataVersion = customSettingDataGlobal.DataVersion + 1
		};
		CustomSetting.SetCustomSettingDataGlobal(requestContext, GenericEventsToolsOptionsPluginId, customSettingInternalData, forceOverride: false);
		UpdateKindVersion(requestContext, customSettingInternalData.DataVersion);
	}

	private static void CreateDefaultDataSources(IGeminiRequestContext requestContext)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml("<ArrayOfDataSourceEntry/>");
		XPathNavigator xPathNavigator = xmlDocument["ArrayOfDataSourceEntry"].CreateNavigator();
		xPathNavigator.AppendChild(_factoryCompatible.Serialize());
		xPathNavigator.AppendChild(_factoryInternational.Serialize());
		CustomSettingInternalData customSettingInternalData = new CustomSettingInternalData
		{
			CustomSettingDataXml = xmlDocument.OuterXml,
			DataVersion = 1
		};
		CustomSetting.SetCustomSettingDataGlobal(requestContext, GenericEventsToolsOptionsPluginId, customSettingInternalData, forceOverride: false);
		UpdateKindVersion(requestContext, customSettingInternalData.DataVersion);
	}

	private static void UpdateKindVersion(IGeminiRequestContext requestContext, int newVersion)
	{
		MIPKind kind = MIPKind.GetKind(requestContext, GenericEventsToolsOptionsPluginId);
		if (kind != null)
		{
			kind.DataVersion = newVersion;
			MIPKind.SaveKind(requestContext, kind);
		}
	}
}
