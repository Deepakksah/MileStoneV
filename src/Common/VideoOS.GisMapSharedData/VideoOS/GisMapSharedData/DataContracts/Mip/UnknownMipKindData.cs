using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Mip;

public class UnknownMipKindData : MipKindData, IUnknownClassData
{
	public string UnknownTypeName { get; set; }
}
