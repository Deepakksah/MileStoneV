using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace VideoOS.ServerConfigurator.ConfigurationProvider;

[Serializable]
[DataContract(Namespace = "")]
[XmlRoot("ConfigurationData")]
public class ConfigurationDataBase : ExtensibleDataObject
{
	[DataMember]
	public ConfigurationVersion ConfigurationVersion { get; set; }

	public ConfigurationDataBase()
	{
		ConfigurationVersion = new ConfigurationVersion();
	}
}
