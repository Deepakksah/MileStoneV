using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.IM.Common.GlobalDefinitions.Enums.Sorting;
using VideoOS.IM.Server.Logger;
using VideoOS.IM.Server.Repository.EntityFramework.Context;
using VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.Common.Filtering;
using VideoOS.IM.Server.Repository.Interfaces.Common;
using VideoOS.IM.Server.RepositoryModel.Model.Abstractions.Common;
using VideoOS.IM.Server.RepositoryModel.RepoRequestObjectModels.Filters;
using VideoOS.IM.Server.RepositoryModel.RepoRequestObjectModels.Paging;
using VideoOS.IM.Server.RepositoryModel.ResponseObjectModels.Common;
using VideoOS.IM.Server.RepositoryModel.ResponseObjectModels.Validations;
using VideoOS.IM.Server.RepositoryModel.ResponseObjectModels.Validations.Helpers;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.Common;

public class RepositoryEntityObjectBaseDAL<Tdb> : IRepositoryObjectBaseDAL<long, Tdb> where Tdb : ARepoBaseEntity
{
	public Func<IQueryable<Tdb>, IQueryable<Tdb>> AdditionalQueryOnQueriable { get; set; }

	public IncidentManagerContext db { get; set; }

	public RepositoryEntityObjectBaseDAL(IncidentManagerContext inDb)
	{
		if (inDb == null)
		{
			throw new ArgumentNullException("inDb", "Error creating implementation of Entity framework Repository Data Access Layer. Database context not exist.");
		}
		db = inDb;
	}

	public virtual Task<RepositoryResponseBase<Tdb>> CreateAsync(Tdb inObject, CancellationToken cancellationToken)
	{
		RepositoryResponseBase<Tdb> repositoryResponseBase = new RepositoryResponseBase<Tdb>();
		try
		{
			List<ValidationResult> inValResult = new List<ValidationResult>();
			if (GenericValidator.Validate(inObject, inValResult))
			{
				inObject.GetType();
				db.Set<Tdb>().Add(inObject);
				repositoryResponseBase.Success = true;
				repositoryResponseBase.Value = inObject;
			}
			else
			{
				repositoryResponseBase.Success = false;
				repositoryResponseBase.Message = "Not all properties are correct.";
				repositoryResponseBase.MessageDescription = ValidationResultConverter.GetMessageDescription(inValResult);
				repositoryResponseBase.Value = inObject;
			}
		}
		catch (Exception ex)
		{
			LoggerProvider.Instance.Logger.LogError($"Error happened while creating {typeof(Tdb)}: {ex}");
			repositoryResponseBase.Success = false;
			repositoryResponseBase.Message = ex.Message;
			repositoryResponseBase.MessageDescription = "Detail error: " + ex;
			repositoryResponseBase.Value = inObject;
		}
		return Task.FromResult(repositoryResponseBase);
	}

