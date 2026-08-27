using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "GetDowntimesResp", Namespace = "urn:milestone-systems")]
public class GetDowntimesResp : IExtensibleDataObject
{
	[DataMember]
	public AccessControlDowntimeData[] DowntimeData { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
