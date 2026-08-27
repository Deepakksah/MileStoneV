using System;
using System.Collections;
using System.Globalization;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Config;

[Serializable]
public class PTZArgs
{
	[XmlElement(typeof(PTZPairDouble))]
	public ArrayList movement = new ArrayList();

	[XmlElement(typeof(PTZPairDouble))]
	public ArrayList speed = new ArrayList();

	[XmlElement(typeof(PTZPairBoolean))]
	public ArrayList automatic = new ArrayList();

	public bool Normalized = true;

	[XmlIgnore]
	private const string PrimaryXmlTag = "ptzargs";

	public bool isPanAvailable => ContainsKey(movement, "pan");

	private bool isTiltAvailable => ContainsKey(movement, "tilt");

	private bool isZoomAvailable => ContainsKey(movement, "zoom");

	private bool isPanSpeedAvailable => ContainsKey(speed, "pan");

	private bool isTiltSpeedAvailable => ContainsKey(speed, "tilt");

	private bool isZoomSpeedAvailable => ContainsKey(speed, "zoom");

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

	public PTZArgs(PTZArgs args)
	{
		movement = (ArrayList)args.movement.Clone();
		speed = (ArrayList)args.speed.Clone();
		automatic = (ArrayList)args.automatic.Clone();
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

	public void SetAuto(string name, bool value)
	{
		SetValue(automatic, name, value);
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

	private static bool ContainsKey(ArrayList list, string name)
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
}
