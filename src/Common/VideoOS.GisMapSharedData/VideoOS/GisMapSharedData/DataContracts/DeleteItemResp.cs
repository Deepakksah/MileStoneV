using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "DeleteItemResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class DeleteItemResp : ParamsRespBase
{
	[DataProperty]
	public Guid Id { get; set; }
}
