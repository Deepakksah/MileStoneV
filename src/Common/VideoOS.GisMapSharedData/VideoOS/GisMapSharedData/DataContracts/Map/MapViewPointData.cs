using System;
using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Map;

[ClassDataProperty("MapViewPointData")]
public class MapViewPointData : IUnknownDataProperties, IUnknownInstance
{
	[DataProperty]
	[JsonProperty("id")]
	public Guid Id { get; set; }

	[DataProperty]
	[JsonProperty("mapId")]
	public Guid MapId { get; set; }

	[DataProperty]
	[JsonProperty("name")]
	public string Name { get; set; }

	[DataProperty]
	[JsonProperty("note")]
	public string Note { get; set; }

	[DataProperty]
	[JsonProperty("viewPort")]
	public Rectangle Viewport { get; set; }

	[DataProperty]
	[JsonProperty("zoom")]
	public double Zoom { get; set; }

	[DataProperty]
	[JsonProperty("isCameraBackgroundDiskVisible")]
	public bool IsCameraBackgroundDiskVisible { get; set; }

	[DataProperty]
	[JsonProperty("isCameraTitleVisible")]
	public bool IsCameraTitleVisible { get; set; }

	[DataProperty]
	[JsonProperty("hiddenLayersIds")]
	public Guid[] HiddenLayersIds { get; set; }

	[JsonIgnore]
	public DataProperty[] UnknownData { get; set; }

	[JsonIgnore]
	IUnknownClassData IUnknownInstance.UnknownInstance => null;
}
