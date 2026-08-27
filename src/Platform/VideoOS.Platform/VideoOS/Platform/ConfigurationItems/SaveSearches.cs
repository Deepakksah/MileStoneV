using System;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class SaveSearches : IConfigurationItem
{
	public enum SaveSearchAvailability
	{
		Private,
		Public
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public string SearchQuery
	{
		get
		{
			return base.Wrapper.GetPropertyValue("SearchQuery");
		}
		set
		{
			base.Wrapper.SetPropertyValue("SearchQuery", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public SaveSearchAvailability Availability
	{
		get
		{
			if (!Enum.TryParse<SaveSearchAvailability>(base.Wrapper.GetPropertyValue("Availability"), out var result))
			{
				return SaveSearchAvailability.Public;
			}
			return result;
		}
		set
		{
			base.Wrapper.SetPropertyValue("Availability", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	[Obsolete("Property is no longer used in the SmartClient; scope items are always included.")]
	public bool IncludesScopeItems
	{
		get
		{
			if (!bool.TryParse(base.Wrapper.GetPropertyValue("IncludesScopeItems"), out var result))
			{
				return false;
			}
			return result;
		}
		set
		{
			base.Wrapper.SetPropertyValue("IncludesScopeItems", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public SaveSearches(ServerId serverId, ConfigurationItem item)
		: base(serverId, item)
	{
	}

	public SaveSearches(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public SaveSearches(FQID fqid)
		: base(fqid)
	{
	}
}
