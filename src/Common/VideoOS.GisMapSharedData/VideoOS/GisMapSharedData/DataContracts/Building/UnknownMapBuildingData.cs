using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Building;

public class UnknownMapBuildingData : MapBuildingData, IUnknownClassData
{
	public string UnknownTypeName { get; set; }
}
