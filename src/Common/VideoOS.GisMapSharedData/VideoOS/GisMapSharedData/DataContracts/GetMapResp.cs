using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Map;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetMapResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetMapResp : ParamsRespBase
{
	[DataProperty]
	public GisMapData Map { get; set; }
}
