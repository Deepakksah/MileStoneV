using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "UpdateBuildingResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class UpdateBuildingResp : UnsupportedRespBase
{
	[DataProperty]
	public Guid Id { get; set; }
}
