using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "DeleteLayersResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class DeleteLayersResp : ParamsRespBase
{
	[DataProperty]
	public Guid[] Ids { get; set; }
}
