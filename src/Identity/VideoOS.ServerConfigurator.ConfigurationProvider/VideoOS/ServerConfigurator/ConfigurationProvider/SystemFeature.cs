using System.Runtime.Serialization;

namespace VideoOS.ServerConfigurator.ConfigurationProvider;

[DataContract]
public enum SystemFeature
{
	[EnumMember]
	None,
	[EnumMember]
	IsManagementServer
}
