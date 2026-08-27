using System;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public enum DatabaseOperationDto
{
	Undefined,
	[Obsolete("We need to keep this value only because it is used in legacy interfaces. It will never really be used for anything anymore.")]
	CreateStorage,
	[Obsolete("We need to keep this value only because it is used in legacy interfaces. It will never really be used for anything anymore.")]
	CreateArchive,
	VerifyPath,
	VerifyNewArchivePath,
	DeleteTable,
	DiskUsageInformationTablesPerStorage,
	FilterTrackIdsToThoseWithoutMediaOnRecorder,
	VerifyStorageAndArchiveOnline
}
