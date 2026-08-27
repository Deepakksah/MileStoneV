using System.Linq;

namespace VideoOS.ServerConfigurator.RegistrationContainer;

public class AssemblyQualifiedName
{
	private string[] _qualifiedNameparts;

	public string AssemblyFileName => _qualifiedNameparts[1] + ".DLL";

	public string FullTypeName => _qualifiedNameparts[0];

	public AssemblyQualifiedName(string assemblyQualifiedName)
	{
		_qualifiedNameparts = (from x in assemblyQualifiedName.Split(',')
			select x.Trim()).ToArray();
	}
}
