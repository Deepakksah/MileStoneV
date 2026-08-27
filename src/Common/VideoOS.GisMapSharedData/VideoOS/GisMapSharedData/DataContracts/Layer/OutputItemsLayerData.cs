using System;
using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts.Layer;

[ClassDataProperty("OutputItemsLayerData")]
public class OutputItemsLayerData : MapLayerData
{
	[DataProperty]
	[JsonProperty("repositoryId")]
	public Guid RepositoryId { get; set; }

	[DataProperty]
	[JsonProperty("isOutputBackgroundDiskVisible")]
	public bool IsOutputBackgroundDiskVisible { get; set; }

	[DataProperty]
	[JsonProperty("isOutputTitleVisible")]
	public bool IsOutputTitleVisible { get; set; }
}
