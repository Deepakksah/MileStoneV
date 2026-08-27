using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Layer;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "AddMapResp", Namespace = "http://videoos.net/2/GisMapCommand")]
public class AddMapResp : UnsupportedRespBase
{
	[DataProperty]
	public MapLayerBrowseData[] AddedLayers { get; set; }
}
