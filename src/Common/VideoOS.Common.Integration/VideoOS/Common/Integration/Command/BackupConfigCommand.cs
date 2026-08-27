using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[DataContract]
public class BackupConfigCommand : CommonCommandBase, ICommandWithSource
{
	public static readonly Guid TokenId = new Guid("C3BE947B-8268-4D79-9BF4-37E5FD1E9B4B");

	[DataMember]
	public BackupConfigAction Action { get; set; }

	public Guid SourceId => Guid.NewGuid();

	public BackupConfigCommand(BackupConfigAction action)
	{
		Action = action;
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
