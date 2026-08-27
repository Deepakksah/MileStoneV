using System;
using System.IO;
using System.Xml;

namespace VideoOS.Common.Integration;

public class PtzSetting
{
	private string _typeId;

	private string _settingName;

	private bool _absolute;

	private bool _relative;

	private bool _automatic;

	private bool _start;

	private bool _stop;

	private bool _speed;

	public string TypeId => _typeId;

	public string SettingName => _settingName;

	public bool Absolute => _absolute;

	public bool Relative => _relative;

	public bool Automatic => _automatic;

	public bool Start => _start;

	public bool Stop => _stop;

	public bool Speed => _speed;

	public PtzSetting(string settingName, string typeId, string xml)
	{
		bool? flag = null;
		bool? flag2 = null;
		bool? flag3 = null;
		bool? flag4 = null;
		bool? flag5 = null;
		bool? flag6 = null;
		try
		{
			using StringReader input = new StringReader(xml);
			using XmlReader xmlReader = XmlReader.Create(input, new XmlReaderSettings
			{
				IgnoreComments = true,
				IgnoreProcessingInstructions = true,
				IgnoreWhitespace = true,
				XmlResolver = null
			});
			while (xmlReader.Read())
			{
				if (xmlReader.IsStartElement())
				{
					switch (xmlReader.Name)
					{
					case "absolutesupport":
						flag = bool.Parse(xmlReader.ReadString());
						break;
					case "relativesupport":
						flag2 = bool.Parse(xmlReader.ReadString());
						break;
					case "stopsupport":
						flag3 = bool.Parse(xmlReader.ReadString());
						break;
					case "startsupport":
						flag4 = bool.Parse(xmlReader.ReadString());
						break;
					case "automaticsupport":
						flag5 = bool.Parse(xmlReader.ReadString());
						break;
					case "speedsupport":
						flag6 = bool.Parse(xmlReader.ReadString());
						break;
					}
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception inner)
		{
			throw new InvalidPtzXmlException("Failed to parse the xml properly", inner);
		}
		if (!flag.HasValue || !flag2.HasValue || !flag3.HasValue || !flag4.HasValue || !flag5.HasValue || !flag6.HasValue)
		{
			throw new InvalidPtzXmlException();
		}
		_absolute = flag.Value;
		_relative = flag2.Value;
		_stop = flag3.Value;
		_start = flag4.Value;
		_automatic = flag5.Value;
		_speed = flag6.Value;
		_settingName = settingName;
		_typeId = typeId;
	}
}
