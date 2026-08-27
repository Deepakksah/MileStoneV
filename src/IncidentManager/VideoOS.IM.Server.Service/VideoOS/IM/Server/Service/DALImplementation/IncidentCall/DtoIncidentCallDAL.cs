using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.IncidentCall;
using VideoOS.IM.Common.Dto.DtoRequestObjectModels.Paging;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.IncidentCall;
using VideoOS.IM.Common.Service.Interfaces.Common;
using VideoOS.IM.Common.Service.Interfaces.IncidentCall;
using VideoOS.IM.Server.DataMapper.CustomMapperFramework;
using VideoOS.IM.Server.Repository.Interfaces;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentCall;
using VideoOS.IM.Server.RepositoryModel.RepoRequestObjectModels.Paging;
using VideoOS.IM.Server.RepositoryModel.ResponseObjectModels.Common;
using VideoOS.IM.Server.Service.Annotations;
using VideoOS.IM.Server.Service.DALImplementation.Common;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Service.DALImplementation.IncidentCall;

public sealed class DtoIncidentCallDAL : DtoObjectBaseDAL, IDtoIncidentCallDAL, IDtoObjectBaseDAL<long, DtoIncidentCall, DtoIncidentCallResponse, DtoIncidentCallListResponse>
{
	public DtoIncidentCallDAL(IRepositoryDAL inDbService)
		: base(inDbService)
	{
	}

	public DtoIncidentCallResponse Create(DtoIncidentCall inObject)
	{
		return CreateAsync(inObject).GetAwaiter().GetResult();
	}

	public async Task<DtoIncidentCallResponse> CreateAsync(DtoIncidentCall inObject, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoIncidentCallResponse();
		RepoIncidentCall dbObject = MyMapper.Convert(inObject);
		RepositoryResponseBase<RepoIncidentCall> repositoryResponseBase = await _dbService.GetIncidentCallDAL().CreateAsync(dbObject, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		DtoIncidentCallResponse toRet = MyMapper.Convert(repositoryResponseBase);
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

	public DtoIncidentCallResponse Delete(long inObjectId)
	{
		return DeleteAsync(inObjectId).GetAwaiter().GetResult();
	}

	public async Task<DtoIncidentCallResponse> DeleteAsync(long inObjectId, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoIncidentCallResponse();
		if (await _dbService.GetIncidentCallDAL().ObjectExistAsync(inObjectId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
		{
			RepositoryResponseBase<RepoIncidentCall> repositoryResponseBase = await _dbService.GetIncidentCallDAL().DeleteAsync(inObjectId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			DtoIncidentCallResponse toRet = MyMapper.Convert(repositoryResponseBase);
			if (repositoryResponseBase.Success)
			{
				RepositoryResponseBase<int> repositoryResponseBase2 = await _dbService.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (!repositoryResponseBase2.Success)
				{
					toRet.Success = false;
					toRet.Message = repositoryResponseBase2.Message;
					toRet.MessageDescription = repositoryResponseBase2.MessageDescription;
				}
			}
			return toRet;
		}
		throw new ArgumentException($"The provided id {inObjectId} does not exist in database", "inObjectId");
	}

	public async Task<DtoIncidentCallListResponse> DeleteRangeAsync([NotNull] HashSet<long> inObjectIds, CancellationToken cancellationToken = default(CancellationToken))
	{
		inObjectIds.NullCheck("", "DeleteRangeAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\DALImplementation\\IncidentCall\\DtoIncidentCallDAL.cs", 94);
		if (!inObjectIds.Any())
		{
			throw new ArgumentException("Ids should have been provided.", "inObjectIds");
		}
		RepositoryResponseBase<IEnumerable<RepoIncidentCall>> repositoryResponseBase = await _dbService.GetIncidentCallDAL().DeleteRangeAsync(inObjectIds, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		DtoIncidentCallListResponse toRet = MyMapper.Convert(repositoryResponseBase);
		if (repositoryResponseBase.Success)
		{
			RepositoryResponseBase<int> repositoryResponseBase2 = await _dbService.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (!repositoryResponseBase2.Success)
			{
				toRet.Success = false;
				toRet.Message = repositoryResponseBase2.Message;
				toRet.MessageDescription = repositoryResponseBase2.MessageDescription;
			}
		}
		return toRet;
	}

	public DtoIncidentCallResponse Get(long inId)
	{
		return GetAsync(inId).GetAwaiter().GetResult();
	}

	public async Task<DtoIncidentCallResponse> GetAsync(long inId, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoIncidentCallResponse();
		return MyMapper.Convert(await _dbService.GetIncidentCallDAL().GetAsync(inId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
	}

	public DtoIncidentCallListResponse GetList(DtoPaging inPaging = null)
	{
		return GetListAsync(inPaging).GetAwaiter().GetResult();
	}

	public async Task<DtoIncidentCallListResponse> GetListAsync(DtoPaging inPaging = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoIncidentCallListResponse();
		RepositoryPaging inRepoPaging = MyMapper.Convert(inPaging);
		return MyMapper.Convert(await _dbService.GetIncidentCallDAL().GetListAsync(inRepoPaging, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
	}

	public DtoIncidentCallResponse Update(DtoIncidentCall inObject)
	{
		return UpdateAsync(inObject).GetAwaiter().GetResult();
	}

	public async Task<DtoIncidentCallResponse> UpdateAsync(DtoIncidentCall inObject, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoIncidentCallResponse();
		if (await _dbService.GetIncidentCallDAL().ObjectExistAsync(inObject?.Id ?? 0, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
		{
			RepoIncidentCall inObject2 = MyMapper.Convert(inObject);
			RepositoryResponseBase<RepoIncidentCall> repositoryResponseBase = await _dbService.GetIncidentCallDAL().UpdateAsync(inObject2, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			DtoIncidentCallResponse toRet = MyMapper.Convert(repositoryResponseBase);
			if (repositoryResponseBase.Success)
			{
				RepositoryResponseBase<int> repositoryResponseBase2 = await _dbService.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (!repositoryResponseBase2.Success)
				{
					toRet.Success = false;
					toRet.Message = repositoryResponseBase2.Message;
					toRet.MessageDescription = repositoryResponseBase2.MessageDescription;
				}
			}
			return toRet;
		}
		throw new ArgumentException($"The provided id {inObject?.Id ?? 0} does not exist in database", "inObject");
	}
}
