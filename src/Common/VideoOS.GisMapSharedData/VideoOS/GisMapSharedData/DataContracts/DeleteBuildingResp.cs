using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "DeleteBuildingResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class DeleteBuildingResp : ParamsRespBase
{
	[DataProperty]
	public Guid Id { get; set; }
}
