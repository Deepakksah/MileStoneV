using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "DeleteItemsResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class DeleteItemsResp : ParamsRespBase
{
	[DataProperty]
	public Guid[] Ids { get; set; }
}
