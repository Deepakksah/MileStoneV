using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:diagnostics")]
public class TestWindowsEventLoggerMsgOut
{
	[DataMember]
	public OperationStatus CurrentStatus;

	[DataMember]
	public bool Success;

	[DataMember]
	public string[] ResultDetails;
}
