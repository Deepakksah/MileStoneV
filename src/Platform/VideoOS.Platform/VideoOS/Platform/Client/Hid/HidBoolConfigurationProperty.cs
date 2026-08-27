using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Client.Hid;

[DataContract]
public sealed class HidBoolConfigurationProperty : HidConfigurationProperty
{
	[DataMember]
	public bool Value { get; set; }

	public HidBoolConfigurationProperty(Guid id, string name)
		: base(id, name)
	{
	}

	public override string ToString()
	{
		return Convert.ToString(Value, CultureInfo.InvariantCulture);
	}
}
