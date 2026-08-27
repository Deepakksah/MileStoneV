using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace VideoOS.Management.Server.ConfigurationApi;

public class ConfigurationItemPathBuilder
{
	private string parentItemType;

	private string itemType;

	private string id;

	public string ParentItemType
	{
		get
		{
			return parentItemType;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			parentItemType = value;
		}
	}

	public string ItemType
	{
		get
		{
			return itemType;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			itemType = value;
		}
	}

	public string Id
	{
		get
		{
			return id;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			id = value;
		}
	}

	public ConfigurationItemPathBuilder()
	{
		ParentItemType = string.Empty;
		ItemType = string.Empty;
		Id = string.Empty;
	}

	public ConfigurationItemPathBuilder(string path)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(path);
		ParentItemType = configurationItemPath.ParentItemType;
		ItemType = configurationItemPath.ItemType;
		Id = configurationItemPath.Id;
	}

	public ConfigurationItemPathBuilder(ConfigurationItemPath path)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		ParentItemType = path.ParentItemType;
		ItemType = path.ItemType;
		Id = path.Id;
	}

	public ConfigurationItemPathBuilder(ConfigurationItemPath parentPath, string itemType)
	{
		if (parentPath == null)
		{
			throw new ArgumentNullException("parentPath");
		}
		if (itemType == null)
		{
			throw new ArgumentNullException("itemType");
		}
		ParentItemType = parentPath.ItemType;
		id = parentPath.Id;
		ItemType = itemType;
	}

	public ConfigurationItemPathBuilder(string itemType, string id)
	{
		ParentItemType = string.Empty;
		ItemType = itemType;
		Id = id;
	}

	public ConfigurationItemPathBuilder(string itemType, string id, string parentItemType)
	{
		ParentItemType = parentItemType;
		ItemType = itemType;
		Id = id;
	}

	public ConfigurationItemPath CreatePath()
	{
		if (string.IsNullOrWhiteSpace(ItemType))
		{
			throw new CreateConfigurationItemPathException("The parameter item type cannot be empty.");
		}
		if (string.Equals(ItemType, "System", StringComparison.Ordinal))
		{
			if (!string.IsNullOrEmpty(Id))
			{
				throw new CreateConfigurationItemPathException("The parameter id must be empty.");
			}
			if (!string.IsNullOrEmpty(ParentItemType))
			{
				throw new CreateConfigurationItemPathException("The parameter parent item type must be empty.");
			}
			string text = "/";
			return new ConfigurationItemPath(isFolder: false, string.Empty, "System", string.Empty, text);
		}
		if (string.Equals(ParentItemType, "System", StringComparison.Ordinal))
		{
			if (!string.IsNullOrEmpty(Id))
			{
				throw new CreateConfigurationItemPathException("The parameter id must be empty.");
			}
			if (!Regex.IsMatch(ItemType, "^(?<FolderEntityName>[a-z]+Folder)$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.CultureInvariant))
			{
				throw new CreateConfigurationItemPathException("The parameter item type must be a valid folder name.");
			}
			string text2 = string.Format(CultureInfo.InvariantCulture, "/{0}", ItemType);
			return new ConfigurationItemPath(isFolder: true, "System", ItemType, string.Empty, text2);
		}
		if (Regex.IsMatch(ParentItemType, "(?<ParentEntityName>[a-z]+)$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.CultureInvariant))
		{
			if (!Regex.IsMatch(id, "(?<Id>[0-9]+|[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12})", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.CultureInvariant))
			{
				throw new CreateConfigurationItemPathException("The parameter id must be a valid integer or guid.");
			}
			if (!Regex.IsMatch(ItemType, "^(?<FolderEntityName>[a-z]+Folder)$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.CultureInvariant))
			{
				throw new CreateConfigurationItemPathException("The parameter item type must be a valid folder name.");
			}
			string text3 = string.Format(CultureInfo.InvariantCulture, "{0}[{1}]/{2}", ParentItemType, Id, ItemType);
			return new ConfigurationItemPath(isFolder: true, ParentItemType, ItemType, Id, text3);
		}
		if (string.IsNullOrWhiteSpace(ParentItemType))
		{
			if (!Regex.IsMatch(id, "(?<Id>[0-9]+|[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12})", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.CultureInvariant))
			{
				throw new CreateConfigurationItemPathException("The parameter id must be a valid integer or guid.");
			}
			if (!Regex.IsMatch(ItemType, "^(?<EntityName>[a-z]+)$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.CultureInvariant))
			{
				throw new CreateConfigurationItemPathException("The parameter item type must be a valid item type name.");
			}
			string text4 = string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", ItemType, Id);
			return new ConfigurationItemPath(isFolder: false, string.Empty, ItemType, Id, text4);
		}
		throw new CreateConfigurationItemPathException("Unable to validate the values for creating a configuration path.");
	}
}
