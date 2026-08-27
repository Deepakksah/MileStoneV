using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Map;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "UpdateMapParams", Namespace = "http://videoos.net/2/GisMapCommand")]
public class UpdateMapParams : ParamsRespBase
{
	[DataProperty]
	public GisMapData Key { get; set; }
}
