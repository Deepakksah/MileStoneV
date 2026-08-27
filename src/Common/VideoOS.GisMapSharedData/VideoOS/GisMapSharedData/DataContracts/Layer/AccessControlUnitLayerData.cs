using System;
using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts.Layer;

[ClassDataProperty("AccessControlUnitLayerData")]
public class AccessControlUnitLayerData : MapLayerData
{
	[DataProperty]
	[JsonProperty("repositoryId")]
	public Guid RepositoryId { get; set; }

	[DataProperty]
	[JsonProperty("isACUnitBackgroundDiskVisible")]
	public bool IsACUnitBackgroundDiskVisible { get; set; }

	[DataProperty]
	[JsonProperty("isACUnitTitleVisible")]
	public bool IsACUnitTitleVisible { get; set; }
}
