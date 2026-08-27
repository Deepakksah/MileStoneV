using System;
using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Building;

[ClassDataProperty("MapBuildingData")]
public class MapBuildingData : IUnknownDataProperties, IUnknownInstance
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
	[JsonProperty("outline")]
	public Polygon Outline { get; set; }

	[DataProperty]
	[JsonProperty("defaultLevelId")]
	public Guid DefaultLevelId { get; set; }

	[DataProperty]
	[JsonProperty("order")]
	public int Order { get; set; }

	[JsonIgnore]
	public DataProperty[] UnknownData { get; set; }

	[JsonIgnore]
	IUnknownClassData IUnknownInstance.UnknownInstance => new UnknownMapBuildingData();
}
