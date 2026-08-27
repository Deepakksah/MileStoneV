using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Item;

[ClassDataProperty("MapItemData")]
[KnownType(typeof(AccessControlUnitMapItemData))]
[KnownType(typeof(CameraMapItemData))]
[KnownType(typeof(DeviceMapItemData))]
[KnownType(typeof(InputMapItemData))]
[KnownType(typeof(LegacyLinkMapItemData))]
[KnownType(typeof(LinkMapItemData))]
[KnownType(typeof(LocationMapItemData))]
[KnownType(typeof(MicrophoneMapItemData))]
[KnownType(typeof(OutputMapItemData))]
[KnownType(typeof(ViewZoneMapItemData))]
public class MapItemData : IUnknownDataProperties, IUnknownInstance
{
	[DataProperty]
	[JsonProperty("id")]
	public Guid Id { get; set; }

	[DataProperty]
	[JsonProperty("layerId")]
	public Guid LayerId { get; set; }

	[DataProperty]
	[JsonProperty("name")]
	public string Name { get; set; }

	[DataProperty]
	[JsonProperty("note")]
	public string Note { get; set; }

	[JsonIgnore]
	public DataProperty[] UnknownData { get; set; }

	[JsonIgnore]
	IUnknownClassData IUnknownInstance.UnknownInstance => new UnknownMapItemData();
}
