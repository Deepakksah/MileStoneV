using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Building;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "UpdateBuildingParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class UpdateBuildingParams : ParamsRespBase
{
	[DataProperty]
	public MapBuildingData Key { get; set; }
}
