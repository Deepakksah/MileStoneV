using System.IO;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

[XmlRoot("DeviceGroupSelections")]
public class DeviceGroupSelections
{
	[XmlElement("DeviceGroupSelection")]
	public DeviceGroupSelectionCollection Items { get; }

	public DeviceGroupSelections()
	{
		Items = new DeviceGroupSelectionCollection();
	}

	public static DeviceGroupSelections LoadXml(string fileName)
	{
		DeviceGroupSelections result = new DeviceGroupSelections();
		if (File.Exists(fileName))
		{
			try
			{
				result = XmlSerializationHelper.LoadXml<DeviceGroupSelections>(fileName);
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
}
