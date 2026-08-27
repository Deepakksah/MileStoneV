using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "GetAccessControlSystemConfigurationResp", Namespace = "urn:milestone-systems")]
public class GetAccessControlSystemConfigurationResp : IExtensibleDataObject
{
	[DataMember]
	public AccessControlSystem AccessControlSystem { get; set; }

	[DataMember]
	public string SerializedConfiguration { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
