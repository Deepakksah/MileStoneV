using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Mip;

public class UnknownMipItemMenuActionsData : MipItemMenuActionsData, IUnknownClassData
{
	public string UnknownTypeName { get; set; }
}
