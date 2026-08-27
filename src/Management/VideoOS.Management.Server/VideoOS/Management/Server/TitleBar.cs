using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "TitleBar", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public enum TitleBar
{
	[EnumMember]
	None,
	[EnumMember]
	Caption,
	[EnumMember]
	CaptionAndLive
}
