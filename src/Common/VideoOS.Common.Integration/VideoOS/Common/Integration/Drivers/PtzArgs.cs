using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.Drivers;

[Serializable]
public class PtzArgs
{
	[XmlElement(typeof(PtzPairDouble))]
	private ArrayList movementList;

	[XmlElement(typeof(PtzPairDouble))]
	private ArrayList speedList;

	[XmlElement(typeof(PtzPairBoolean))]
	private ArrayList automaticList;

	[XmlIgnore]
	private const string PrimaryXmlTag = "ptzargs";

	public bool Normalized { get; set; }

	public bool PanAvailable => ContainsKey(movementList, "pan");

	public bool TiltAvailable => ContainsKey(movementList, "tilt");

	public bool ZoomAvailable => ContainsKey(movementList, "zoom");

	public bool PanSpeedAvailable => ContainsKey(speedList, "pan");

	public bool TiltSpeedAvailable => ContainsKey(speedList, "tilt");

	public bool ZoomSpeedAvailable => ContainsKey(speedList, "zoom");

	[XmlIgnore]
	public double Pan
	{
		get
		{
			return GetDoubleValue(movementList, "pan");
		}
		set
		{
			if (!double.IsNaN(value))
			{
				SetValue(movementList, "pan", value);
			}
		}
	}

	[XmlIgnore]
	public double Tilt
	{
		get
		{
			return GetDoubleValue(movementList, "tilt");
		}
		set
		{
			if (!double.IsNaN(value))
			{
				SetValue(movementList, "tilt", value);
			}
		}
	}

	[XmlIgnore]
	public double Zoom
	{
		get
		{
			return GetDoubleValue(movementList, "zoom");
		}
		set
		{
			if (!double.IsNaN(value))
			{
				SetValue(movementList, "zoom", value);
			}
		}
	}

	[XmlIgnore]
	public double PanSpeed
	{
		get
		{
			return GetDoubleValue(speedList, "pan");
		}
		set
		{
			if (!double.IsNaN(value))
			{
				SetValue(speedList, "pan", value);
			}
		}
	}

	[XmlIgnore]
	public double TiltSpeed
	{
		get
		{
			return GetDoubleValue(speedList, "tilt");
		}
		set
		{
			if (!double.IsNaN(value))
			{
				SetValue(speedList, "tilt", value);
			}
		}
	}

