using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Map;

public class UnknownGisMapData : GisMapBrowseData, IUnknownClassData
{
	public string UnknownTypeName { get; set; }
}
