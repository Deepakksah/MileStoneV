using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Layer;

[ClassDataProperty("MapLayerBrowseData")]
[KnownType(typeof(MapLayerData))]
[KnownType(typeof(BingMapLayerData))]
[KnownType(typeof(BuildingLayerData))]
[KnownType(typeof(CADLayerData))]
[KnownType(typeof(CameraItemsLayerData))]
[KnownType(typeof(GoogleMapLayerData))]
[KnownType(typeof(ImageLayerData))]
[KnownType(typeof(InputItemsLayerData))]
[KnownType(typeof(LegacyLinkItemsLayerData))]
[KnownType(typeof(LinkItemsLayerData))]
[KnownType(typeof(LocationItemsLayerData))]
[KnownType(typeof(MicrophoneItemsLayerData))]
[KnownType(typeof(MipLayerData))]
[KnownType(typeof(OpenStreetMapLayerData))]
[KnownType(typeof(OutputItemsLayerData))]
[KnownType(typeof(ProjectionSupportedLayerData))]
[KnownType(typeof(ShapeFileLayerData))]
[KnownType(typeof(ViewZoneItemsLayerData))]
public class MapLayerBrowseData : IUnknownDataProperties, IUnknownInstance
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
	[JsonProperty("order")]
	public int Order { get; set; }

	[DataProperty]
	[JsonProperty("modifiers")]
	public int Modifiers { get; set; }

	[JsonIgnore]
	public DataProperty[] UnknownData { get; set; }

	[JsonIgnore]
	IUnknownClassData IUnknownInstance.UnknownInstance => new UnknownMapLayerBrowseData();
}
