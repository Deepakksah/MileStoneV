using System;
using System.Diagnostics;
using System.Globalization;
using VideoOS.Platform.Util.VideoWall;

namespace VideoOS.Platform.Proxy.ConfigApi;

[DebuggerDisplay("{Path}")]
public class ConfigurationItemPath
{
	public string Path { get; private set; }

	public bool IsFolder { get; private set; }

	public string ParentItemType { get; private set; }

	public string ItemType { get; private set; }

	public string Id { get; private set; }

	public string Text => Path;

	public static ConfigurationItemPath CreatePath(string path)
	{
		return new ConfigurationItemPath(path);
	}

	public static ConfigurationItemPath CreateItemPath(string itemType, string id)
	{
		return new ConfigurationItemPath(itemType, id);
	}

	public ConfigurationItemPath(string path)
	{
		Path = path;
		if (string.IsNullOrEmpty(path) || path == "/")
		{
			Id = "";
			ItemType = "System";
			ParentItemType = "";
			IsFolder = false;
			return;
		}
		string[] array = path.Split('/');
		int num = path.IndexOf("[", StringComparison.InvariantCulture);
		int num2 = path.IndexOf("]", StringComparison.InvariantCulture);
		if (path.StartsWith("/", StringComparison.InvariantCulture))
		{
			ParentItemType = "System";
			ItemType = array[1];
		}
		else if (array.Length == 1)
		{
			if (num < 0 || num2 < 0 || num > num2)
			{
				EnvironmentManager.Instance.Log(error: true, "ConfigurationItemPath", "Invalid Path:" + path);
				throw new ArgumentMIPException("Invalid Path: " + path);
			}
			Id = path.Substring(num + 1, num2 - num - 1);
			ItemType = path.Substring(0, num);
			ParentItemType = "";
		}
		else if (array.Length == 2 && string.IsNullOrEmpty(array[1]))
		{
			ItemType = array[0];
			Id = string.Empty;
			ParentItemType = "";
		}
		else
		{
			if (array.Length != 2 || num < 0 || num2 < 0 || num > num2)
			{
				EnvironmentManager.Instance.Log(error: true, "ConfigurationItemPath", "Invalid Path:" + path);
				throw new ArgumentMIPException("Invalid Path: " + path);
			}
			ItemType = array[1];
			Id = path.Substring(num + 1, num2 - num - 1);
			ParentItemType = path.Substring(0, num);
		}
		if (ItemType.EndsWith("Folder", StringComparison.InvariantCulture))
		{
			IsFolder = true;
		}
	}

	public ConfigurationItemPath(FQID fqid)
	{
		if (fqid == null)
		{
			throw new ArgumentNullException("fqid");
		}
		Id = fqid.ObjectId.ToString().ToUpper(CultureInfo.InvariantCulture);
		IsFolder = false;
		ItemType = ConfigurationApiClassConverter.ConvertToItemType(fqid.Kind);
		ParentItemType = ItemType + "Folder";
		if (IsFolder)
		{
			Path = string.Format(CultureInfo.InvariantCulture, "{0}[{1}]/{2}", ParentItemType, Id, ItemType);
		}
		else
		{
			Path = string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", ItemType, Id);
		}
	}

	public ConfigurationItemPath(string itemType, Guid guid)
		: this(itemType, guid.ToString())
	{
	}

	public ConfigurationItemPath(string itemType, string id)
	{
		Id = id;
		IsFolder = false;
		if (string.IsNullOrEmpty(itemType) || itemType == "System")
		{
			ItemType = "System";
			Path = "/";
			return;
		}
		ItemType = itemType;
		ParentItemType = ItemType + "Folder";
		if (IsFolder)
		{
			Path = string.Format(CultureInfo.InvariantCulture, "{0}[{1}]/{2}", ParentItemType, Id, ItemType);
		}
		else
		{
			Path = string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", ItemType, Id);
		}
	}

	public ConfigurationItemPath(ConfigurationItemPath parent, string childItemType)
	{
		string text = (childItemType.EndsWith("Folder", StringComparison.Ordinal) ? childItemType : (childItemType + "Folder"));
		IsFolder = true;
		if (parent.Path == "/")
		{
			ParentItemType = "System";
			Id = parent.Id;
			ItemType = childItemType;
			Path = "/" + text;
		}
		else
		{
			ParentItemType = parent.ItemType;
			Id = parent.Id;
			ItemType = childItemType;
			Path = parent.Path + "/" + text;
		}
	}
}
