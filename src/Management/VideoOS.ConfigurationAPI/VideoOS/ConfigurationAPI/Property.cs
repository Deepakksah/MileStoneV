using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.ConfigurationAPI;

[DataContract]
[DebuggerDisplay("{Key}, {DebugDisplayValue}")]
public class Property
{
	public string DebugDisplayValue
	{
		get
		{
			if (ValueArray == null)
			{
				return Value;
			}
			return string.Join(", ", ValueArray);
		}
	}

	[DataMember]
	public string DisplayName { get; set; }

	[DataMember]
	public string Key { get; set; }

	[DataMember]
	public string Value { get; set; }

	[DataMember]
	public string[] ValueArray { get; set; }

	[DataMember]
	public bool ServerValidation { get; set; }

	[DataMember]
	public bool IsSettable { get; set; }

	[DataMember]
	public int UIImportance { get; set; }

	[DataMember]
	public int SortKey { get; set; }

	[DataMember]
	public string TranslationId { get; set; }

	[DataMember]
	public string ToolTipTranslationId { get; set; }

	[DataMember]
	public bool ItemReference { get; set; }

	[DataMember]
	public string ValueType { get; set; }

	[DataMember]
	public string SelectedValueTypeInfoName { get; set; }

	[DataMember]
	public ValueTypeInfo[] ValueTypeInfos { get; set; }

	public Property()
	{
		UIImportance = 0;
		ValueType = "String";
		ServerValidation = false;
		SortKey = 100;
	}

	public Property(string name, string value)
	{
		DisplayName = name;
		Key = name;
		Value = value;
		ServerValidation = false;
		UIImportance = 0;
		ValueType = "String";
		SortKey = 100;
	}

	public Property(string key, string name, string value)
	{
		DisplayName = name;
		Key = key;
		Value = value;
		ServerValidation = false;
		UIImportance = 0;
		ValueType = "String";
		SortKey = 100;
	}

	public Property(string name, string[] valueArray)
		: this(name, name, valueArray)
	{
	}

	public Property(string key, string name, string[] valueArray)
	{
		DisplayName = name;
		Key = key;
		ValueArray = valueArray;
		ValueType = "Array";
		ServerValidation = false;
		UIImportance = 0;
		ValueType = "String";
		SortKey = 100;
	}
}
