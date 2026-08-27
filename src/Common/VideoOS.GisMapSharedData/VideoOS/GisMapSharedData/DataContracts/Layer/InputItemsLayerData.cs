using System;
using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts.Layer;

[ClassDataProperty("InputItemsLayerData")]
public class InputItemsLayerData : MapLayerData
{
	[DataProperty]
	[JsonProperty("repositoryId")]
	public Guid RepositoryId { get; set; }

	[DataProperty]
	[JsonProperty("isInputBackgroundDiskVisible")]
	public bool IsInputBackgroundDiskVisible { get; set; }

	[DataProperty]
	[JsonProperty("isInputTitleVisible")]
	public bool IsInputTitleVisible { get; set; }
}
