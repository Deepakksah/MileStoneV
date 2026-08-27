using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using VideoOS.Common.Properties;

namespace VideoOS.Common.Drivers;

[Serializable]
public class PTZArgs : IPTZArgs
{
	public bool Normalized = true;

	[XmlIgnore]
	private const string PrimaryXmlTag = "ptzargs";

	[XmlElement(typeof(PTZPairDouble))]
	public ArrayList movement { get; private set; } = new ArrayList();

	[XmlElement(typeof(PTZPairDouble))]
	public ArrayList speed { get; private set; } = new ArrayList();

	[XmlElement(typeof(PTZPairBoolean))]
	public ArrayList automatic { get; private set; } = new ArrayList();

	public bool isPanAvailable => ContainsKey(movement, "pan");

	public bool isTiltAvailable => ContainsKey(movement, "tilt");

	public bool isZoomAvailable => ContainsKey(movement, "zoom");

	public bool isPanSpeedAvailable => ContainsKey(speed, "pan");

	public bool isTiltSpeedAvailable => ContainsKey(speed, "tilt");

	public bool isZoomSpeedAvailable => ContainsKey(speed, "zoom");

	[XmlIgnore]
	public double Pan
	{
		get
		{
			if (!isPanAvailable)
			{
				return double.NaN;
			}
			return GetDoubleValue(movement, "pan");
		}
		set
		{
			if (!double.IsNaN(value))
			{
				SetValue(movement, "pan", value);
			}
		}
	}

	[XmlIgnore]
	public double Tilt
	{
		get
		{
			if (!isTiltAvailable)
			{
				return double.NaN;
			}
			return GetDoubleValue(movement, "tilt");
		}
		set
		{
			if (!double.IsNaN(value))
			{
				SetValue(movement, "tilt", value);
			}
		}
	}

	[XmlIgnore]
	public double Zoom
	{
		get
		{
			if (!isZoomAvailable)
			{
				return double.NaN;
			}
			return GetDoubleValue(movement, "zoom");
		}
		set
		{
			if (!double.IsNaN(value))
			{
				SetValue(movement, "zoom", value);
			}
		}
	}

	[XmlIgnore]
	public double PanSpeed
	{
		get
		{
			if (!isPanSpeedAvailable)
			{
				return double.NaN;
			}
			return GetDoubleValue(speed, "pan");
		}
		set
		{
			if (!double.IsNaN(value))
			{
				SetValue(speed, "pan", value);
			}
		}
	}

	[XmlIgnore]
	public double TiltSpeed
	{
		get
		{
			if (!isTiltSpeedAvailable)
			{
				return double.NaN;
			}
			return GetDoubleValue(speed, "tilt");
		}
		set
		{
			if (!double.IsNaN(value))
			{
				SetValue(speed, "tilt", value);
			}
		}
	}

	[XmlIgnore]
	public double ZoomSpeed
	{
		get
		{
			if (!isZoomSpeedAvailable)
			{
				return double.NaN;
			}
			return GetDoubleValue(speed, "zoom");
		}
		set
		{
			if (!double.IsNaN(value))
			{
				SetValue(speed, "zoom", value);
			}
		}
	}

	public double this[string name]
	{
		get
		{
			return GetMovement(name);
		}
		set
		{
			SetMovement(name, value);
		}
	}

	public PTZArgs()
	{
	}

	public override string ToString()
	{
		return SaveAsXml();
	}

	public PTZArgs(PTZArgs args)
	{
		movement = (ArrayList)args.movement.Clone();
		speed = (ArrayList)args.speed.Clone();
		automatic = (ArrayList)args.automatic.Clone();
	}

	public PTZArgs(string Xml)
	{
		LoadXml(Xml);
	}

