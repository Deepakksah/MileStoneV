using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.IM.Common.Dto.DtoModels.Implementations.DraftSequence;
using VideoOS.IM.Common.Dto.DtoRequestObjectModels.Paging;
using VideoOS.IM.Common.Dto.DtoResponseObjectModels.DraftSequence;
using VideoOS.IM.Common.Service.Interfaces.Common;
using VideoOS.IM.Common.Service.Interfaces.DraftSequence;
using VideoOS.IM.Common.Service.Properties;
using VideoOS.IM.Server.DataMapper.CustomMapperFramework;
using VideoOS.IM.Server.Repository.Interfaces;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.DraftSequence;
using VideoOS.IM.Server.RepositoryModel.RepoRequestObjectModels.Paging;
using VideoOS.IM.Server.RepositoryModel.ResponseObjectModels.Common;
using VideoOS.IM.Server.Service.DALImplementation.Common;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Service.DALImplementation.DraftSequence;

public sealed class DtoDraftSequenceDAL : DtoObjectBaseDAL, IDtoDraftSequenceDAL, IDtoObjectBaseDAL<long, DtoDraftSequence, DtoDraftSequenceResponse, DtoDraftSequenceListResponse>
{
	public DtoDraftSequenceDAL(IRepositoryDAL inDbService)
		: base(inDbService)
	{
	}

	public DtoDraftSequenceResponse Create(DtoDraftSequence inObject)
	{
		return CreateAsync(inObject).GetAwaiter().GetResult();
	}

	public async Task<DtoDraftSequenceResponse> CreateAsync(DtoDraftSequence inObject, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoDraftSequenceResponse();
		RepoDraftSequence dbObject = MyMapper.Convert(inObject);
		RepositoryResponseBase<RepoDraftSequence> repositoryResponseBase = await _dbService.GetDraftSequenceDAL().CreateAsync(dbObject, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		DtoDraftSequenceResponse toRet = MyMapper.Convert(repositoryResponseBase);
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

	public DtoDraftSequenceResponse Delete(long inObjectId)
	{
		return DeleteAsync(inObjectId).GetAwaiter().GetResult();
	}

	public async Task<DtoDraftSequenceResponse> DeleteAsync(long inObjectId, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoDraftSequenceResponse();
		if (await _dbService.GetDraftSequenceDAL().ObjectExistAsync(inObjectId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
		{
			RepositoryResponseBase<RepoDraftSequence> repositoryResponseBase = await _dbService.GetDraftSequenceDAL().DeleteAsync(inObjectId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			DtoDraftSequenceResponse toRet = MyMapper.Convert(repositoryResponseBase);
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

	public async Task<DtoDraftSequenceListResponse> DeleteRangeAsync([NotNull] HashSet<long> inObjectIds, CancellationToken cancellationToken = default(CancellationToken))
	{
		inObjectIds.NullCheck("", "DeleteRangeAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\DALImplementation\\DraftSequence\\DtoDraftSequenceDAL.cs", 95);
		if (!inObjectIds.Any())
		{
			throw new ArgumentException("Ids should have been provided.", "inObjectIds");
		}
		RepositoryResponseBase<IEnumerable<RepoDraftSequence>> repositoryResponseBase = await _dbService.GetDraftSequenceDAL().DeleteRangeAsync(inObjectIds, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		DtoDraftSequenceListResponse toRet = MyMapper.Convert(repositoryResponseBase);
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

	public DtoDraftSequenceResponse Get(long inId)
	{
		return GetAsync(inId).GetAwaiter().GetResult();
	}

	public async Task<DtoDraftSequenceResponse> GetAsync(long inId, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoDraftSequenceResponse();
		return MyMapper.Convert(await _dbService.GetDraftSequenceDAL().GetAsync(inId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
	}

	public DtoDraftSequenceListResponse GetList(DtoPaging inPaging = null)
	{
		return GetListAsync(inPaging).GetAwaiter().GetResult();
	}

	public async Task<DtoDraftSequenceListResponse> GetListAsync(DtoPaging inPaging = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoDraftSequenceListResponse();
		RepositoryPaging inRepoPaging = MyMapper.Convert(inPaging);
		return MyMapper.Convert(await _dbService.GetDraftSequenceDAL().GetListAsync(inRepoPaging, cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
	}

	public DtoDraftSequenceResponse Update(DtoDraftSequence inObject)
	{
		return UpdateAsync(inObject).GetAwaiter().GetResult();
	}

	public async Task<DtoDraftSequenceResponse> UpdateAsync(DtoDraftSequence inObject, CancellationToken cancellationToken = default(CancellationToken))
	{
		new DtoDraftSequenceResponse();
		if (await _dbService.GetDraftSequenceDAL().ObjectExistAsync(inObject?.Id ?? 0, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
		{
			RepoDraftSequence inObject2 = MyMapper.Convert(inObject);
			RepositoryResponseBase<RepoDraftSequence> repositoryResponseBase = await _dbService.GetDraftSequenceDAL().UpdateAsync(inObject2, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			DtoDraftSequenceResponse toRet = MyMapper.Convert(repositoryResponseBase);
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
