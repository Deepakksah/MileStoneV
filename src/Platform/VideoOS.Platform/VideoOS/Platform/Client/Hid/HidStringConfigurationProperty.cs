using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Client.Hid;

[DataContract]
public sealed class HidStringConfigurationProperty : HidConfigurationProperty
{
	[DataMember]
	public string Value { get; set; }

	public string PlaceholderText { get; set; }

	public HidStringConfigurationProperty(Guid id, string name)
		: base(id, name)
	{
	}

	public override string ToString()
	{
		return Value;
	}
}
