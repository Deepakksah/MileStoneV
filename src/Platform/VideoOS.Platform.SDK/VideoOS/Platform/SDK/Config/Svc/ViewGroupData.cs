using System.Xml;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config.Svc;

public class ViewGroupData
{
	public int XmlVersion { get; set; }

	public int DataVersion { get; set; }

	public XmlNode ViewGroupDataXml { get; set; }

	public ViewGroupData(IViewGroupData viewGroupData)
	{
		XmlVersion = viewGroupData.XmlVersion;
		DataVersion = viewGroupData.DataVersion;
		ViewGroupDataXml = viewGroupData.ViewGroupDataXml;
	}

	public ViewGroupData()
	{
	}
}