	[XmlIgnore]
	public double ZoomSpeed
	{
		get
		{
			return GetDoubleValue(speedList, "zoom");
		}
		set
		{
			if (!double.IsNaN(value))
			{
				SetValue(speedList, "zoom", value);
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

	public ArrayList GetMovementList()
	{
		return (ArrayList)movementList.Clone();
	}

	public ArrayList GetSpeedList()
	{
		return (ArrayList)speedList.Clone();
	}

	public ArrayList GetAutomaticList()
	{
		return (ArrayList)automaticList.Clone();
	}

	public PtzArgs()
	{
		movementList = new ArrayList();
		speedList = new ArrayList();
		automaticList = new ArrayList();
		Normalized = true;
	}

	public PtzArgs(PtzArgs args)
	{
		if (args == null)
		{
			throw new ArgumentNullException("args");
		}
		Normalized = true;
		movementList = new ArrayList();
		speedList = new ArrayList();
		automaticList = new ArrayList();
		string xml = args.SaveAsXml();
		LoadXml(xml);
	}

	public PtzArgs Clone()
	{
		return new PtzArgs(this);
	}

	public PtzArgs(string xml)
	{
		movementList = new ArrayList();
		speedList = new ArrayList();
		automaticList = new ArrayList();
		Normalized = true;
		LoadXml(xml);
	}

	public void LoadXml(string xml)
	{
		using StringReader input = new StringReader(xml);
		using XmlTextReader xmlTextReader = new XmlTextReader(input);
		LoadXmlFragment(xmlTextReader);
		xmlTextReader.Close();
	}

	public void LoadXmlFragment(XmlTextReader xmlTextReader)
	{
		if (xmlTextReader == null)
		{
			throw new ArgumentNullException("xmlTextReader");
		}
		string text = "";
		string text2 = "";
		string name = "";
		string value = "";
		while (xmlTextReader.Read())
		{
			if (xmlTextReader.NodeType == XmlNodeType.Element)
			{
				text = xmlTextReader.LocalName;
			}
			else if (xmlTextReader.NodeType == XmlNodeType.EndElement)
			{
				switch (xmlTextReader.LocalName)
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
				if (xmlTextReader.NodeType != XmlNodeType.Text)
				{
					continue;
				}
				if (!(text == "name"))
				{
					if (text == "value")
					{
						value = xmlTextReader.Value;
					}
				}
				else
				{
					name = xmlTextReader.Value;
				}
			}
		}
	}

	public string SaveAsXml()
	{
		using StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
		using XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
		SaveAsXml(xmlTextWriter);
		stringWriter.Flush();
		return stringWriter.ToString();
	}

	public void SaveAsXml(XmlTextWriter xmlTextWriter)
	{
		SaveAsXml(xmlTextWriter, "ptzargs");
	}

	public string SaveAsXmlFragment(string primaryTag)
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
		foreach (PtzPairDouble movement in movementList)
		{
			wr.WriteStartElement("movement");
			string name = movement.Name;
			string value = Convert.ToString(Convert.ToDouble(movement.Value), CultureInfo.InvariantCulture);
			wr.WriteElementString("name", name);
			wr.WriteElementString("value", value);
			wr.WriteEndElement();
		}
		foreach (PtzPairDouble speed in speedList)
		{
			wr.WriteStartElement("speed");
			string name2 = speed.Name;
			string value2 = Convert.ToString(Convert.ToDouble(speed.Value), CultureInfo.InvariantCulture);
			wr.WriteElementString("name", name2);
			wr.WriteElementString("value", value2);
			wr.WriteEndElement();
		}
		foreach (PtzPairBoolean automatic in automaticList)
		{
			wr.WriteStartElement("automatic");
			string name3 = automatic.Name;
			string value3 = (Convert.ToBoolean(automatic.Value) ? "true" : "false");
			wr.WriteElementString("name", name3);
			wr.WriteElementString("value", value3);
			wr.WriteEndElement();
		}
		wr.WriteEndElement();
	}

	public bool AutoDefined(string name)
	{
		return ContainsKey(automaticList, name);
	}

	public bool AutoDefined(Guid id)
	{
		return ContainsKey(automaticList, id.ToString().ToUpperInvariant());
	}

	public bool GetAuto(string name)
	{
		return GetBooleanValue(automaticList, name);
	}

	public void SetAuto(string name, bool value)
	{
		SetValue(automaticList, name, value);
	}

	public void SetAuto(Guid id, bool value)
	{
		SetValue(automaticList, id.ToString(), value);
	}

	public void SetMovement(string name, double value)
	{
		if (!double.IsNaN(value))
		{
			SetValue(movementList, name, value);
		}
	}

	public void SetSpeed(string name, double value)
	{
		if (!double.IsNaN(value))
		{
			SetValue(speedList, name, value);
		}
	}

	public bool Available(string name)
	{
		return ContainsKey(movementList, name);
	}

	public bool SpeedAvailable(string name)
	{
		return ContainsKey(speedList, name);
	}

	public double GetMovement(string name)
	{
		return GetDoubleValue(movementList, name);
	}

	public double GetSpeed(string name)
	{
		return GetDoubleValue(speedList, name);
	}

	private static bool ContainsKey(ArrayList list, string name)
	{
		foreach (IPtzName item in list)
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
		foreach (PtzPairDouble item in list)
		{
			if (item.Name == name)
			{
				item.Value = value;
				return;
			}
		}
		list.Add(new PtzPairDouble(name, value));
	}

	private static void SetValue(ArrayList list, string name, bool value)
	{
		foreach (PtzPairBoolean item in list)
		{
			if (item.Name == name)
			{
				item.Value = value;
				return;
			}
		}
		list.Add(new PtzPairBoolean(name, value));
	}

	private static double GetDoubleValue(ArrayList list, string name)
	{
		foreach (PtzPairDouble item in list)
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
		foreach (PtzPairBoolean item in list)
		{
			if (item.Name == name)
			{
				return item.Value;
			}
		}
		return false;
	}
}
