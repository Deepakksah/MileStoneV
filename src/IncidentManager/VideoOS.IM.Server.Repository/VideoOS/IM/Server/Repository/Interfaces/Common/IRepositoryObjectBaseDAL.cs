using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.IM.Server.RepositoryModel.RepoRequestObjectModels.Paging;
using VideoOS.IM.Server.RepositoryModel.ResponseObjectModels.Common;

namespace VideoOS.IM.Server.Repository.Interfaces.Common;

public interface IRepositoryObjectBaseDAL<TIdType, TRepo>
{
	Task<bool> ObjectExistAsync(TIdType inId, CancellationToken cancellationToken);

	Task<RepositoryResponseBase<TRepo>> GetAsync(TIdType inId, CancellationToken cancellationToken);

	Task<RepositoryResponseBase<IEnumerable<TRepo>>> GetListAsync(RepositoryPaging inRepoPaging, CancellationToken cancellationToken);

	Task<RepositoryResponseBase<TRepo>> UpdateAsync(TRepo inObject, CancellationToken cancellationToken);

	Task<RepositoryResponseBase<TRepo>> CreateAsync(TRepo inObject, CancellationToken cancellationToken);

	Task<RepositoryResponseBase<TRepo>> DeleteAsync(TRepo inObject, CancellationToken cancellationToken);

	Task<RepositoryResponseBase<TRepo>> DeleteAsync(TIdType inId, CancellationToken cancellationToken);

	Task<RepositoryResponseBase<IEnumerable<TRepo>>> DeleteRangeAsync(HashSet<TIdType> inIds, CancellationToken cancellationToken);
}
