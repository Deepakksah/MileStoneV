using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Building;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "ReorderLevelsParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class ReorderLevelsParams : ParamsRespBase
{
	[DataProperty]
	public MapLevelData[] Keys { get; set; }
}
