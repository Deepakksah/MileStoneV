using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Building;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "AddBuildingParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class AddBuildingParams : ParamsRespBase
{
	[DataProperty]
	public MapBuildingData Key { get; set; }
}
