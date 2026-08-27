using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[DataContract]
public enum DatabaseOperation
{
	[EnumMember]
	Undefined,
	[EnumMember]
	VerifyPath,
	[EnumMember]
	VerifyNewArchivePath,
	[EnumMember]
	DiskUsageInformationTablesPerStorage,
	[EnumMember]
	FilterTrackIdsToThoseWithoutMediaOnRecorder,
	[EnumMember]
	VerifyStorageAndArchiveOnline
}
