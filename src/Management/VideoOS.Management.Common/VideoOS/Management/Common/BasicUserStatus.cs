using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[DataContract(Name = "BasicUserStatus", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public enum BasicUserStatus
{
	[EnumMember]
	Enabled,
	[EnumMember]
	LockedOutByAdmin,
	[EnumMember]
	LockedOutBySystem
}
