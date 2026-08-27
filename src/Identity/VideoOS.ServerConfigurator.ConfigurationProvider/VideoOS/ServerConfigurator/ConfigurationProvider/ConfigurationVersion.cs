using System;
using System.Runtime.Serialization;

namespace VideoOS.ServerConfigurator.ConfigurationProvider;

[Serializable]
[DataContract(Namespace = "")]
public class ConfigurationVersion : ExtensibleDataObject
{
	[DataMember]
	public int Version { get; set; } = 2;
}
