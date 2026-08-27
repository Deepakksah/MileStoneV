using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Client.Hid;

[DataContract]
public sealed class HidIntConfigurationProperty : HidConfigurationProperty
{
	[DataMember]
	public int Value { get; set; }

	public string PlaceholderText { get; set; }

	public HidIntConfigurationProperty(Guid id, string name)
		: base(id, name)
	{
	}

	public override string ToString()
	{
		return Convert.ToString(Value, CultureInfo.InvariantCulture);
	}
}
