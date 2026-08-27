using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[DataContract]
public enum BackupConfigAction
{
	[EnumMember]
	StartBackup,
	[EnumMember]
	AbortBackup
}
