using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts;

[ClassDataProperty("Rectangle")]
public class Rectangle : IUnknownDataProperties, IUnknownInstance
{
	[DataProperty]
	[JsonProperty("upperLeft")]
	public Position UpperLeft { get; set; }

	[DataProperty]
	[JsonProperty("lowerRight")]
	public Position LowerRight { get; set; }

	[JsonIgnore]
	public DataProperty[] UnknownData { get; set; }

	[JsonIgnore]
	IUnknownClassData IUnknownInstance.UnknownInstance => null;
}
