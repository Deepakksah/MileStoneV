using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[DataContract]
public enum SearchFactor
{
	[EnumMember]
	Sid,
	[EnumMember]
	AccountName,
	[EnumMember]
	DistinguishedName
}
