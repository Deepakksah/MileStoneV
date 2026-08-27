using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Client.Hid;

[DataContract]
public sealed class HidEnumConfigurationProperty : HidConfigurationProperty
{
	public string[] EnumOptions { get; private set; }

	[DataMember]
	public string Value { get; set; }

	public string PlaceholderText { get; set; }

	public HidEnumConfigurationProperty(Guid id, string name, params string[] enumOptions)
		: base(id, name)
	{
		EnumOptions = enumOptions;
	}

	public override string ToString()
	{
		return Value;
	}
}
