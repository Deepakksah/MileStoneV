using System;
using System.Runtime.Serialization;

namespace VideoOS.ServerConfigurator.ConfigurationProvider;

[Serializable]
[DataContract(Namespace = "")]
public class SystemConfigurationElement : ExtensibleDataObject
{
	[DataMember(Order = 0)]
	public Guid ConfigurationElementId { get; set; }

	[DataMember(Order = 1)]
	public string DisplayName { get; set; }

	[DataMember(Order = 2)]
	public string ConfigurationRegistrarType { get; set; }

	[DataMember(Order = 3)]
	public string RegistrarImplementationLocation { get; set; }

	[DataMember(Order = 4)]
	public SystemFeature SystemFeature { get; set; }

	[DataMember(Order = 5)]
	public ConfigurationPurpose Purpose { get; set; }
}
