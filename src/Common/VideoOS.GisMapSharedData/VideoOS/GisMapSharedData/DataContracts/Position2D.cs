using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts;

[ClassDataProperty("Position2D")]
public class Position2D : IUnknownDataProperties, IUnknownInstance
{
	[DataProperty]
	[JsonProperty("x")]
	public double X { get; set; }

	[DataProperty]
	[JsonProperty("y")]
	public double Y { get; set; }

	[JsonIgnore]
	public DataProperty[] UnknownData { get; set; }

	[JsonIgnore]
	IUnknownClassData IUnknownInstance.UnknownInstance => null;
}
