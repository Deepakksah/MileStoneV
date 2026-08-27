using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentActivityLog;
using VideoOS.IM.Common.Dto.DtoRequestObjectModels.Paging;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.IncidentActivityLog;
using VideoOS.IM.Common.Service.Interfaces.Common;
using VideoOS.IM.Common.Service.Interfaces.IncidentActivityLog;
using VideoOS.IM.Server.DataMapper.CustomMapperFramework;
using VideoOS.IM.Server.Repository.Interfaces;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentActivityLog;
using VideoOS.IM.Server.RepositoryModel.RepoRequestObjectModels.Paging;
using VideoOS.IM.Server.RepositoryModel.ResponseObjectModels.Common;
using VideoOS.IM.Server.Service.Annotations;
using VideoOS.IM.Server.Service.DALImplementation.Common;

namespace VideoOS.IM.Server.Service.DALImplementation.IncidentActivityLog;

public sealed class DtoIncidentActivityLogDAL : DtoObjectBaseDAL, IDtoIncidentActivityLogDAL, IDtoObjectBaseDAL<long, DtoIncidentActivityLog, DtoIncidentActivityLogResponse, DtoIncidentActivityLogListResponse>
{
	public DtoIncidentActivityLogDAL(IRepositoryDAL inDbService)
		: base(inDbService)
	{
	}

	public DtoIncidentActivityLogResponse Create(DtoIncidentActivityLog inObject)
	{
		return CreateAsync(inObject).GetAwaiter().GetResult();
	}

	public async Task<DtoIncidentActivityLogResponse> CreateAsync(DtoIncidentActivityLog inObject, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoIncidentActivityLogResponse();
		RepoIncidentActivityLog dbObject = MyMapper.Convert(inObject);
		RepositoryResponseBase<RepoIncidentActivityLog> repositoryResponseBase = await _dbService.GetIncidentActivityLogDAL().CreateAsync(dbObject, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		DtoIncidentActivityLogResponse toRet = MyMapper.Convert(repositoryResponseBase);
		if (repositoryResponseBase.Success)
		{
			RepositoryResponseBase<int> repositoryResponseBase2 = await _dbService.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (!repositoryResponseBase2.Success)
			{
				toRet.Success = false;
				toRet.Message = repositoryResponseBase2.Message;
				toRet.MessageDescription = repositoryResponseBase2.MessageDescription;
			}
			else if (toRet?.Value != null)
			{
				await _dbService.UpdateObjectPropertiesFromDbAsync(dbObject, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				toRet.Value.Id = dbObject.Id;
			}
		}
		return toRet;
	}

	public DtoIncidentActivityLogResponse Delete(long inObjectId)
	{
		throw new InvalidOperationException("Activity log cannot be deleted.");
	}

	public Task<DtoIncidentActivityLogResponse> DeleteAsync(long inObjectId, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new InvalidOperationException("Activity log cannot be deleted.");
	}

	public Task<DtoIncidentActivityLogListResponse> DeleteRangeAsync([NotNull] HashSet<long> inObjectIds, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new InvalidOperationException("Activity logs cannot be deleted.");
	}

	public DtoIncidentActivityLogResponse Get(long inId)
	{
		return GetAsync(inId).GetAwaiter().GetResult();
	}

	public async Task<DtoIncidentActivityLogResponse> GetAsync(long inId, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoIncidentActivityLogResponse();
		return MyMapper.Convert(await _dbService.GetIncidentActivityLogDAL().GetAsync(inId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
	}

	public DtoIncidentActivityLogListResponse GetList(DtoPaging inPaging = null)
	{
		return GetListAsync(inPaging).GetAwaiter().GetResult();
	}

	public async Task<DtoIncidentActivityLogListResponse> GetListAsync(DtoPaging inPaging = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoIncidentActivityLogListResponse();
		RepositoryPaging inRepoPaging = MyMapper.Convert(inPaging);
		return MyMapper.Convert(await _dbService.GetIncidentActivityLogDAL().GetListAsync(inRepoPaging, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
	}

	public DtoIncidentActivityLogResponse Update(DtoIncidentActivityLog inObject)
	{
		throw new InvalidOperationException("Activity log cannot be updated.");
	}

	public Task<DtoIncidentActivityLogResponse> UpdateAsync(DtoIncidentActivityLog inObject, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new InvalidOperationException("Activity log cannot be updated.");
	}
}
