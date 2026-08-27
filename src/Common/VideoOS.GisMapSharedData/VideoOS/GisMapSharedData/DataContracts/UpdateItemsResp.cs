using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "UpdateItemsResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class UpdateItemsResp : UnsupportedRespBase
{
	[DataProperty]
	public Guid[] Ids { get; set; }
}
