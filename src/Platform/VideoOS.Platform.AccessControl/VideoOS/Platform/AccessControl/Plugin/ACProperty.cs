using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Plugin;

[Serializable]
[DataContract(Namespace = "")]
public sealed class ACProperty
{
	[DataMember]
	public string Key { get; private set; }

	[DataMember]
	public string Value { get; private set; }

	public ACProperty(string key, string value)
	{
		Key = key;
		Value = value;
	}

	public void InternalSetValue(string value)
	{
		Value = value;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ACProperty aCProperty))
		{
			return false;
		}
		if (Key == aCProperty.Key)
		{
			return Value == aCProperty.Value;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Key.GetHashCode();
	}
}
