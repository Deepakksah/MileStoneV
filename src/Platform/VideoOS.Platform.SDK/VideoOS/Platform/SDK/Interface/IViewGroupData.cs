using System.Xml;

namespace VideoOS.Platform.SDK.Interface;

public interface IViewGroupData
{
	int DataVersion { get; set; }

	int XmlVersion { get; set; }

	XmlNode ViewGroupDataXml { get; set; }
}
