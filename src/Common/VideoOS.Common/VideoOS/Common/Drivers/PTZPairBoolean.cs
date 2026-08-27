using System;
using System.Xml.Serialization;

namespace VideoOS.Common.Drivers;

[Serializable]
public class PTZPairBoolean : IPTZName
{
	private string _name;

	[XmlElement("value")]
	public bool Value;

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

	public PTZPairBoolean()
	{
	}

	public PTZPairBoolean(string name, bool value)
	{
		_name = name;
		Value = value;
	}
}
