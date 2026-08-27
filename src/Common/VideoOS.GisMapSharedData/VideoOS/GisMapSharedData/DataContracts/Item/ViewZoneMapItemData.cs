using System;
using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts.Item;

[ClassDataProperty("ViewZoneMapItemData")]
public class ViewZoneMapItemData : MapItemData
{
	[DataProperty]
	[JsonProperty("viewDirection")]
	public double ViewDirection { get; set; }

	[DataProperty]
	[JsonProperty("viewDepth")]
	public double ViewDepth { get; set; }

	[DataProperty]
	[JsonProperty("fieldOfView")]
	public double FieldOfView { get; set; }

	[DataProperty]
	[JsonProperty("dependsOnId")]
	public Guid DependsOnId { get; set; }
}
