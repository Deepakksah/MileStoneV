using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[DataContract(Name = "Property", Namespace = "urn:milestone-systems")]
public class Property
{
	[DataMember(Order = 1)]
	public string Key { get; set; }

	[DataMember(Order = 2)]
	public string Value { get; set; }

	public override bool Equals(object obj)
	{
		if (obj is Property property && property.Key == Key)
		{
			return property.Value == Value;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Key.GetHashCode();
	}
}
