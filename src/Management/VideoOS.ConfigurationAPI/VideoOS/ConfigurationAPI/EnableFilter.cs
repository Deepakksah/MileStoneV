using System.Runtime.Serialization;

namespace VideoOS.ConfigurationAPI;

[DataContract]
public enum EnableFilter
{
	[EnumMember]
	All,
	[EnumMember]
	Enabled,
	[EnumMember]
	Disabled
}
