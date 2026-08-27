using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Building;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "UpdateLevelsParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class UpdateLevelsParams : ParamsRespBase
{
	[DataProperty]
	public MapLevelData[] Keys { get; set; }
}
