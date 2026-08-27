using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "DeleteBuildingsResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class DeleteBuildingsResp : ParamsRespBase
{
	[DataProperty]
	public Guid[] Ids { get; set; }
}
