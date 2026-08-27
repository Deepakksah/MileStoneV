using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Building;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetLevelsByBuildingResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetLevelsByBuildingResp : ParamsRespBase
{
	[DataProperty]
	public MapLevelData[] Levels { get; set; }
}
