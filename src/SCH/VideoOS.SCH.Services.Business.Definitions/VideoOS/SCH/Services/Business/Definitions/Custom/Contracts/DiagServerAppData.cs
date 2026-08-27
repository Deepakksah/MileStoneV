using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:diagnostics")]
public class DiagServerAppData
{
	[DataMember]
	public int LastMessageID;

	[DataMember]
	public DiagMessage[] Messages;

	[DataMember]
	public DiagSession[] Sessions;

	[DataMember]
	public DiagMessageWatch[] MessageWatchItems;

	[DataMember]
	public DiagMatchOperator[] MatchOperators;

	[DataMember]
	public DiagBatchJobStatus[] BatchJobStatusArray;
}
