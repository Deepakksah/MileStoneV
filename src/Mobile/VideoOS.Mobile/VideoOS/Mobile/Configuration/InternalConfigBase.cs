using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Runtime.Serialization;
using System.Xml.Linq;
using VideoOS.Mobile.Configuration.Group;
using VideoOS.Mobile.Configuration.Section;

namespace VideoOS.Mobile.Configuration;

[DataContract]
public class InternalConfigBase
{
	private static class LocalConfigOverride
	{
		public static IDictionary<string, string> LocalConfig;

		public static void LoadConfig()
		{
		}

		public static NameValueCollection ApplyLocalConfigValues(IConfigGroup group, IConfigSection section, NameValueCollection config)
		{
			return config;
		}
	}

	private string _path;

	[DataMember]
	public ManagementServerConfigGroup ManagementServer { get; private set; }

	[DataMember]
	public HttpMetaChannelConfigGroup HttpMetaChannel { get; private set; }

	[DataMember]
	public HttpSecureMetaChannelConfigGroup HttpSecureMetaChannel { get; private set; }

	[DataMember]
	public ServerSettingsConfigGroup ServerSettings { get; private set; }

	public List<IConfigGroup> ConfigGroups { get; private set; }

	public InternalConfigBase(string path = null)
	{
		_path = path;
		ManagementServer = new ManagementServerConfigGroup();
		HttpMetaChannel = new HttpMetaChannelConfigGroup();
		HttpSecureMetaChannel = new HttpSecureMetaChannelConfigGroup();
		ServerSettings = new ServerSettingsConfigGroup();
		ConfigGroups = new List<IConfigGroup> { ManagementServer, HttpMetaChannel, HttpSecureMetaChannel, ServerSettings };
	}

	public virtual void InitFromConfigurationFile()
	{
		LocalConfigOverride.LoadConfig();
		foreach (IConfigGroup configGroup in ConfigGroups)
		{
			foreach (IConfigSection item in configGroup.Items)
			{
				NameValueCollection nameValueCollection;
				if (!string.IsNullOrEmpty(_path))
				{
					string text = ConfigurationManager.OpenExeConfiguration(_path).GetSection(configGroup.GroupName + "/" + item.SectionName)?.SectionInformation.GetRawXml();
					if (text == null)
					{
						continue;
					}
					nameValueCollection = new NameValueCollection();
					foreach (XElement item2 in XDocument.Parse(text).Root.Elements("add"))
					{
						nameValueCollection.Add(item2.Attribute("key")?.Value, item2.Attribute("value")?.Value);
					}
				}
				else
				{
					nameValueCollection = (NameValueCollection)ConfigurationManager.GetSection(configGroup.GroupName + "/" + item.SectionName);
				}
				nameValueCollection = LocalConfigOverride.ApplyLocalConfigValues(configGroup, item, nameValueCollection);
				if (item != null && nameValueCollection != null)
				{
					item.InitFromCollection(nameValueCollection);
				}
			}
		}
	}
}
