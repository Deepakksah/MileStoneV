using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts;

[ClassDataProperty("TransformInfo")]
public class TransformInfo : IUnknownDataProperties, IUnknownInstance
{
	[DataProperty]
	[JsonProperty("upperLeft")]
	public Position UpperLeft { get; set; }

	[DataProperty]
	[JsonProperty("upperRight")]
	public Position UpperRight { get; set; }

	[DataProperty]
	[JsonProperty("lowerLeft")]
	public Position LowerLeft { get; set; }

	[DataProperty]
	[JsonProperty("lowerRight")]
	public Position LowerRight { get; set; }

	[JsonIgnore]
	public DataProperty[] UnknownData { get; set; }

	[JsonIgnore]
	IUnknownClassData IUnknownInstance.UnknownInstance => null;
}
