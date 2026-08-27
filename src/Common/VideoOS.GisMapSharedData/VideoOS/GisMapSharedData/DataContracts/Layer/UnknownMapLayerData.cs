using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Layer;

public class UnknownMapLayerData : MapLayerData, IUnknownClassData
{
	public string UnknownTypeName { get; set; }
}
