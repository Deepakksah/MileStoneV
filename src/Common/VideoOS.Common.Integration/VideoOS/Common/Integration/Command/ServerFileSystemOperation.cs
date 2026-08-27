using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public enum ServerFileSystemOperation
{
	[EnumMember]
	QueryDrives,
	[EnumMember]
	QueryDirectories,
	[EnumMember]
	CreateDirectory
}
