using System;
using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Building;

[ClassDataProperty("BuildingMapItemData")]
public class BuildingMapItemData : IUnknownDataProperties, IUnknownInstance
{
	[DataProperty]
	[JsonProperty("id")]
	public Guid Id { get; set; }

	[JsonIgnore]
	public DataProperty[] UnknownData { get; set; }

	[JsonIgnore]
	IUnknownClassData IUnknownInstance.UnknownInstance => null;
}
