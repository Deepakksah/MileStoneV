namespace VideoOS.Platform.ConfigurationItems;

public class UpdateHardwareDetail
{
	public string Path { get; private set; }

	public string Name { get; private set; }

	public string ItemType { get; private set; }

	public string Change { get; private set; }

	public UpdateHardwareDetail(string path, string name, string itemType, string change)
	{
		Path = path;
		Name = name;
		Change = change;
		ItemType = itemType;
	}
}
