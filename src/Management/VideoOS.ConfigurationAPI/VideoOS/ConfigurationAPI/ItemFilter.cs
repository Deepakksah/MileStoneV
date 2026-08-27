using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.ConfigurationAPI;

[DataContract]
[DebuggerDisplay("{ItemType} Enabled={EnableFilter} ")]
public class ItemFilter
{
	[DataMember]
	public string ItemType { get; set; }

	[DataMember]
	public PropertyFilter[] PropertyFilters { get; set; }

	[DataMember]
	public EnableFilter EnableFilter { get; set; }

	public ItemFilter()
	{
	}

	public ItemFilter(string itemType, PropertyFilter[] propertyFilters, EnableFilter enableFilter)
	{
		ItemType = itemType;
		PropertyFilters = propertyFilters;
		EnableFilter = enableFilter;
	}

	public ItemFilter(string itemType, PropertyFilter[] propertyFilters)
	{
		ItemType = itemType;
		PropertyFilters = propertyFilters;
		EnableFilter = EnableFilter.Enabled;
	}
}
