using System;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.Drivers;

[Serializable]
public class PtzPairDouble : IPtzName
{
	[XmlElement("name")]
	public string Name { get; set; }

	[XmlElement("value")]
	public double Value { get; set; }

	public PtzPairDouble()
	{
	}

	public PtzPairDouble(string name, double value)
	{
		Name = name;
		Value = value;
	}
}
