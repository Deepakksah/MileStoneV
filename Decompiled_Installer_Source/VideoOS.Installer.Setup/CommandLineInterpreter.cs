using System.Text.RegularExpressions;
using VideoOS.Installer.CommandLine;

namespace VideoOS.Installer.Setup;

internal static class CommandLineInterpreter
{
	internal static InstallerOptions GetOptions(string[] args)
	{
		InstallerOptions installerOptions = new InstallerOptions();
		ICommandLineParser commandLineParser = new CommandLineParser();
		string[] array = (string[])args.Clone();
		for (int i = 0; i < args.Length; i++)
		{
			array[i] = Regex.Replace(array[i], "^([^-].*)", "--${1}");
			array[i] = array[i].Replace("\"", "");
		}
		if (!commandLineParser.ParseArguments(array, installerOptions))
		{
			return null;
		}
		return installerOptions;
	}
}
