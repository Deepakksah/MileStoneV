using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.GlobalSettings;
using VideoOS.IM.Common.Dto.DtoRequestObjectModels.Paging;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.GlobalSettings;
using VideoOS.IM.Common.Service.Interfaces.Common;
using VideoOS.IM.Common.Service.Interfaces.GlobalSettings;
using VideoOS.IM.Server.DataMapper.CustomMapperFramework;
using VideoOS.IM.Server.Repository.Interfaces;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.GlobalSettings;
using VideoOS.IM.Server.RepositoryModel.RepoRequestObjectModels.Paging;
using VideoOS.IM.Server.RepositoryModel.ResponseObjectModels.Common;
using VideoOS.IM.Server.Service.Annotations;
using VideoOS.IM.Server.Service.DALImplementation.Common;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Service.DALImplementation.GlobalSettings;

public sealed class DtoGlobalSettingsDAL : DtoObjectBaseDAL, IDtoGlobalSettingsDAL, IDtoObjectBaseDAL<long, DtoGlobalSettings, DtoGlobalSettingsResponse, DtoGlobalSettingsListResponse>
{
	public DtoGlobalSettingsDAL(IRepositoryDAL inDbService)
		: base(inDbService)
	{
	}

	public DtoGlobalSettingsResponse Create(DtoGlobalSettings inObject)
	{
		return CreateAsync(inObject).GetAwaiter().GetResult();
	}

	public async Task<DtoGlobalSettingsResponse> CreateAsync(DtoGlobalSettings inObject, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoGlobalSettingsResponse();
		RepoGlobalSettings dbObject = MyMapper.Convert(inObject);
		RepositoryResponseBase<RepoGlobalSettings> repositoryResponseBase = await _dbService.GetGlobalSettingsDAL().CreateAsync(dbObject, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		DtoGlobalSettingsResponse toRet = MyMapper.Convert(repositoryResponseBase);
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

	public DtoGlobalSettingsResponse Delete(long inObjectId)
	{
		return DeleteAsync(inObjectId).GetAwaiter().GetResult();
	}

	public async Task<DtoGlobalSettingsResponse> DeleteAsync(long inObjectId, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoGlobalSettingsResponse();
		if (await _dbService.GetGlobalSettingsDAL().ObjectExistAsync(inObjectId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
		{
			RepositoryResponseBase<RepoGlobalSettings> repositoryResponseBase = await _dbService.GetGlobalSettingsDAL().DeleteAsync(inObjectId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			DtoGlobalSettingsResponse toRet = MyMapper.Convert(repositoryResponseBase);
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

	public async Task<DtoGlobalSettingsListResponse> DeleteRangeAsync([NotNull] HashSet<long> inObjectIds, CancellationToken cancellationToken = default(CancellationToken))
	{
		inObjectIds.NullCheck("", "DeleteRangeAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\DALImplementation\\GlobalSettings\\DtoGlobalSettingsDAL.cs", 94);
		if (!inObjectIds.Any())
		{
			throw new ArgumentException("Ids should have been provided.", "inObjectIds");
		}
		RepositoryResponseBase<IEnumerable<RepoGlobalSettings>> repositoryResponseBase = await _dbService.GetGlobalSettingsDAL().DeleteRangeAsync(inObjectIds, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		DtoGlobalSettingsListResponse toRet = MyMapper.Convert(repositoryResponseBase);
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

	public DtoGlobalSettingsResponse Get(long inId)
	{
		return GetAsync(inId).GetAwaiter().GetResult();
	}

	public async Task<DtoGlobalSettingsResponse> GetAsync(long inId, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoGlobalSettingsResponse();
		return MyMapper.Convert(await _dbService.GetGlobalSettingsDAL().GetAsync(inId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
	}

	public DtoGlobalSettingsListResponse GetList(DtoPaging inPaging = null)
	{
		return GetListAsync(inPaging).GetAwaiter().GetResult();
	}

	public async Task<DtoGlobalSettingsListResponse> GetListAsync(DtoPaging inPaging = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoGlobalSettingsListResponse();
		RepositoryPaging inRepoPaging = MyMapper.Convert(inPaging);
		return MyMapper.Convert(await _dbService.GetGlobalSettingsDAL().GetListAsync(inRepoPaging, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
	}

	public DtoGlobalSettingsResponse Update(DtoGlobalSettings inObject)
	{
		return UpdateAsync(inObject).GetAwaiter().GetResult();
	}

	public async Task<DtoGlobalSettingsResponse> UpdateAsync(DtoGlobalSettings inObject, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoGlobalSettingsResponse();
		if (await _dbService.GetGlobalSettingsDAL().ObjectExistAsync(inObject?.Id ?? 0, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
		{
			RepoGlobalSettings inObject2 = MyMapper.Convert(inObject);
			RepositoryResponseBase<RepoGlobalSettings> repositoryResponseBase = await _dbService.GetGlobalSettingsDAL().UpdateAsync(inObject2, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			DtoGlobalSettingsResponse toRet = MyMapper.Convert(repositoryResponseBase);
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
