using System;
using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetLevelsByBuildingParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetLevelsByBuildingParams : ParamsRespBase
{
	[DataProperty]
	public Guid Key { get; set; }
}
