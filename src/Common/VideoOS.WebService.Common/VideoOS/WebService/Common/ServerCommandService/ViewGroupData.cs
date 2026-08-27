using System.Runtime.Serialization;
using System.Xml;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ViewGroupData
{
	[DataMember]
	public int DataVersion { get; set; }

	[DataMember]
	public int XmlVersion { get; set; }

	[DataMember]
	public XmlNode ViewGroupDataXml { get; set; }
}
