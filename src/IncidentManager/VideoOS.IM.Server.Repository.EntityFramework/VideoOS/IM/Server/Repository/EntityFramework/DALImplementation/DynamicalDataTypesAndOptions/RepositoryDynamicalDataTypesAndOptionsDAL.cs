using System;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.IM.Server.Logger;
using VideoOS.IM.Server.Repository.EntityFramework.Context;
using VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.Common;
using VideoOS.IM.Server.Repository.Interfaces.Common;
using VideoOS.IM.Server.Repository.Interfaces.DynamicalDataTypesAndOptions;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.DynamicalDataTypesAndOptions;
using VideoOS.IM.Server.RepositoryModel.ResponseObjectModels.Common;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.DynamicalDataTypesAndOptions;

public class RepositoryDynamicalDataTypesAndOptionsDAL : RepositoryEntityObjectBaseDAL<RepoDynamicalDataTypesAndOptions>, IRepositoryDynamicalDataTypesAndOptionsDAL, IRepositoryObjectBaseDAL<long, RepoDynamicalDataTypesAndOptions>
{
	public RepositoryDynamicalDataTypesAndOptionsDAL(IncidentManagerContext inDb)
		: base(inDb)
	{
		base.AdditionalQueryOnQueriable = delegate(IQueryable<RepoDynamicalDataTypesAndOptions> q)
		{
			if (q != null)
			{
				q = q.Select((RepoDynamicalDataTypesAndOptions o) => new PseudoRepoDynamicalDataTypesAndOptionsView
				{
					ClassOfData = o.ClassOfData,
					ClassOfDataint = o.ClassOfDataint,
					CreatedUserId = o.CreatedUserId,
					CreatedUserName = o.CreatedUserName,
					CustomAction = o.CustomAction,
					Description = o.Description,
					EntityScopeStr = o.EntityScopeStr,
					Id = o.Id,
					ModifiedUserId = o.ModifiedUserId,
					ModifiedUserName = o.ModifiedUserName,
					Name = o.Name,
					NumberOfForeignReferences = null,
					PropertyScopeStr = o.PropertyScopeStr,
					SystemString = o.SystemString,
					TimeCreated = o.TimeCreated,
					TimeModified = o.TimeModified,
					Title = o.Title,
					TypeOfItemStr = o.TypeOfItemStr,
					ValueBool = o.ValueBool,
					ValueDecimal = o.ValueDecimal,
					ValueInt = o.ValueInt,
					ValueStr = o.ValueStr
				});
			}
			return q;
		};
	}

	public override async Task<RepositoryResponseBase<RepoDynamicalDataTypesAndOptions>> GetAsync(long inId, CancellationToken cancellationToken)
	{
		RepositoryResponseBase<RepoDynamicalDataTypesAndOptions> toRet = new RepositoryResponseBase<RepoDynamicalDataTypesAndOptions>();
		RepoDynamicalDataTypesAndOptions dbObj = null;
		try
		{
			dbObj = await base.db.DynamicalDataTypesAndOptions.Select((RepoDynamicalDataTypesAndOptions o) => new PseudoRepoDynamicalDataTypesAndOptionsView
			{
				ClassOfData = o.ClassOfData,
				ClassOfDataint = o.ClassOfDataint,
				CreatedUserId = o.CreatedUserId,
				CreatedUserName = o.CreatedUserName,
				CustomAction = o.CustomAction,
				Description = o.Description,
				EntityScopeStr = o.EntityScopeStr,
				Id = o.Id,
				ModifiedUserId = o.ModifiedUserId,
				ModifiedUserName = o.ModifiedUserName,
				Name = o.Name,
				NumberOfForeignReferences = null,
				PropertyScopeStr = o.PropertyScopeStr,
				SystemString = o.SystemString,
				TimeCreated = o.TimeCreated,
				TimeModified = o.TimeModified,
				Title = o.Title,
				TypeOfItemStr = o.TypeOfItemStr,
				ValueBool = o.ValueBool,
				ValueDecimal = o.ValueDecimal,
				ValueInt = o.ValueInt,
				ValueStr = o.ValueStr
			}).FirstOrDefaultAsync((PseudoRepoDynamicalDataTypesAndOptionsView o) => o.Id == inId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (dbObj == null)
			{
				throw new ObjectNotFoundException($"Object with id: {inId}, not found in database.");
			}
			RepositoryCommonHelperFunctions.CustomGetInclude(dbObj, base.db);
			toRet.Success = true;
			toRet.Value = dbObj;
		}
		catch (Exception ex)
		{
			LoggerProvider.Instance.Logger.LogError(string.Format("Error happened while retrieving object from {0}: {1}", "RepoDynamicalDataTypesAndOptions", ex));
			toRet.Success = false;
			toRet.Message = ex.Message;
			toRet.MessageDescription = "Detail error: " + ex;
			toRet.Value = dbObj;
		}
		return toRet;
	}
}
