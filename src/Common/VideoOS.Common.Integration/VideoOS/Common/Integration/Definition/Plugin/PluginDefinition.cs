using VideoOS.Common.Integration.Definition.Plugin.Schema;

namespace VideoOS.Common.Integration.Definition.Plugin;

public class PluginDefinition
{
	private definitionPlugin _definitionPlugin;

	public definitionPlugin Base => _definitionPlugin;

	public PluginDefinition(definitionPlugin definitionPlugin)
	{
		_definitionPlugin = definitionPlugin;
	}
}
