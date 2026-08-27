using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Item;

public class UnknownMapItemData : MapItemData, IUnknownClassData
{
	public string UnknownTypeName { get; set; }
}
