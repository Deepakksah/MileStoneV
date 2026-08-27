using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication.Internal;

namespace VideoOS.Common.Integration.Communication;

public static class ServerFileSystemResponseDtoConvert
{
	public static ServerFileSystemResponseQueryDrives CreateServerFileSystemResponseQueryDrives(ServerFileSystemResponseStatusDto serverFileSystemResponseStatusDto, string message, string[] drives)
	{
		return new ServerFileSystemResponseQueryDrives(DtoConvert.ToServerFileSystemResponseStatus(serverFileSystemResponseStatusDto), message)
		{
			Operation = ServerFileSystemOperation.QueryDrives,
			Drives = drives
		};
	}

	public static ServerFileSystemResponseQueryDirectories CreateServerFileSystemResponseQueryDirectories(ServerFileSystemResponseStatusDto serverFileSystemResponseStatusDto, string message, string[] directories)
	{
		return new ServerFileSystemResponseQueryDirectories(DtoConvert.ToServerFileSystemResponseStatus(serverFileSystemResponseStatusDto), message)
		{
			Operation = ServerFileSystemOperation.QueryDirectories,
			Directories = directories
		};
	}

	public static ServerFileSystemResponseCreateDirectory CreateServerFileSystemResponseCreateDirectory(ServerFileSystemResponseStatusDto serverFileSystemResponseStatusDto, string message)
	{
		return new ServerFileSystemResponseCreateDirectory(DtoConvert.ToServerFileSystemResponseStatus(serverFileSystemResponseStatusDto), message)
		{
			Operation = ServerFileSystemOperation.CreateDirectory
		};
	}
}
