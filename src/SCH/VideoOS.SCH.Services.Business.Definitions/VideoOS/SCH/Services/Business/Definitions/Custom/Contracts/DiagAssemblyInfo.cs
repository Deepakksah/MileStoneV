using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:diagnostics")]
public class DiagAssemblyInfo
{
	[DataMember]
	public string Name;

	[DataMember]
	public string Version;

	[DataMember]
	public string CodeBase;

	[DataMember]
	public string Location;
}
