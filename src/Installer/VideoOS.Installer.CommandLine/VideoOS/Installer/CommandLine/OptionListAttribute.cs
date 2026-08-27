using System;

namespace VideoOS.Installer.CommandLine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public sealed class OptionListAttribute : OptionAttribute
{
	public char Separator { get; }

	public OptionListAttribute(string shortName, string longName, string shortExample, char separator)
		: base(shortName, longName, shortExample)
	{
		Separator = separator;
	}
}
