using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace VideoOS.Management.Common;

public class PatrollingPosition
{
	private PatrollingPreset _preset = new PatrollingPreset();

	private TimeSpan _waitTime;

	public PatrollingPreset Preset => _preset;

	public TimeSpan WaitTime
	{
		get
		{
			return _waitTime;
		}
		set
		{
			_waitTime = value;
		}
	}

	public void LoadXml(XmlNode positionNode)
	{
		_preset.Key = positionNode.SelectSingleNode("PresetKey").InnerText;
		_waitTime = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(positionNode.SelectSingleNode("WaitTime").InnerText, CultureInfo.InvariantCulture));
	}

	public string ToXml()
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder, CultureInfo.InvariantCulture));
		xmlTextWriter.WriteStartElement("StartPosition");
		xmlTextWriter.WriteElementString("PresetKey", _preset.Key);
		xmlTextWriter.WriteElementString("WaitTime", _waitTime.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.Close();
		return stringBuilder.ToString();
	}

	public void Validate()
	{
		if (_waitTime < TimeSpan.Zero)
		{
			throw new FormatException("Wait time can't be less than zero");
		}
	}
}
