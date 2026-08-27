using VideoOS.Installer.Core.Arguments.Interfaces;

namespace VideoOS.Installer.Core.Arguments;

public class Argument : IArgument
{
	public string Name { get; private set; }

	public string Description { get; private set; }

	public string Value { get; private set; }

	public Argument(string name, string value, string description)
	{
		Name = name;
		Value = value;
		Description = description;
	}
}