	public void LoadXml(string Xml)
	{
		XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(Xml));
		LoadXmlFragment(xmlTextReader);
		xmlTextReader.Close();
	}

	public void LoadXmlFragment(XmlTextReader xtr)
	{
		string text = "";
		string text2 = "";
		string name = "";
		string value = "";
		while (xtr.Read())
		{
			if (xtr.NodeType == XmlNodeType.Element)
			{
				text = xtr.Name;
			}
			else if (xtr.NodeType == XmlNodeType.EndElement)
			{
				switch (xtr.Name)
				{
				case "movement":
					SetMovement(name, Convert.ToDouble(value, CultureInfo.InvariantCulture));
					break;
				case "speed":
					SetSpeed(name, Convert.ToDouble(value, CultureInfo.InvariantCulture));
					break;
				case "automatic":
					SetAuto(name, Convert.ToBoolean(value, CultureInfo.InvariantCulture));
					break;
				case "ptzargs":
					return;
				case "preset":
					return;
				}
			}
			else
			{
				if (xtr.NodeType != XmlNodeType.Text)
				{
					continue;
				}
				if (!(text == "name"))
				{
					if (text == "value")
					{
						value = xtr.Value;
					}
				}
				else
				{
					name = xtr.Value;
				}
			}
		}
	}

	public PTZArgs(Array names, Array values)
	{
		if (names.Length != values.Length)
		{
			throw new Exception(Resources.InvalidArrayLength);
		}
		for (int i = 0; i < names.Length; i++)
		{
			SetValue(movement, Convert.ToString(names.GetValue(i), CultureInfo.InvariantCulture), Convert.ToDouble(values.GetValue(i), CultureInfo.InvariantCulture));
		}
	}

	public PTZArgs(Array names, Array values, Array speedvalues)
	{
		if (names.Length != values.Length)
		{
			throw new Exception(Resources.InvalidArrayLength);
		}
		for (int i = 0; i < names.Length; i++)
		{
			SetValue(movement, Convert.ToString(names.GetValue(i), CultureInfo.InvariantCulture), Convert.ToDouble(values.GetValue(i), CultureInfo.InvariantCulture));
		}
		for (int j = 0; j < names.Length; j++)
		{
			SetValue(speed, Convert.ToString(names.GetValue(j), CultureInfo.InvariantCulture), Convert.ToDouble(values.GetValue(j), CultureInfo.InvariantCulture));
		}
	}

	public string SaveAsXml()
	{
		StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
		XmlTextWriter wr = new XmlTextWriter(stringWriter);
		SaveAsXml(wr);
		stringWriter.Flush();
		return stringWriter.ToString();
	}

	public void SaveAsXml(XmlTextWriter wr)
	{
		SaveAsXml(wr, "ptzargs");
	}

	public string SaveAsXMLFragment(string primaryTag)
	{
		StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
		XmlTextWriter wr = new XmlTextWriter(stringWriter);
		SaveAsXml(wr, primaryTag);
		stringWriter.Flush();
		return stringWriter.ToString();
	}

	private void SaveAsXml(XmlTextWriter wr, string primarytag)
	{
		wr.WriteStartElement(primarytag);
		foreach (PTZPairDouble item in movement)
		{
			wr.WriteStartElement("movement");
			string name = item.Name;
			string value = Convert.ToString(Convert.ToDouble(item.Value), CultureInfo.InvariantCulture);
			wr.WriteElementString("name", name);
			wr.WriteElementString("value", value);
			wr.WriteEndElement();
		}
		foreach (PTZPairDouble item2 in speed)
		{
			wr.WriteStartElement("speed");
			string name2 = item2.Name;
			string value2 = Convert.ToString(Convert.ToDouble(item2.Value), CultureInfo.InvariantCulture);
			wr.WriteElementString("name", name2);
			wr.WriteElementString("value", value2);
			wr.WriteEndElement();
		}
		foreach (PTZPairBoolean item3 in automatic)
		{
			wr.WriteStartElement("automatic");
			string name3 = item3.Name;
			string value3 = (Convert.ToBoolean(item3.Value) ? "true" : "false");
			wr.WriteElementString("name", name3);
			wr.WriteElementString("value", value3);
			wr.WriteEndElement();
		}
		wr.WriteEndElement();
	}

	public bool isAutoDefined(string name)
	{
		return ContainsKey(automatic, name);
	}

	public bool isAutoDefined(Guid guid)
	{
		if (!ContainsKey(automatic, guid.ToString().ToUpper(CultureInfo.InvariantCulture)))
		{
			return ContainsKey(automatic, guid.ToString().ToUpper(CultureInfo.InvariantCulture));
		}
		return true;
	}

	public bool GetAuto(string name)
	{
		if (isAutoDefined(name))
		{
			return Convert.ToBoolean(GetBooleanValue(automatic, name));
		}
		return false;
	}

	public void SetAuto(string name, bool value)
	{
		SetValue(automatic, name, value);
	}

	public void RemoveAuto(string name)
	{
		if (!ContainsKey(automatic, name))
		{
			return;
		}
		foreach (PTZPairBoolean item in automatic)
		{
			if (item.Name == name)
			{
				automatic.Remove(item);
				break;
			}
		}
	}

	public void SetAuto(Guid guid, bool value)
	{
		SetValue(automatic, guid.ToString(), value);
	}

	public void SetMovement(string name, double value)
	{
		if (!double.IsNaN(value))
		{
			SetValue(movement, name, value);
		}
	}

	public void SetSpeed(string name, double value)
	{
		if (!double.IsNaN(value))
		{
			SetValue(speed, name, value);
		}
	}

	public bool isAvailable(string name)
	{
		return ContainsKey(movement, name);
	}

	public bool isSpeedAvailable(string name)
	{
		return ContainsKey(speed, name);
	}

	public double GetMovement(string name)
	{
		if (!ContainsKey(movement, name))
		{
			return double.NaN;
		}
		return GetDoubleValue(movement, name);
	}

	public double GetSpeed(string name)
	{
		if (!ContainsKey(movement, name))
		{
			return double.NaN;
		}
		return GetDoubleValue(speed, name);
	}

	private bool ContainsKey(ArrayList list, string name)
	{
		foreach (IPTZName item in list)
		{
			if (item.Name == name)
			{
				return true;
			}
		}
		return false;
	}

	private static void SetValue(ArrayList list, string name, double value)
	{
		foreach (PTZPairDouble item in list)
		{
			if (item.Name == name)
			{
				item.Value = value;
				return;
			}
		}
		list.Add(new PTZPairDouble(name, value));
	}

	private static void SetValue(ArrayList list, string name, bool value)
	{
		foreach (PTZPairBoolean item in list)
		{
			if (item.Name == name)
			{
				item.Value = value;
				return;
			}
		}
		list.Add(new PTZPairBoolean(name, value));
	}

	private static double GetDoubleValue(ArrayList list, string name)
	{
		foreach (PTZPairDouble item in list)
		{
			if (item.Name == name)
			{
				return item.Value;
			}
		}
		return double.NaN;
	}

	private static bool GetBooleanValue(ArrayList list, string name)
	{
		foreach (PTZPairBoolean item in list)
		{
			if (item.Name == name)
			{
				return item.Value;
			}
		}
		return false;
	}
}
