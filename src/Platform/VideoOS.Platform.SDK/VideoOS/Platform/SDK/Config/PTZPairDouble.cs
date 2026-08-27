using System;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Config;

[Serializable]
public class PTZPairDouble : IPTZName
{
	private string _name;

	[XmlElement("value")]
	public double Value;

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

	public PTZPairDouble()
	{
	}

	public PTZPairDouble(string name, double value)
	{
		_name = name;
		Value = value;
	}
}
