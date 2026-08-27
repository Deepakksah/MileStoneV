namespace VideoOS.Installer.Core.Arguments.Interfaces;

public interface IArgument
{
	string Name { get; }

	string Description { get; }

	string Value { get; }
}
