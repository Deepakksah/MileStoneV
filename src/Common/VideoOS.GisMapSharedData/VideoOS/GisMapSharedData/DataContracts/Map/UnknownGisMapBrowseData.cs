using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Map;

public class UnknownGisMapBrowseData : GisMapBrowseData, IUnknownClassData
{
	public string UnknownTypeName { get; set; }
}
