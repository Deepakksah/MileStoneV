using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Mip;

public class UnknownMipItemData : MipItemData, IUnknownClassData
{
	public string UnknownTypeName { get; set; }
}
