using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "DeleteRepositoryResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class DeleteRepositoryResp : ParamsRespBase
{
	[DataProperty]
	public Guid Id { get; set; }
}