	public virtual async Task<RepositoryResponseBase<Tdb>> DeleteAsync(Tdb inObject, CancellationToken cancellationToken)
	{
		return await DeleteAsync(inObject?.Id ?? 0, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public virtual async Task<RepositoryResponseBase<Tdb>> DeleteAsync(long inId, CancellationToken cancellationToken)
	{
		RepositoryResponseBase<Tdb> toRet = new RepositoryResponseBase<Tdb>();
		Tdb dbObj = null;
		try
		{
			dbObj = await db.Set<Tdb>().FindAsync(cancellationToken, inId).ConfigureAwait(continueOnCapturedContext: false);
			if (dbObj == null)
			{
				throw new ObjectNotFoundException($"Delete object error. Object with id: {inId}, not found in database.");
			}
			db.Set<Tdb>().Remove(dbObj);
			toRet.Success = true;
			toRet.Value = dbObj;
		}
		catch (Exception ex)
		{
			LoggerProvider.Instance.Logger.LogError($"Error happened while deleting {typeof(Tdb)}: {ex}");
			toRet.Success = false;
			toRet.Message = ex.Message;
			toRet.MessageDescription = "Detail error: " + ex;
			toRet.Value = dbObj;
		}
		return toRet;
	}

	public async Task<RepositoryResponseBase<IEnumerable<Tdb>>> DeleteRangeAsync(HashSet<long> inIds, CancellationToken cancellationToken)
	{
		RepositoryResponseBase<IEnumerable<Tdb>> toRet = new RepositoryResponseBase<IEnumerable<Tdb>>();
		IEnumerable<Tdb> removedEntities = Enumerable.Empty<Tdb>();
		try
		{
			removedEntities = await (from x in db.Set<Tdb>()
				where inIds.Contains(x.Id)
				select x).ToListAsync(cancellationToken);
			db.Set<Tdb>().RemoveRange(removedEntities);
			toRet.Success = true;
			toRet.Value = removedEntities;
		}
		catch (Exception ex)
		{
			LoggerProvider.Instance.Logger.LogError($"Error happened while deleting {typeof(Tdb)}: {ex}");
			toRet.Success = false;
			toRet.Message = ex.Message;
			toRet.MessageDescription = $"Detail error: {ex}";
			toRet.Value = removedEntities;
		}
		return toRet;
	}

	public virtual async Task<RepositoryResponseBase<Tdb>> GetAsync(long inId, CancellationToken cancellationToken)
	{
		RepositoryResponseBase<Tdb> toRet = new RepositoryResponseBase<Tdb>();
		Tdb dbObj = null;
		try
		{
			dbObj = await db.Set<Tdb>().FindAsync(cancellationToken, inId).ConfigureAwait(continueOnCapturedContext: false);
			if (dbObj == null)
			{
				throw new ObjectNotFoundException($"Object with id: {inId}, not found in database.");
			}
			RepositoryCommonHelperFunctions.CustomGetInclude(dbObj, db);
			toRet.Success = true;
			toRet.Value = dbObj;
		}
		catch (Exception ex)
		{
			LoggerProvider.Instance.Logger.LogError($"Error happened while retrieving {typeof(Tdb)}: {ex}");
			toRet.Success = false;
			toRet.Message = ex.Message;
			toRet.MessageDescription = "Detail error: " + ex;
			toRet.Value = dbObj;
		}
		return toRet;
	}

	public virtual async Task<RepositoryResponseBase<IEnumerable<Tdb>>> GetListAsync(RepositoryPaging inRepoPaging, CancellationToken cancellationToken)
	{
		RepositoryResponseBase<IEnumerable<Tdb>> toRet = new RepositoryResponseBase<IEnumerable<Tdb>>();
		List<Tdb> currentPageList = null;
		try
		{
			if (inRepoPaging == null)
			{
				inRepoPaging = new RepositoryPaging();
			}
			if (inRepoPaging.RepositoryFilters == null || inRepoPaging.RepositoryFilters.Count == 0 || !inRepoPaging.RepositoryFilters.Any((RepositoryFilter o) => o.ApplySort))
			{
				if (inRepoPaging.RepositoryFilters == null)
				{
					inRepoPaging.RepositoryFilters = new List<RepositoryFilter>();
				}
				inRepoPaging.RepositoryFilters.Add(new RepositoryFilter
				{
					PropName = "Id",
					ApplySort = true,
					SortType = ESortTypes.Descending
				});
			}
			IQueryable<Tdb> entityQueryable = db.Set<Tdb>().AsQueryable();
			if (AdditionalQueryOnQueriable != null)
			{
				entityQueryable = AdditionalQueryOnQueriable(entityQueryable);
			}
			foreach (RepositoryFilter repositoryFilter in inRepoPaging.RepositoryFilters)
			{
				if (repositoryFilter != null)
				{
					entityQueryable = RepositoryFilteringManager<Tdb>.ApplyGlobalGeneralPropertyFilter(ref entityQueryable, repositoryFilter);
				}
			}
			RepositoryPaging repositoryPaging = inRepoPaging;
			repositoryPaging.TotalCount = await entityQueryable.CountAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (inRepoPaging.CountPerPage == 0)
			{
				inRepoPaging.CountPerPage = RepositoryPaging.GlobalPageCountSize;
			}
			inRepoPaging.TotalPages = (int)Math.Ceiling((decimal)inRepoPaging.TotalCount / (decimal)inRepoPaging.CountPerPage);
			int num = inRepoPaging.Page - 1;
			if (num < 0)
			{
				num = 0;
			}
			currentPageList = await entityQueryable.Skip(num * inRepoPaging.CountPerPage).Take(inRepoPaging.CountPerPage).ToListAsync(cancellationToken)
				.ConfigureAwait(continueOnCapturedContext: false);
			if (currentPageList == null)
			{
				throw new InvalidOperationException("Error paging object in database.");
			}
			RepositoryCommonHelperFunctions.CustomGetInclude(currentPageList.Cast<ARepoBaseEntity>().ToList(), db);
			toRet.Success = true;
			toRet.PagingObject = inRepoPaging;
			toRet.Value = currentPageList;
		}
		catch (Exception ex)
		{
			LoggerProvider.Instance.Logger.LogError($"Error happened while retrieving list of {typeof(Tdb)}: {ex}");
			toRet.Success = false;
			toRet.Message = ex.Message;
			toRet.MessageDescription = "Detail error: " + ex;
			toRet.Value = currentPageList;
		}
		return toRet;
	}

	public virtual async Task<bool> ObjectExistAsync(long inId, CancellationToken cancellationToken)
	{
		return await db.Set<Tdb>().AnyAsync((Tdb o) => o.Id == inId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public virtual async Task<RepositoryResponseBase<Tdb>> UpdateAsync(Tdb inObject, CancellationToken cancellationToken)
	{
		RepositoryResponseBase<Tdb> toRet = new RepositoryResponseBase<Tdb>();
		Tdb dbObject = null;
		try
		{
			long inId = inObject?.Id ?? 0;
			dbObject = await db.Set<Tdb>().FirstOrDefaultAsync((Tdb o) => o.Id == inId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (dbObject == null)
			{
				throw new ObjectNotFoundException($"Update object error. Object with id: {inId}, not found in database.");
			}
			List<ValidationResult> inValResult = new List<ValidationResult>();
			if (!GenericValidator.Validate(inObject, inValResult))
			{
				toRet.Success = false;
				toRet.Message = "Not all properties are correct.";
				toRet.MessageDescription = ValidationResultConverter.GetMessageDescription(inValResult);
				toRet.Value = inObject;
				return toRet;
			}
			db.Entry(dbObject).CurrentValues.SetValues(inObject);
			dbObject.FlagNoDb_UserOpenedIncident = inObject.FlagNoDb_UserOpenedIncident;
			toRet.Success = true;
			toRet.Value = dbObject;
		}
		catch (Exception ex)
		{
			LoggerProvider.Instance.Logger.LogError($"Error happened while updating {typeof(Tdb)}: {ex}");
			toRet.Success = false;
			toRet.Message = ex.Message;
			toRet.MessageDescription = "Detail error: " + ex;
			toRet.Value = dbObject;
		}
		return toRet;
	}
}
