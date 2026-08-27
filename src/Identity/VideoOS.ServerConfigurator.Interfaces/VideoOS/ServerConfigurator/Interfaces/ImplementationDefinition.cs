using System;

namespace VideoOS.ServerConfigurator.Interfaces;

[Serializable]
public class ImplementationDefinition
{
	public string AssemblyQualifiedName { get; }

	public string FileLocation { get; private set; }

	private ImplementationDefinition()
	{
	}

	public ImplementationDefinition(string assemblyQualifiedName)
		: this(assemblyQualifiedName, null)
	{
	}

	public ImplementationDefinition(string assemblyQualifiedName, string fileLocation)
	{
		AssemblyQualifiedName = assemblyQualifiedName;
		FileLocation = fileLocation;
	}

	public void SetFileLocation(string fileLocation)
	{
		if (FileLocation != null)
		{
			throw new InvalidOperationException(FormattableString.Invariant($"File location already set to {FileLocation}. The file location cannot be changed"));
		}
		FileLocation = fileLocation;
	}
}
