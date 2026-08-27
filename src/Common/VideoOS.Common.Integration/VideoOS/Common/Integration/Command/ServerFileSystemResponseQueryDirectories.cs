using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class ServerFileSystemResponseQueryDirectories : ServerFileSystemResponse
{
	[DataMember]
	public string[] Directories { get; public set; }

	public ServerFileSystemResponseQueryDirectories(string[] directories)
		: base(ServerFileSystemOperation.QueryDirectories)
	{
		Directories = directories;
	}

	public ServerFileSystemResponseQueryDirectories(ServerFileSystemResponseStatus status, string message)
		: base(ServerFileSystemOperation.QueryDirectories, status, message)
	{
	}

	public override void Accept(ICommonResponseVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override async Task AcceptAsync(ICommonResponseVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		await visitor.Visit(this);
	}
}
