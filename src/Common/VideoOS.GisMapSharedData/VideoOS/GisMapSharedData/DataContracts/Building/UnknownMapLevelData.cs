using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Building;

public class UnknownMapLevelData : MapLevelData, IUnknownClassData
{
	public string UnknownTypeName { get; set; }
}
