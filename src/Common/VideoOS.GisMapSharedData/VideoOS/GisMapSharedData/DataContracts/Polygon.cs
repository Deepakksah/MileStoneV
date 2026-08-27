using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts;

[ClassDataProperty("Polygon")]
public class Polygon : IUnknownDataProperties, IUnknownInstance
{
	[DataProperty]
	[JsonProperty("outerRing")]
	public Position2D[] OuterRing { get; set; }

	[JsonIgnore]
	public DataProperty[] UnknownData { get; set; }

	[JsonIgnore]
	IUnknownClassData IUnknownInstance.UnknownInstance => null;
}
