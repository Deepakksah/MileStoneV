using System.Collections.Generic;

namespace VideoOS.Installer.CommandLine;

public interface ICommandLineParser
{
	bool ParseArguments(string[] args, object options);

	IEnumerable<OptionAttribute> GetOptionList(object options);
}
