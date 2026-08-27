using Newtonsoft.Json;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Map;

[ClassDataProperty("MapSettingsData")]
public class MapSettingsData : IUnknownDataProperties, IUnknownInstance
{
	[DataProperty]
	[JsonProperty("viewport")]
	public Rectangle Viewport { get; set; }

	[DataProperty]
	[JsonProperty("zoomMax")]
	public double ZoomMax { get; set; }

	[DataProperty]
	[JsonProperty("zoomMin")]
	public double ZoomMin { get; set; }

	[JsonIgnore]
	public DataProperty[] UnknownData { get; set; }

	[JsonIgnore]
	IUnknownClassData IUnknownInstance.UnknownInstance => null;
}
