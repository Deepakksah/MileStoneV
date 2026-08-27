using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts.Layer;

[ClassDataProperty("GoogleMapLayerData")]
public class GoogleMapLayerData : MapLayerData
{
	[DataProperty]
	[JsonProperty("mapType")]
	public int MapType { get; set; }
}
