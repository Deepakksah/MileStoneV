using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts.Layer;

[ClassDataProperty("ProjectionSupportedLayerData")]
public class ProjectionSupportedLayerData : MapLayerData
{
	[DataProperty]
	[JsonProperty("srid")]
	public int SRID { get; set; }

	[DataProperty]
	[JsonProperty("originalExtent")]
	public Rectangle OriginalExtent { get; set; }

	[DataProperty]
	[JsonProperty("transformInfo")]
	public TransformInfo TransformInfo { get; set; }
}
