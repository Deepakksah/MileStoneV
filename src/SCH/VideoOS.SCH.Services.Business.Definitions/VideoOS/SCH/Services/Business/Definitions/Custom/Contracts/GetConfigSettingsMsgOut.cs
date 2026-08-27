using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:diagnostics")]
public class GetConfigSettingsMsgOut
{
	[DataMember]
	public OperationStatus CurrentStatus;

	[DataMember]
	public DiagSimpleKVPair[] ConfigSettings;
}
