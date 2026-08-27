using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Layer;

public class UnknownMapLayerBrowseData : MapLayerBrowseData, IUnknownClassData
{
	public string UnknownTypeName { get; set; }
}
