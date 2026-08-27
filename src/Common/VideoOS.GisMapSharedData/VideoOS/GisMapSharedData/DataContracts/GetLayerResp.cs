using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Layer;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "GetLayerResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class GetLayerResp : UnsupportedRespBase
{
	[DataProperty]
	public MapLayerData Layer { get; set; }
}
