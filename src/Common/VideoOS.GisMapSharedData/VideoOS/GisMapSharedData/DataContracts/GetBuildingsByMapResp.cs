using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Building;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetBuildingsByMapResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetBuildingsByMapResp : ParamsRespBase
{
	[DataProperty]
	public MapBuildingData[] Buildings { get; set; }
}
