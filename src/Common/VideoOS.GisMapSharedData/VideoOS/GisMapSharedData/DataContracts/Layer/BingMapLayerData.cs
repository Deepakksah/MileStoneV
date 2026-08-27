using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts.Layer;

[ClassDataProperty("BingMapLayerData")]
public class BingMapLayerData : MapLayerData
{
	[DataProperty]
	[JsonProperty("mapType")]
	public int MapType { get; set; }
}
