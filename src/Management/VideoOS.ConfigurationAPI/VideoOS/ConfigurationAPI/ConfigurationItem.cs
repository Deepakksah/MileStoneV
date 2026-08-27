using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.ConfigurationAPI;

[DebuggerDisplay("{ItemType}, {DisplayName}")]
[KnownType(typeof(ItemTypes))]
[DataContract]
public class ConfigurationItem
{
	[DataMember]
	public string DisplayName { get; set; }

	[DataMember]
	public EnablePropertyInfo EnableProperty { get; set; }

	[DataMember]
	public string ItemType { get; set; }

	[DataMember]
	public string ItemCategory { get; set; }

	[DataMember]
	public string Path { get; set; }

	[DataMember]
	public string ServerCookie { get; set; }

	[DataMember]
	public string ParentPath { get; set; }

	[DataMember]
	public bool ChildrenFilled { get; set; }

	[DataMember]
	public ConfigurationItem[] Children { get; set; }

	[DataMember]
	public int SortKey { get; set; }

	[DataMember]
	public string UIHint { get; set; }

	[DataMember]
	public string[] MethodIds { get; set; }

	[DataMember]
	public virtual Property[] Properties { get; set; }

	public ConfigurationItem()
	{
		SortKey = 100;
		ItemCategory = ItemCategories.Item;
		MethodIds = new string[0];
		Children = new ConfigurationItem[0];
	}
}
