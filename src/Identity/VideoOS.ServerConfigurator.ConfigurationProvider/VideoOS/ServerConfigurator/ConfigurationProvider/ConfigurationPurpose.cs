using System.Runtime.Serialization;

namespace VideoOS.ServerConfigurator.ConfigurationProvider;

[DataContract]
public enum ConfigurationPurpose
{
	[EnumMember]
	None,
	[EnumMember]
	ChangeHostName
}
