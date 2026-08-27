using System;
using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts.Item;

[ClassDataProperty("LinkMapItemData")]
public class LinkMapItemData : PointLinkMapItemData
{
	[DataProperty]
	[JsonProperty("targetServerId")]
	public Guid TargetServerId { get; set; }

	[DataProperty]
	[JsonProperty("targetMapId")]
	public Guid TargetMapId { get; set; }

	[DataProperty]
	[JsonProperty("color")]
	public string Color { get; set; }

	[DataProperty]
	[JsonProperty("locationId")]
	public Guid LocationId { get; set; }

	[DataProperty]
	[JsonProperty("displayStyleId")]
	public int DisplayStyleId { get; set; }
}
