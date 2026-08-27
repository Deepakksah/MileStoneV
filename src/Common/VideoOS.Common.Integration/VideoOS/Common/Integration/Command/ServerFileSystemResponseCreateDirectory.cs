using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class ServerFileSystemResponseCreateDirectory : ServerFileSystemResponse
{
	public ServerFileSystemResponseCreateDirectory()
		: base(ServerFileSystemOperation.CreateDirectory)
	{
	}

	public ServerFileSystemResponseCreateDirectory(ServerFileSystemResponseStatus status, string message)
		: base(ServerFileSystemOperation.CreateDirectory, status, message)
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
