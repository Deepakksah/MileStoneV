using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:diagnostics")]
public class DiagMessageWatch
{
	[DataMember]
	public string SessionID;

	[DataMember]
	public string MessageTypeName;

	[DataMember]
	public DiagMatchExpression[] Expressions;
}
