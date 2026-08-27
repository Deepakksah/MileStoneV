using System.IO;
using System.Linq;
using System.Xml.Serialization;
using VideoOS.Utilities.Sorting;

namespace VideoOS.Common.Integration.DeviceDiscovery;

[XmlRoot("DriverSelections")]
public class DriverSelections
{
	[XmlElement("Driver")]
	public DriverSelectionCollection Items { get; set; }

	public DriverSelections()
	{
		Items = new DriverSelectionCollection();
	}

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public static DriverSelections FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<DriverSelections>(xml);
	}

	public static DriverSelections LoadXml(string fileName)
	{
		DriverSelections result = new DriverSelections();
		if (File.Exists(fileName))
		{
			try
			{
				result = XmlSerializationHelper.LoadXml<DriverSelections>(fileName);
			}
			catch (LoadXmlException)
			{
			}
		}
		return result;
	}

	public void SaveXml(string fileName)
	{
		XmlSerializationHelper.SaveXml(this, fileName);
	}

	public void Sort()
	{
		DriverSelection[] array = Items.OrderBy((DriverSelection drv) => drv.Name, new AlphanumericComparatorFast()).ToArray();
		Items.Clear();
		DriverSelection[] array2 = array;
		foreach (DriverSelection item in array2)
		{
			Items.Add(item);
		}
	}
}
