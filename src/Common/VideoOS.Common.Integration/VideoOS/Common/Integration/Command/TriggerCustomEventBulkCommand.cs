using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
public class TriggerCustomEventBulkCommand : CommonCommandBase, ICommandBulk
{
	private Collection<CommandPackage> _commands;

	public Collection<CommandPackage> Commands => _commands;

	public void AddCommand(CommandPackage command)
	{
		_commands.Add(command);
	}

	public TriggerCustomEventBulkCommand()
	{
		_commands = new Collection<CommandPackage>();
	}

	public TriggerCustomEventBulkCommand(Collection<CommandPackage> commands)
	{
		_commands = commands;
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
