using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Interfaces;
using VideoOS.GisMapSharedData.DataContracts.Layer;

namespace VideoOS.GisMapSharedData.DataContracts.Map;

[ClassDataProperty("GisMapData")]
public class GisMapData : GisMapBrowseData, IUnknownInstance
{
	[DataProperty]
	public MapLayerData[] Layers { get; set; }

	[DataProperty]
	public Position Position { get; set; }

	[DataProperty]
	public MapSettingsData MapSettings { get; set; }

	[DataProperty]
	public MapViewPointData[] MapViewPoints { get; set; }

	[DataProperty]
	public MapCapability[] Capabilities { get; set; }

	IUnknownClassData IUnknownInstance.UnknownInstance => new UnknownGisMapData();
}
