using System;
using System.Threading.Tasks;
using VideoOS.GisMapSharedData.DataContracts;

namespace VideoOS.GisMapCommunication.Repository;

public interface IRepositoryClient : IClientBase, IDisposable
{
	Task<AddRepositoryResp> AddRepositoryAsync(AddRepositoryParams parameters);

	Task<UpdateRepositoryResp> UpdateRepositoryAsync(UpdateRepositoryParams parameters);

	Task<GetRepositoryResp> GetRepositoryAsync(GetRepositoryParams parameters);

	AddRepositoryResp AddRepository(AddRepositoryParams parameters);

	UpdateRepositoryResp UpdateRepository(UpdateRepositoryParams parameters);

	GetRepositoryResp GetRepository(GetRepositoryParams parameters);
}
