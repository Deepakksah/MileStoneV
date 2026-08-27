using System;
using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Layer;

[ClassDataProperty("MapLayerData")]
public class MapLayerData : MapLayerBrowseData, IUnknownInstance
{
	[DataProperty]
	[JsonProperty("isVisible")]
	public bool IsVisible { get; set; }

	[DataProperty]
	[JsonProperty("extent")]
	public Rectangle Extent { get; set; }

	[DataProperty]
	[JsonProperty("levelId")]
	public Guid LevelId { get; set; }

	[JsonIgnore]
	IUnknownClassData IUnknownInstance.UnknownInstance => new UnknownMapLayerData();
}
