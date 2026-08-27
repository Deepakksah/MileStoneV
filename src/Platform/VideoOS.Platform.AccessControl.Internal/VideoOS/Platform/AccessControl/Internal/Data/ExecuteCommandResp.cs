using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "ExecuteCommandResp", Namespace = "urn:milestone-systems")]
public class ExecuteCommandResp : IExtensibleDataObject
{
	[DataMember]
	public AccessControlCommandResult AccessControlCommandResult { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
