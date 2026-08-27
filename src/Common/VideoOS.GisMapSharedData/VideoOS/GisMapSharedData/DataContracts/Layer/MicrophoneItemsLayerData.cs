using System;
using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts.Layer;

[ClassDataProperty("MicrophoneItemsLayerData")]
public class MicrophoneItemsLayerData : MapLayerData
{
	[DataProperty]
	[JsonProperty("repositoryId")]
	public Guid RepositoryId { get; set; }

	[DataProperty]
	[JsonProperty("isMicrophoneBackgroundDiskVisible")]
	public bool IsMicrophoneBackgroundDiskVisible { get; set; }

	[DataProperty]
	[JsonProperty("isMicrophoneTitleVisible")]
	public bool IsMicrophoneTitleVisible { get; set; }
}
