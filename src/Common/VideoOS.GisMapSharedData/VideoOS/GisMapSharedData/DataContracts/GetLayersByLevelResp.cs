using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Layer;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetLayersByLevelResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetLayersByLevelResp : ParamsRespBase
{
	[DataProperty]
	public MapLayerData[] Layers { get; set; }
}
