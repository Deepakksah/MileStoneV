using System.Collections.ObjectModel;

namespace VideoOS.Common.Integration.Command;

public interface ICommandBulk
{
	Collection<CommandPackage> Commands { get; }

	void AddCommand(CommandPackage command);
}
