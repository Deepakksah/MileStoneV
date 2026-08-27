using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GisMapServiceFault", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GisMapServiceFault : IExtensibleDataObject
{
	[DataMember]
	public string ErrorDescription { get; set; }

	[DataMember]
	public string ErrorReason { get; set; }

	[DataMember]
	public GisMapServiceFaultData[] Data { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
