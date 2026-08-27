using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "AccessControlPropertyValidationResult", Namespace = "urn:milestone-systems")]
public class AccessControlPropertyValidationResult : IExtensibleDataObject
{
	[DataMember(Order = 1)]
	public bool ValidatedOk { get; set; }

	[DataMember(Order = 2)]
	public string FirstErrorPropertyKey { get; set; }

	[DataMember(Order = 3)]
	public string ErrorText { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
