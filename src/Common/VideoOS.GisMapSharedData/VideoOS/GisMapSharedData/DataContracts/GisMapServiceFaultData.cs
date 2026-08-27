using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GisMapServiceFaultData", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GisMapServiceFaultData : IExtensibleDataObject
{
	[DataMember]
	public string Key { get; set; }

	[DataMember]
	public string Value { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
