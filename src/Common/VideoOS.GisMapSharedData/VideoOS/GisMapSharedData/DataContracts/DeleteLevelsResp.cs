using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "DeleteLevelsResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class DeleteLevelsResp : ParamsRespBase
{
	[DataProperty]
	public Guid[] Ids { get; set; }
}
