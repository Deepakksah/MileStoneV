using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Unsupported;

public class UnknownUnsupportedData : UnsupportedData, IUnknownClassData
{
	public string UnknownTypeName { get; set; }
}
