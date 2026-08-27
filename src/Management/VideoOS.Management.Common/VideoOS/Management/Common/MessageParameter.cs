using System;

namespace VideoOS.Management.Common;

[Serializable]
public class MessageParameter
{
	public int Index { get; set; }

	public string Name { get; set; }

	public string Value { get; set; }

	public bool Localizable { get; set; }

	public string ResourceName { get; set; }

	public MessageParameter(int index, string name, string value)
	{
		Index = index;
		Name = name;
		Value = value;
		Localizable = false;
		ResourceName = string.Empty;
	}

	public MessageParameter(int index, string name, string value, bool localizable, string resourceName)
	{
		Index = index;
		Name = name;
		Value = value;
		Localizable = localizable;
		ResourceName = resourceName;
	}
}
