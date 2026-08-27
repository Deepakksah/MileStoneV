using System;
using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts.Item;

[ClassDataProperty("PointMapItemData")]
public class PointMapItemData : MapItemData
{
	[DataProperty]
	[JsonProperty("position")]
	public Position Position { get; set; }

	[DataProperty]
	[JsonProperty("iconId")]
	public Guid IconId { get; set; }
}
