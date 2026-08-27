using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Map;

[ClassDataProperty("MapCapability")]
public class MapCapability : IUnknownDataProperties, IUnknownInstance
{
	[DataProperty]
	[JsonProperty("name")]
	public string Name { get; set; }

	[DataProperty]
	[JsonProperty("generation")]
	public int Generation { get; set; }

	[JsonIgnore]
	public DataProperty[] UnknownData { get; set; }

	[JsonIgnore]
	IUnknownClassData IUnknownInstance.UnknownInstance => null;
}
