using System;
using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts.Layer;

[ClassDataProperty("CADLayerData")]
public class CADLayerData : ProjectionSupportedLayerData
{
	[DataProperty]
	[JsonProperty("repositoryId")]
	public Guid RepositoryId { get; set; }
}
