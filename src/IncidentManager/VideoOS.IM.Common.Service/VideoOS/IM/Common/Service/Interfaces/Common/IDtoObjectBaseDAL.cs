using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.IM.Common.Dto.DtoRequestObjectModels.Paging;
using VideoOS.IM.Common.Service.Properties;

namespace VideoOS.IM.Common.Service.Interfaces.Common;

public interface IDtoObjectBaseDAL<TIdType, in TDto, TDtoResponse, TDtoListResponse>
{
	TDtoResponse Get(TIdType inId);

	Task<TDtoResponse> GetAsync(TIdType inId, CancellationToken cancellationToken = default(CancellationToken));

	TDtoListResponse GetList(DtoPaging inPaging = null);

	Task<TDtoListResponse> GetListAsync(DtoPaging inPaging = null, CancellationToken cancellationToken = default(CancellationToken));

	TDtoResponse Update(TDto inObject);

	Task<TDtoResponse> UpdateAsync(TDto inObject, CancellationToken cancellationToken = default(CancellationToken));

	TDtoResponse Create(TDto inObject);

	Task<TDtoResponse> CreateAsync(TDto inObject, CancellationToken cancellationToken = default(CancellationToken));

	TDtoResponse Delete(TIdType inObjectId);

	Task<TDtoResponse> DeleteAsync(TIdType inObjectId, CancellationToken cancellationToken = default(CancellationToken));

	Task<TDtoListResponse> DeleteRangeAsync([NotNull] HashSet<TIdType> inObjectIds, CancellationToken cancellationToken = default(CancellationToken));
}
