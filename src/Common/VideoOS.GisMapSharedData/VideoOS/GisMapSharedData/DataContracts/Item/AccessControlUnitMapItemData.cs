using System;
using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts.Item;

[ClassDataProperty("AccessControlUnitMapItemData")]
public class AccessControlUnitMapItemData : PointMapItemData
{
	[DataProperty]
	[JsonProperty("acUnitId")]
	public Guid ACUnitId { get; set; }

	[DataProperty]
	[JsonProperty("acSystemId")]
	public Guid ACSystemId { get; set; }
}
