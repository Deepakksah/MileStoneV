using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

public class RuleActionFactoryInfo
{
	private string _assemblyFile;

	private string _className;

	private RuleActionFactoryAttribute _attribute;

	public string AssemblyFile => _assemblyFile;

	public string ClassName => _className;

	public RuleActionFactoryAttribute Attribute => _attribute;

	public RuleActionFactoryInfo(string assemblyFile, string className, RuleActionFactoryAttribute attribute)
	{
		_assemblyFile = assemblyFile;
		_className = className;
		_attribute = attribute;
	}
}
