using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts.Layer;

[ClassDataProperty("MipLayerData")]
public class MipLayerData : MapLayerData
{
	[DataProperty]
	[JsonProperty("anyItemsOnMap")]
	public bool AnyItemsOnMap { get; set; }
}
