namespace VideoOS.Platform.AccessControl.Plugin;

public sealed class ACExternalCommand
{
	public string Label { get; private set; }

	public string Command { get; private set; }

	public string Type { get; private set; }

	public int Order { get; private set; }

	public ACExternalCommand(string label, string command, string type, int order)
	{
		Label = label;
		Command = command;
		Type = type;
		Order = order;
	}
}
