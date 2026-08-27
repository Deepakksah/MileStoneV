using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.ConfigurationAPI;

[DataContract]
[DebuggerDisplay("PropertyFilter: {Key} {Operator} {Value}")]
public class PropertyFilter
{
	[DataMember]
	public string Key { get; set; }

	[DataMember]
	public Operator Operator { get; set; }

	[DataMember]
	public string Value { get; set; }

	public PropertyFilter()
	{
	}

	public PropertyFilter(string key, Operator opr, string value)
	{
		Key = key;
		Operator = opr;
		Value = value;
	}

	public override string ToString()
	{
		return $"{Key} {Operator} {Value}";
	}
}
