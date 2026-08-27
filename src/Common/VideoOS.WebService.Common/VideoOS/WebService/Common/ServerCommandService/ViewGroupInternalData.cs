using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Proxy.Server")]
public class ViewGroupInternalData
{
	[DataMember]
	public int DataVersion { get; set; }

	[DataMember]
	public int XmlVersion { get; set; }

	[DataMember]
	public string ViewGroupDataXml { get; set; }
}
