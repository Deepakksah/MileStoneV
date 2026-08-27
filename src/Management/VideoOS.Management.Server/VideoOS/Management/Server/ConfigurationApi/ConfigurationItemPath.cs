using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace VideoOS.Management.Server.ConfigurationApi;

[DebuggerDisplay("{Text}")]
public class ConfigurationItemPath
{
	public string Text { get; private set; }

	public bool IsFolder { get; private set; }

	public string ParentItemType { get; private set; }

	public string ItemType { get; private set; }

	public string Id { get; private set; }

	public ConfigurationItemPath(bool isFolder, string parentItemName, string itemName, string itemId, string text)
	{
		IsFolder = isFolder;
		ParentItemType = parentItemName;
		ItemType = itemName;
		Id = itemId;
		Text = text;
	}

	public static ConfigurationItemPath CreatePath(string path)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (string.Equals(path, "/", StringComparison.Ordinal))
		{
			return new ConfigurationItemPath(isFolder: false, string.Empty, "System", string.Empty, path);
		}
		Match match = Regex.Match(path, "^(?<ParentEntityName>[a-zA-Z]+)\\[(?<Id>[0-9]+|[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12})\\]/(?<FolderEntityName>[a-zA-Z]+Folder)$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.CultureInvariant);
		if (match.Success)
		{
			return new ConfigurationItemPath(isFolder: true, match.Groups["ParentEntityName"].Value, match.Groups["FolderEntityName"].Value, match.Groups["Id"].Value, path);
		}
		match = Regex.Match(path, "^(?<EntityName>[a-zA-Z]+)\\[(?<Id>[0-9]+|[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12})\\]$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.CultureInvariant);
		if (match.Success)
		{
			return new ConfigurationItemPath(isFolder: false, string.Empty, match.Groups["EntityName"].Value, match.Groups["Id"].Value, path);
		}
		match = Regex.Match(path, "^/(?<FolderEntityName>[a-zA-Z]+Folder)$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.CultureInvariant);
		if (match.Success)
		{
			return new ConfigurationItemPath(isFolder: true, "System", match.Groups["FolderEntityName"].Value, string.Empty, path);
		}
		match = Regex.Match(path, "^(?<EntityName>[a-zA-Z]+)\\[(?<Id>S-\\d-(\\d+-){1,14}\\d+)\\]$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.CultureInvariant);
		if (match.Success)
		{
			return new ConfigurationItemPath(isFolder: false, string.Empty, match.Groups["EntityName"].Value, match.Groups["Id"].Value, path);
		}
		throw new ArgumentException("The 'path' is not a valid/recognized path:" + path, "path");
	}

	public static ConfigurationItemPath CreateSystemPath()
	{
		return new ConfigurationItemPathBuilder
		{
			ParentItemType = string.Empty,
			Id = string.Empty,
			ItemType = "System"
		}.CreatePath();
	}

	public static ConfigurationItemPath CreateSystemFolderPath(string folderType)
	{
		if (folderType == null)
		{
			throw new ArgumentNullException("folderType");
		}
		return new ConfigurationItemPathBuilder
		{
			ParentItemType = "System",
			Id = string.Empty,
			ItemType = folderType
		}.CreatePath();
	}

	public static ConfigurationItemPath CreateItemPath(string itemType, string id)
	{
		if (itemType == null)
		{
			throw new ArgumentNullException("itemType");
		}
		if (id == null)
		{
			throw new ArgumentNullException("id");
		}
		if (!Regex.IsMatch(id, "S-\\d-(?:\\d+-){1,14}\\d+", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.CultureInvariant) && !Regex.IsMatch(id, "^(?<Id>[0-9]+|[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12})$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.CultureInvariant))
		{
			throw new ArgumentException("The parameter is not a valid id.", "id");
		}
		return new ConfigurationItemPathBuilder
		{
			ParentItemType = string.Empty,
			Id = id,
			ItemType = itemType
		}.CreatePath();
	}

	public static ConfigurationItemPath CreateFolderPath(ConfigurationItemPath parentPath, string folderType)
	{
		return new ConfigurationItemPathBuilder(parentPath, folderType).CreatePath();
	}

	public static ConfigurationItemPath CreateFolderPath(string parentType, string parentId, string folderType)
	{
		return new ConfigurationItemPathBuilder
		{
			ParentItemType = parentType,
			Id = parentId,
			ItemType = folderType
		}.CreatePath();
	}
}
