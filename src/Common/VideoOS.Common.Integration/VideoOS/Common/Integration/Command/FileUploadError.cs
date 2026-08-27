using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public enum FileUploadError
{
	[EnumMember]
	InvalidChecksum,
	[EnumMember]
	FileDisposed,
	[EnumMember]
	CannotCreateFile,
	[EnumMember]
	CannotOpenFile,
	[EnumMember]
	QuotaExceeded,
	[EnumMember]
	Unknown
}
