using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[DataContract]
public enum BackupConfigError
{
	[EnumMember]
	Undefined = 0,
	[EnumMember]
	BackupAlreadyActive = -100001,
	[EnumMember]
	BackupAlreadyInactive = -100002,
	[EnumMember]
	ExceptionOccurred = -100003,
	[EnumMember]
	SqlServerRunningAsLocal = -100004,
	[EnumMember]
	NoBackupFilesWereMade = -100005,
	[EnumMember]
	MoveFilesFailed = -100006,
	[EnumMember]
	MissingBackupFolder = -100007,
	[EnumMember]
	LowOnDiskSpace = -100008,
	[EnumMember]
	SqlBackupError = -100009,
	[EnumMember]
	SqlAuthenticationError = -100010,
	[EnumMember]
	FileIOError = -100011,
	[EnumMember]
	SqlNetworkError = -100012
}
