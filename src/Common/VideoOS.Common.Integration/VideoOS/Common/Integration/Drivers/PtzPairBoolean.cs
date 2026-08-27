using System;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.Drivers;

[Serializable]
public class PtzPairBoolean : IPtzName
{
	private string _name;

	[XmlElement("name")]
	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
		}
	}

	[XmlElement("value")]
	public bool Value { get; set; }

	public PtzPairBoolean()
	{
	}

	public PtzPairBoolean(string name, bool value)
	{
		_name = name;
		Value = value;
	}
}
