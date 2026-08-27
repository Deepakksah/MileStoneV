using System;
using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Item;

namespace VideoOS.GisMapSharedData.DataContracts.Mip;

[ClassDataProperty("MipItemData")]
public class MipItemData : PointMapItemData
{
	[DataProperty]
	[JsonProperty("clusterCount")]
	public int ClusterCount { get; set; }

	[DataProperty]
	[JsonProperty("buildingId")]
	public Guid BuildingId { get; set; }
}
