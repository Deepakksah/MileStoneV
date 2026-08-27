using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Building;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "AddBuildingsParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class AddBuildingsParams : ParamsRespBase
{
	[DataProperty]
	public MapBuildingData[] Keys { get; set; }
}
