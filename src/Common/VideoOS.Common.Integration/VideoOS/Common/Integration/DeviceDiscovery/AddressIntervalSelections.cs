using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

[XmlRoot("AddressIntervalSelections")]
public class AddressIntervalSelections
{
	[XmlElement("AddressIntervalSelection")]
	public AddressIntervalSelectionCollection Items { get; set; }

	public AddressIntervalSelections()
	{
		Items = new AddressIntervalSelectionCollection();
	}

	public static AddressIntervalSelections FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<AddressIntervalSelections>(xml);
	}

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public static AddressIntervalSelections LoadXml(string addressIntervalsFileName, string scanRangesFileName)
	{
		AddressIntervalSelections addressIntervalSelections = new AddressIntervalSelections();
		bool flag = false;
		if (File.Exists(addressIntervalsFileName))
		{
			try
			{
				addressIntervalSelections = XmlSerializationHelper.LoadXml<AddressIntervalSelections>(addressIntervalsFileName);
				flag = true;
			}
			catch (LoadXmlException)
			{
			}
		}
		if (!flag)
		{
			try
			{
				foreach (AddressInterval item in AddressIntervals.FromXml(scanRangesFileName).Items)
				{
					addressIntervalSelections.Items.Add(selected: true, item);
				}
			}
			catch (LoadXmlException)
			{
			}
		}
		return addressIntervalSelections;
	}

	public void SaveXml(string fileName)
	{
		XmlSerializationHelper.SaveXml(this, fileName);
	}

	public AddressIntervals ExtractSelectedAddressIntervals()
	{
		AddressIntervals addressIntervals = new AddressIntervals();
		foreach (AddressIntervalSelection item in Items.Where((AddressIntervalSelection sai) => sai.Selected))
		{
			addressIntervals.Items.Add(item.AddressInterval);
		}
		return addressIntervals;
	}
}
