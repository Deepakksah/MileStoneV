using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Building;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "UpdateLevelParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class UpdateLevelParams : ParamsRespBase
{
	[DataProperty]
	public MapLevelData Key { get; set; }
}
