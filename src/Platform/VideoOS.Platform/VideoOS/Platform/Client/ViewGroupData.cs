using System.Xml;

namespace VideoOS.Platform.Client;

public class ViewGroupData
{
	public int DataVersion { get; set; }

	public XmlNode ViewGroupDataXml { get; set; }

	public int XmlVersion { get; set; }
}
