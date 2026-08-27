using System;

namespace VideoOS.Installer.CommandLine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class OptionAttribute : Attribute
{
	public string ShortName { get; }

	public string ShortExample { get; }

	public string LongName { get; }

	public string HelpText { get; set; }

	public OptionAttribute(string shortName, string longName, string shortExample)
	{
		ShortName = shortName;
		LongName = longName ?? throw new ArgumentNullException("longName");
		ShortExample = shortExample;
	}
}
