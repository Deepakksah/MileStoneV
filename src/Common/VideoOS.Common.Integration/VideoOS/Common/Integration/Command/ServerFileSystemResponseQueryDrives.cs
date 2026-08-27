using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class ServerFileSystemResponseQueryDrives : ServerFileSystemResponse
{
	[DataMember]
	public string[] Drives { get; public set; }

	public ServerFileSystemResponseQueryDrives(string[] drives)
		: base(ServerFileSystemOperation.QueryDrives)
	{
		Drives = drives;
	}

	public ServerFileSystemResponseQueryDrives(ServerFileSystemResponseStatus status, string message)
		: base(ServerFileSystemOperation.QueryDrives, status, message)
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
