using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace VideoOS.ConfigurationApi.ClientService;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ConfigurationItem", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.ConfigurationAPI")]
public class ConfigurationItem : IExtensibleDataObject
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	private ConfigurationItem[] ChildrenField;

	private bool ChildrenFilledField;

	private string DisplayNameField;

	[NonSerialized]
	private EnablePropertyInfo EnablePropertyField;

	private string ItemCategoryField;

	private string ItemTypeField;

	private string[] MethodIdsField;

	private string ParentPathField;

	private string PathField;

	private Property[] PropertiesField;

	private string ServerCookieField;

	private int SortKeyField;

	private string UIHintField;

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
	public ConfigurationItem[] Children
	{
		get
		{
			return ChildrenField;
		}
		set
		{
			ChildrenField = value;
		}
	}

	[DataMember]
	public bool ChildrenFilled
	{
		get
		{
			return ChildrenFilledField;
		}
		set
		{
			ChildrenFilledField = value;
		}
	}

	[DataMember]
	public string DisplayName
	{
		get
		{
			return DisplayNameField;
		}
		set
		{
			DisplayNameField = value;
		}
	}

	[DataMember]
	public EnablePropertyInfo EnableProperty
	{
		get
		{
			return EnablePropertyField;
		}
		set
		{
			EnablePropertyField = value;
		}
	}

	[DataMember]
	public string ItemCategory
	{
		get
		{
			return ItemCategoryField;
		}
		set
		{
			ItemCategoryField = value;
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
	public string[] MethodIds
	{
		get
		{
			return MethodIdsField;
		}
		set
		{
			MethodIdsField = value;
		}
	}

	[DataMember]
	public string ParentPath
	{
		get
		{
			return ParentPathField;
		}
		set
		{
			ParentPathField = value;
		}
	}

	[DataMember]
	public string Path
	{
		get
		{
			return PathField;
		}
		set
		{
			PathField = value;
		}
	}

	[DataMember]
	public Property[] Properties
	{
		get
		{
			return PropertiesField;
		}
		set
		{
			PropertiesField = value;
		}
	}

	[DataMember]
	public string ServerCookie
	{
		get
		{
			return ServerCookieField;
		}
		set
		{
			ServerCookieField = value;
		}
	}

	[DataMember]
	public int SortKey
	{
		get
		{
			return SortKeyField;
		}
		set
		{
			SortKeyField = value;
		}
	}

	[DataMember]
	public string UIHint
	{
		get
		{
			return UIHintField;
		}
		set
		{
			UIHintField = value;
		}
	}

	public ConfigurationItem CloneWithOnlyChildItemChildren()
	{
		return new ConfigurationItem
		{
			ChildrenFilled = true,
			DisplayName = DisplayName,
			EnableProperty = EnableProperty,
			ItemCategory = ItemCategory,
			ItemType = ItemType,
			MethodIds = (MethodIds?.Clone() as string[]),
			ParentPath = ParentPath,
			Path = Path,
			Properties = (Properties?.Clone() as Property[]),
			ServerCookie = ServerCookie,
			SortKey = SortKey,
			UIHint = UIHint,
			Children = Children?.Where((ConfigurationItem c) => c.ItemCategory == "ChildItem").ToArray()
		};
	}

	public ConfigurationItem CloneWithNewProperties(Property[] properties)
	{
		return new ConfigurationItem
		{
			ChildrenFilled = true,
			DisplayName = DisplayName,
			EnableProperty = EnableProperty,
			ItemCategory = ItemCategory,
			ItemType = ItemType,
			MethodIds = (MethodIds?.Clone() as string[]),
			ParentPath = ParentPath,
			Path = Path,
			Properties = properties,
			ServerCookie = ServerCookie,
			SortKey = SortKey,
			UIHint = UIHint,
			Children = Children?.Where((ConfigurationItem c) => c.ItemCategory == "ChildItem").ToArray()
		};
	}
}
