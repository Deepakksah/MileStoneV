using VideoOS.Common.Integration.Drivers;

namespace VideoOS.Common.Integration;

public class PluginInfo
{
	private string _assemblyFile;

	private string _className;

	private PluginAttribute _attribute;

	public string AssemblyFile => _assemblyFile;

	public string ClassName => _className;

	public PluginAttribute Attribute => _attribute;

	public PluginInfo(string assemblyFile, string className, PluginAttribute attribute)
	{
		_assemblyFile = assemblyFile;
		_className = className;
		_attribute = attribute;
	}
}
