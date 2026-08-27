using System;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public enum ServerFileSystemOperationDto
{
	QueryDrives,
	QueryDirectories,
	CreateDirectory
}
