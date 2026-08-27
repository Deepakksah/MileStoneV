using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Mip;

public class UnknownMipPluginData : MipPluginData, IUnknownClassData
{
	public string UnknownTypeName { get; set; }
}
