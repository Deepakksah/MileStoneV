using System;
using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;

[DataContract(Namespace = "urn:videoos:contracts:servicechannel:diagnostics")]
public class DiagBatchJobStatus
{
	[DataMember]
	public string BatchJobID;

	[DataMember]
	public int Status;

	[DataMember]
	public int NoofFailedStarts;

	[DataMember]
	public DateTime LastSuccess;

	[DataMember]
	public string LastJobMessage;
}
