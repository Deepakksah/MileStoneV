using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Building;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetBuildingResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetBuildingResp : ParamsRespBase
{
	[DataProperty]
	public MapBuildingData Building { get; set; }
}
