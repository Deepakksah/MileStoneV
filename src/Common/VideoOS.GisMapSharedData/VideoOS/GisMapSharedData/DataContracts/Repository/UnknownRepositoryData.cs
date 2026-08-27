using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Repository;

public class UnknownRepositoryData : RepositoryData, IUnknownClassData
{
	public string UnknownTypeName { get; set; }
}
