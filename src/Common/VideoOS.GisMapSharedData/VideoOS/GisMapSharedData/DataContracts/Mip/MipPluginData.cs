using System;
using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Mip;

[ClassDataProperty("MipPluginData")]
public class MipPluginData : IUnknownDataProperties, IUnknownInstance
{
	[DataProperty]
	[JsonProperty("id")]
	public Guid Id { get; set; }

	[DataProperty]
	[JsonProperty("name")]
	public string Name { get; set; }

	[DataProperty]
	[JsonProperty("icon")]
	public byte[] Icon { get; set; }

	[DataProperty]
	[JsonProperty("kinds")]
	public MipKindData[] Kinds { get; set; }

	[JsonIgnore]
	public DataProperty[] UnknownData { get; set; }

	[JsonIgnore]
	IUnknownClassData IUnknownInstance.UnknownInstance => new UnknownMipPluginData();
}
