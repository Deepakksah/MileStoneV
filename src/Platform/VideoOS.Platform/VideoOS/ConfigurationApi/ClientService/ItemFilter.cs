using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.ConfigurationApi.ClientService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ItemFilter", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.ConfigurationAPI")]
public class ItemFilter : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private EnableFilter EnableFilterField;

	private string ItemTypeField;

	private PropertyFilter[] PropertyFiltersField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember]
	public EnableFilter EnableFilter
	{
		get
		{
			return EnableFilterField;
		}
		set
		{
			EnableFilterField = value;
		}
	}

	[DataMember]
	public string ItemType
	{
		get
		{
			return ItemTypeField;
		}
		set
		{
			ItemTypeField = value;
		}
	}

	[DataMember]
	public PropertyFilter[] PropertyFilters
	{
		get
		{
			return PropertyFiltersField;
		}
		set
		{
			PropertyFiltersField = value;
		}
	}

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
