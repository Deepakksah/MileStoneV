using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Layer;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetLayersByMapResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetLayersByMapResp : ParamsRespBase
{
	[DataProperty]
	public MapLayerData[] Layers { get; set; }
}
