using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class ServerFileSystemOperationCommand : CommonCommandBase
{
	[DataMember]
	public Guid ServerId { get; private set; }

	[DataMember]
	public ServerFileSystemServerType ServerType { get; private set; }

	[DataMember]
	public ServerFileSystemOperation Operation { get; private set; }

	[DataMember]
	public string Path { get; private set; }

	public ServerFileSystemOperationCommand()
	{
	}

	public ServerFileSystemOperationCommand(Guid serverId, ServerFileSystemServerType serverType, ServerFileSystemOperation operation, string path)
	{
		ServerId = serverId;
		ServerType = serverType;
		Operation = operation;
		Path = path;
	}

	public void QueryRecorderDrives(Guid recorderId)
	{
		ServerId = recorderId;
		ServerType = ServerFileSystemServerType.RecordingServer;
		Operation = ServerFileSystemOperation.QueryDrives;
		Path = string.Empty;
	}

	public void QueryRecorderDirectories(Guid recorderId, string path)
	{
		ServerId = recorderId;
		ServerType = ServerFileSystemServerType.RecordingServer;
		Operation = ServerFileSystemOperation.QueryDirectories;
		Path = path;
	}

	public void CreateRecorderDirectory(Guid recorderId, string path)
	{
		ServerId = recorderId;
		ServerType = ServerFileSystemServerType.RecordingServer;
		Operation = ServerFileSystemOperation.CreateDirectory;
		Path = path;
	}

	public override void Accept(ICommandVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override async Task AcceptAsync(ICommonCommandVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		await visitor.Visit(this);
	}
}
