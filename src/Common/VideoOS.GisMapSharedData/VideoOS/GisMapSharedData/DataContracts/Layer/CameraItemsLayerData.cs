using System;
using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts.Layer;

[ClassDataProperty("CameraItemsLayerData")]
public class CameraItemsLayerData : MapLayerData
{
	[DataProperty]
	[JsonProperty("repositoryId")]
	public Guid RepositoryId { get; set; }

	[DataProperty]
	[JsonProperty("isCameraBackgroundDiskVisible")]
	public bool IsCameraBackgroundDiskVisible { get; set; }

	[DataProperty]
	[JsonProperty("isCameraTitleVisible")]
	public bool IsCameraTitleVisible { get; set; }
}
