using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:diagnostics")]
public class GetAssemblyInfoMsgOut
{
	[DataMember]
	public OperationStatus CurrentStatus;

	[DataMember]
	public DiagAssemblyInfo[] AssemblyInfoArray;
}
