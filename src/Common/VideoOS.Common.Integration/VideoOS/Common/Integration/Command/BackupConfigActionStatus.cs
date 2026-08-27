using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[DataContract]
public enum BackupConfigActionStatus
{
	[EnumMember]
	Undefined,
	[EnumMember]
	NotStarted,
	[EnumMember]
	Validating,
	[EnumMember]
	CreatingBackup,
	[EnumMember]
	CompressingBackupFiles
}
