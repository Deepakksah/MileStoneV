using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public enum ServerFileSystemResponseStatus
{
	[EnumMember]
	Success = 1,
	[EnumMember]
	Error
}
