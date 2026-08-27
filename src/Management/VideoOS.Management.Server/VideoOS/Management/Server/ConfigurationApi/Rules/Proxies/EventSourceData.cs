namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public class EventSourceData
{
	public string[] ItemTypes { get; set; } = new string[0];

	public string[] Filters { get; set; } = new string[0];

	public string PluginType { get; set; } = string.Empty;
}
