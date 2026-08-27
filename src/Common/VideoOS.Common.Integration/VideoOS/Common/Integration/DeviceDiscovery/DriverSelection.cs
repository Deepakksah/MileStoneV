using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class DriverSelection
{
	[XmlElement("Number")]
	public int Number { get; set; }

	[XmlElement("Name")]
	public string Name { get; set; }

	[XmlElement("GroupName")]
	public string GroupName { get; set; }

	[XmlElement("Selected")]
	public bool Selected { get; set; }

	[XmlIgnore]
	public bool ResolvedSelected
	{
		get
		{
			if (Visible)
			{
				return Selected;
			}
			return false;
		}
	}

	[XmlIgnore]
	public bool Visible { get; set; }

	[XmlIgnore]
	public int UseCount { get; set; }

	[XmlIgnore]
	public string ResolvedGroupName { get; set; }

	[XmlIgnore]
	public bool NewDriver { get; set; }

	public DriverSelection()
	{
		Number = -1;
		Selected = false;
		Name = string.Empty;
		GroupName = string.Empty;
		Visible = false;
		UseCount = 0;
		NewDriver = false;
		ResolvedGroupName = string.Empty;
	}

	public DriverSelection(int number, string name, string groupName, int useCount, bool selected, bool visible, bool newDriver, string resolvedGroupName)
	{
		Number = number;
		Name = name;
		GroupName = groupName;
		Selected = selected;
		Visible = visible;
		UseCount = useCount;
		NewDriver = newDriver;
		ResolvedGroupName = resolvedGroupName;
	}
}
