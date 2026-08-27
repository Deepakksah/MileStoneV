using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.IM.Server.Logger;
using VideoOS.IM.Server.Repository.Abstractions;
using VideoOS.IM.Server.Repository.EntityFramework.Context;
using VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.DraftSequence;
using VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.DynamicalDataTypesAndOptions;
using VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.GlobalSettings;
using VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.Incident;
using VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.IncidentActivityLog;
using VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.IncidentCall;
using VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.IncidentComment;
using VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.IncidentSequence;
using VideoOS.IM.Server.RepositoryModel.Model.Abstractions.Common;
using VideoOS.IM.Server.RepositoryModel.ResponseObjectModels.Common;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation;

public class EntityFrameworkDAL : ARepositoryDAL
{
	public IncidentManagerContext db { get; set; }

	public EntityFrameworkDAL(string inConnectionString)
	{
		db = new IncidentManagerContext(inConnectionString);
		base.IncidentDALService = new RepositoryIncidentDAL(db);
		base.DraftSequenceDALService = new RepositoryDraftSequenceDAL(db);
		base.DynamicalDataTypesAndOptionsDALService = new RepositoryDynamicalDataTypesAndOptionsDAL(db);
		base.IncidentCallDALService = new RepositoryIncidentCallDAL(db);
		base.IncidentSequenceDALService = new RepositoryIncidentSequenceDAL(db);
		base.IncidentCommentDALService = new RepositoryIncidentCommentDAL(db);
		base.IncidentActivityLogDALService = new RepositoryIncidentActivityLogDAL(db);
		base.GlobalSettingsDALService = new RepositoryGlobalSettingsDAL(db);
	}

	public override async Task<RepositoryResponseBase<int>> SaveChangesAsync(CancellationToken cancellationToken)
	{
		RepositoryResponseBase<int> toRet = new RepositoryResponseBase<int>();
		try
		{
			int value = await db.SaveChangesAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			toRet.Success = true;
			toRet.Value = value;
		}
		catch (Exception arg)
		{
			toRet.Success = false;
			toRet.Value = -1;
			LoggerProvider.Instance.Logger.LogError($"Error while saving changes to DB: {arg}");
		}
		return toRet;
	}

	public override async Task UpdateObjectPropertiesFromDbAsync(ARepoBaseEntity inObject, CancellationToken cancellationToken)
	{
		try
		{
			await db.Entry(inObject).ReloadAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (Exception arg)
		{
			LoggerProvider.Instance.Logger.LogError($"Error while reloading object from DB: {arg}");
		}
	}
}
