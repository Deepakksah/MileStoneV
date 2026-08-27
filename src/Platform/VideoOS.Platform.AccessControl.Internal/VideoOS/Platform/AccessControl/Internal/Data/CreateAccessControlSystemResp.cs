using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "CreateAccessControlSystemResp", Namespace = "urn:milestone-systems")]
public class CreateAccessControlSystemResp : IExtensibleDataObject
{
	[DataMember]
	public AccessControlSystem AccessControlSystem { get; set; }

	[DataMember]
	public string SerializedConfiguration { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
