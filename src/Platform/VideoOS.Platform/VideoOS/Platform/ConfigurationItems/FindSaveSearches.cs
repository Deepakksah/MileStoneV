using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class FindSaveSearches : IConfigurationItem
{
	public int Count
	{
		get
		{
			if (int.TryParse(base.Wrapper.GetPropertyValue("Count"), out var result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("Count", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public int StartIndex
	{
		get
		{
			if (int.TryParse(base.Wrapper.GetPropertyValue("StartIndex"), out var result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("StartIndex", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public string KeywordFilter
	{
		get
		{
			return base.Wrapper.GetPropertyValue("KeywordFilter");
		}
		set
		{
			base.Wrapper.SetPropertyValue("KeywordFilter", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool PrivateOnly
	{
		get
		{
			if (!bool.TryParse(base.Wrapper.GetPropertyValue("PrivateOnly"), out var result))
			{
				return false;
			}
			return result;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PrivateOnly", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public ICollection<SaveSearches> SaveSearches
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<SaveSearches> list = new List<SaveSearches>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "SaveSearches")
				{
					list.Add(new SaveSearches(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public FindSaveSearches(ServerId serverId, ConfigurationItem item)
		: base(serverId, item)
	{
	}

	public FindSaveSearches(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public FindSaveSearches(FQID fqid)
		: base(fqid)
	{
	}
}
