using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.Incident;
using VideoOS.IM.Common.Dto.DtoRequestObjectModels.Paging;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.Incident;
using VideoOS.IM.Common.Service.Interfaces.Common;
using VideoOS.IM.Common.Service.Interfaces.Incident;
using VideoOS.IM.Server.DataMapper.CustomMapperFramework;
using VideoOS.IM.Server.Repository.Interfaces;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.Incident;
using VideoOS.IM.Server.RepositoryModel.RepoRequestObjectModels.Paging;
using VideoOS.IM.Server.RepositoryModel.ResponseObjectModels.Common;
using VideoOS.IM.Server.Service.Annotations;
using VideoOS.IM.Server.Service.DALImplementation.Common;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Service.DALImplementation.Incident;

public sealed class DtoIncidentDAL : DtoObjectBaseDAL, IDtoIncidentDAL, IDtoObjectBaseDAL<long, DtoIncident, DtoIncidentResponse, DtoIncidentListResponse>
{
	public DtoIncidentDAL(IRepositoryDAL inDbService)
		: base(inDbService)
	{
	}

	public DtoIncidentResponse Create(DtoIncident inObject)
	{
		return CreateAsync(inObject).GetAwaiter().GetResult();
	}

	public async Task<DtoIncidentResponse> CreateAsync(DtoIncident inObject, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoIncidentResponse();
		RepoIncident dbObject = MyMapper.Convert(inObject);
		RepositoryResponseBase<RepoIncident> repositoryResponseBase = await _dbService.GetIncidentDAL().CreateAsync(dbObject, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		DtoIncidentResponse toRet = MyMapper.Convert(repositoryResponseBase);
		if (repositoryResponseBase.Success)
		{
			RepositoryResponseBase<int> repositoryResponseBase2 = await _dbService.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (!repositoryResponseBase2.Success)
			{
				toRet.Success = false;
				toRet.Message = repositoryResponseBase2.Message;
				toRet.MessageDescription = repositoryResponseBase2.MessageDescription;
				return toRet;
			}
			if (toRet?.Value != null)
			{
				await _dbService.UpdateObjectPropertiesFromDbAsync(dbObject, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				toRet.Value.Id = dbObject.Id;
			}
		}
		return toRet;
	}

	public DtoIncidentResponse Delete(long inObjectId)
	{
		return DeleteAsync(inObjectId).GetAwaiter().GetResult();
	}

	public async Task<DtoIncidentResponse> DeleteAsync(long inObjectId, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoIncidentResponse();
		if (await _dbService.GetIncidentDAL().ObjectExistAsync(inObjectId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
		{
			RepositoryResponseBase<RepoIncident> repositoryResponseBase = await _dbService.GetIncidentDAL().DeleteAsync(inObjectId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			DtoIncidentResponse toRet = MyMapper.Convert(repositoryResponseBase);
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

	public async Task<DtoIncidentListResponse> DeleteRangeAsync([NotNull] HashSet<long> inObjectIds, CancellationToken cancellationToken = default(CancellationToken))
	{
		inObjectIds.NullCheck("", "DeleteRangeAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\DALImplementation\\Incident\\DtoIncidentDAL.cs", 100);
		if (!inObjectIds.Any())
		{
			throw new ArgumentException("Ids should have been provided.", "inObjectIds");
		}
		RepositoryResponseBase<IEnumerable<RepoIncident>> repositoryResponseBase = await _dbService.GetIncidentDAL().DeleteRangeAsync(inObjectIds, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		DtoIncidentListResponse toRet = MyMapper.Convert(repositoryResponseBase);
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

	public DtoIncidentResponse Get(long inId)
	{
		return GetAsync(inId).GetAwaiter().GetResult();
	}

	public async Task<DtoIncidentResponse> GetAsync(long inId, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoIncidentResponse();
		return MyMapper.Convert(await _dbService.GetIncidentDAL().GetAsync(inId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
	}

	public DtoIncidentListResponse GetList(DtoPaging inPaging = null)
	{
		return GetListAsync(inPaging).GetAwaiter().GetResult();
	}

	public async Task<DtoIncidentListResponse> GetListAsync(DtoPaging inPaging = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoIncidentListResponse();
		RepositoryPaging inRepoPaging = MyMapper.Convert(inPaging);
		return MyMapper.Convert(await _dbService.GetIncidentDAL().GetListAsync(inRepoPaging, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
	}

	public DtoIncidentResponse Update(DtoIncident inObject)
	{
		return UpdateAsync(inObject).GetAwaiter().GetResult();
	}

	public async Task<DtoIncidentResponse> UpdateAsync(DtoIncident inObject, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoIncidentResponse();
		if (await _dbService.GetIncidentDAL().ObjectExistAsync(inObject?.Id ?? 0, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
		{
			RepoIncident inObject2 = MyMapper.Convert(inObject);
			RepositoryResponseBase<RepoIncident> repositoryResponseBase = await _dbService.GetIncidentDAL().UpdateAsync(inObject2, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			DtoIncidentResponse toRet = MyMapper.Convert(repositoryResponseBase);
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

	public string GenerateExportIncidentUrlLink(long inId)
	{
		throw new NotImplementedException();
	}
}
