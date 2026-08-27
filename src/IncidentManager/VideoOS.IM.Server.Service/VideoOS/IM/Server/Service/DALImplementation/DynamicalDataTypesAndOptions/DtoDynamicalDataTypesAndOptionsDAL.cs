using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.DynamicalDataTypesAndOptions;
using VideoOS.IM.Common.Dto.DtoRequestObjectModels.Paging;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.DynamicalDataTypesAndOptions;
using VideoOS.IM.Common.Service.Interfaces.Common;
using VideoOS.IM.Common.Service.Interfaces.DynamicalDataTypesAndOptions;
using VideoOS.IM.Server.DataMapper.CustomMapperFramework;
using VideoOS.IM.Server.Repository.Interfaces;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.DynamicalDataTypesAndOptions;
using VideoOS.IM.Server.RepositoryModel.RepoRequestObjectModels.Paging;
using VideoOS.IM.Server.RepositoryModel.ResponseObjectModels.Common;
using VideoOS.IM.Server.Service.Annotations;
using VideoOS.IM.Server.Service.DALImplementation.Common;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Service.DALImplementation.DynamicalDataTypesAndOptions;

public sealed class DtoDynamicalDataTypesAndOptionsDAL : DtoObjectBaseDAL, IDtoDynamicalDataTypesAndOptionsDAL, IDtoObjectBaseDAL<long, DtoDynamicalDataTypesAndOptions, DtoDynamicalDataTypesAndOptionsResponse, DtoDynamicalDataTypesAndOptionsListResponse>
{
	public DtoDynamicalDataTypesAndOptionsDAL(IRepositoryDAL inDbService)
		: base(inDbService)
	{
	}

	public DtoDynamicalDataTypesAndOptionsResponse Create(DtoDynamicalDataTypesAndOptions inObject)
	{
		return CreateAsync(inObject).GetAwaiter().GetResult();
	}

	public async Task<DtoDynamicalDataTypesAndOptionsResponse> CreateAsync(DtoDynamicalDataTypesAndOptions inObject, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoDynamicalDataTypesAndOptionsResponse();
		RepoDynamicalDataTypesAndOptions dbObject = MyMapper.Convert(inObject);
		RepositoryResponseBase<RepoDynamicalDataTypesAndOptions> repositoryResponseBase = await _dbService.GetDynamicalDataTypesAndOptionsDAL().CreateAsync(dbObject, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		DtoDynamicalDataTypesAndOptionsResponse toRet = MyMapper.Convert(repositoryResponseBase);
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

	public DtoDynamicalDataTypesAndOptionsResponse Delete(long inObjectId)
	{
		return DeleteAsync(inObjectId).GetAwaiter().GetResult();
	}

	public async Task<DtoDynamicalDataTypesAndOptionsResponse> DeleteAsync(long inObjectId, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoDynamicalDataTypesAndOptionsResponse();
		if (await _dbService.GetDynamicalDataTypesAndOptionsDAL().ObjectExistAsync(inObjectId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
		{
			RepositoryResponseBase<RepoDynamicalDataTypesAndOptions> repositoryResponseBase = await _dbService.GetDynamicalDataTypesAndOptionsDAL().DeleteAsync(inObjectId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			DtoDynamicalDataTypesAndOptionsResponse toRet = MyMapper.Convert(repositoryResponseBase);
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

	public async Task<DtoDynamicalDataTypesAndOptionsListResponse> DeleteRangeAsync([NotNull] HashSet<long> inObjectIds, CancellationToken cancellationToken = default(CancellationToken))
	{
		inObjectIds.NullCheck("", "DeleteRangeAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\DALImplementation\\DynamicalDataTypesAndOptions\\DtoDynamicalDataTypesAndOptionsDAL.cs", 94);
		if (!inObjectIds.Any())
		{
			throw new ArgumentException("Ids should have been provided.", "inObjectIds");
		}
		RepositoryResponseBase<IEnumerable<RepoDynamicalDataTypesAndOptions>> repositoryResponseBase = await _dbService.GetDynamicalDataTypesAndOptionsDAL().DeleteRangeAsync(inObjectIds, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		DtoDynamicalDataTypesAndOptionsListResponse toRet = MyMapper.Convert(repositoryResponseBase);
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

	public DtoDynamicalDataTypesAndOptionsResponse Get(long inId)
	{
		return GetAsync(inId).GetAwaiter().GetResult();
	}

	public async Task<DtoDynamicalDataTypesAndOptionsResponse> GetAsync(long inId, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoDynamicalDataTypesAndOptionsResponse();
		return MyMapper.Convert(await _dbService.GetDynamicalDataTypesAndOptionsDAL().GetAsync(inId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
	}

	public DtoDynamicalDataTypesAndOptionsListResponse GetList(DtoPaging inPaging = null)
	{
		return GetListAsync(inPaging).GetAwaiter().GetResult();
	}

	public async Task<DtoDynamicalDataTypesAndOptionsListResponse> GetListAsync(DtoPaging inPaging = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoDynamicalDataTypesAndOptionsListResponse();
		RepositoryPaging inRepoPaging = MyMapper.Convert(inPaging);
		return MyMapper.Convert(await _dbService.GetDynamicalDataTypesAndOptionsDAL().GetListAsync(inRepoPaging, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
	}

	public DtoDynamicalDataTypesAndOptionsResponse Update(DtoDynamicalDataTypesAndOptions inObject)
	{
		return UpdateAsync(inObject).GetAwaiter().GetResult();
	}

	public async Task<DtoDynamicalDataTypesAndOptionsResponse> UpdateAsync(DtoDynamicalDataTypesAndOptions inObject, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoDynamicalDataTypesAndOptionsResponse();
		RepositoryResponseBase<RepoDynamicalDataTypesAndOptions> repositoryResponseBase = await _dbService.GetDynamicalDataTypesAndOptionsDAL().GetAsync(inObject?.Id ?? 0, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (inObject != null && repositoryResponseBase?.Value != null)
		{
			inObject.CreatedUserId = repositoryResponseBase.Value.CreatedUserId;
			inObject.CreatedUserName = repositoryResponseBase.Value.CreatedUserName;
			RepoDynamicalDataTypesAndOptions inObject2 = MyMapper.Convert(inObject);
			RepositoryResponseBase<RepoDynamicalDataTypesAndOptions> repositoryResponseBase2 = await _dbService.GetDynamicalDataTypesAndOptionsDAL().UpdateAsync(inObject2, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			DtoDynamicalDataTypesAndOptionsResponse toRet = MyMapper.Convert(repositoryResponseBase2);
			if (repositoryResponseBase2.Success)
			{
				RepositoryResponseBase<int> repositoryResponseBase3 = await _dbService.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (!repositoryResponseBase3.Success)
				{
					toRet.Success = false;
					toRet.Message = repositoryResponseBase3.Message;
					toRet.MessageDescription = repositoryResponseBase3.MessageDescription;
				}
			}
			return toRet;
		}
		throw new ArgumentException($"The provided id {inObject?.Id ?? 0} does not exist in database", "inObject");
	}
}
