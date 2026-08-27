using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "AccessControlCommandResult", Namespace = "urn:milestone-systems")]
public class AccessControlCommandResult : IExtensibleDataObject
{
	[DataMember(Order = 1)]
	public bool Executed { get; set; }

	[DataMember(Order = 2)]
	public string ErrorMessage { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }

	public AccessControlCommandResult()
	{
	}

	public AccessControlCommandResult(bool executed, string errorMessage)
	{
		Executed = executed;
		ErrorMessage = errorMessage;
	}
}
