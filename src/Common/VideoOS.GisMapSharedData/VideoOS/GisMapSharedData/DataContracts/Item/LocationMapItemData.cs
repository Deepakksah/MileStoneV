using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts.Item;

[ClassDataProperty("LocationMapItemData")]
public class LocationMapItemData : PointMapItemData
{
	[DataProperty]
	[JsonProperty("scale")]
	public double Scale { get; set; }

	[DataProperty]
	[JsonProperty("color")]
	public string Color { get; set; }
}
