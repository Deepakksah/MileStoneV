namespace VideoOS.Platform.Proxy.RestApi;

public class ResourceEntry
{
	public string ItemType { get; }

	public string ItemTypeFolder { get; }

	public string ResourceNamePlural { get; }

	public ResourceEntry(string itemType, string itemTypeFolder, string resourceNamePlural)
	{
		ItemType = itemType;
		ItemTypeFolder = itemTypeFolder;
		ResourceNamePlural = resourceNamePlural;
	}
}
