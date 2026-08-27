using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Layer;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "AddLayerResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class AddLayerResp : UnsupportedRespBase
{
	[DataProperty]
	public MapLayerBrowseData AddedLayer { get; set; }
}
