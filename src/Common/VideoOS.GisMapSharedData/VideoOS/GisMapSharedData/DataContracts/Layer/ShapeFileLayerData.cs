using System;
using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts.Layer;

[ClassDataProperty("ShapeFileLayerData")]
public class ShapeFileLayerData : ProjectionSupportedLayerData
{
	[DataProperty]
	[JsonProperty("repositoryId")]
	public Guid RepositoryId { get; set; }

	[DataProperty]
	[JsonProperty("fillColor")]
	public string FillColor { get; set; }

	[DataProperty]
	[JsonProperty("lineColor")]
	public string LineColor { get; set; }
}
