using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.IM.Server.Logger;
using VideoOS.IM.Server.Repository.EntityFramework.Context;
using VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.Common;
using VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.Incident;
using VideoOS.IM.Server.Repository.Interfaces.Common;
using VideoOS.IM.Server.Repository.Interfaces.IncidentSequence;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.Incident;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentSequence;
using VideoOS.IM.Server.RepositoryModel.ResponseObjectModels.Common;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.IncidentSequence;

public class RepositoryIncidentSequenceDAL : RepositoryEntityObjectBaseDAL<RepoIncidentSequence>, IRepositoryIncidentSequenceDAL, IRepositoryObjectBaseDAL<long, RepoIncidentSequence>
{
	public RepositoryIncidentSequenceDAL(IncidentManagerContext inDb)
		: base(inDb)
	{
	}

	public override async Task<RepositoryResponseBase<RepoIncidentSequence>> CreateAsync(RepoIncidentSequence inObject, CancellationToken cancellationToken)
	{
		RepositoryResponseBase<RepoIncidentSequence> toRet = await base.CreateAsync(inObject, cancellationToken);
		try
		{
			await UpdateIncidentStartEndTimeAsync(toRet, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (Exception arg)
		{
			LoggerProvider.Instance.Logger.LogError(string.Format("Error happened while creating {0}: {1}", "RepoIncidentSequence", arg));
		}
		return toRet;
	}

	public override async Task<RepositoryResponseBase<RepoIncidentSequence>> DeleteAsync(RepoIncidentSequence inObject, CancellationToken cancellationToken)
	{
		long? removedIncidentId = inObject.IncidentId;
		RepositoryResponseBase<RepoIncidentSequence> toRet = await base.DeleteAsync(inObject, cancellationToken);
		try
		{
			if (removedIncidentId.HasValue)
			{
				await UpdateIncidentStartEndTimeAsync(removedIncidentId.Value, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		catch (Exception arg)
		{
			LoggerProvider.Instance.Logger.LogError(string.Format("Error happened while deleting {0}: {1}", "RepoIncidentSequence", arg));
		}
		return toRet;
	}

	public override async Task<RepositoryResponseBase<RepoIncidentSequence>> DeleteAsync(long inId, CancellationToken cancellationToken)
	{
		long? removedIncidentId = base.db.IncidentSequence.FirstOrDefault((RepoIncidentSequence o) => o.Id == inId)?.IncidentId;
		RepositoryResponseBase<RepoIncidentSequence> toRet = await base.DeleteAsync(inId, cancellationToken);
		try
		{
			if (removedIncidentId.HasValue)
			{
				await UpdateIncidentStartEndTimeAsync(removedIncidentId.Value, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		catch (Exception arg)
		{
			LoggerProvider.Instance.Logger.LogError(string.Format("Error happened while deleting {0}: {1}", "RepoIncidentSequence", arg));
		}
		return toRet;
	}

	public override async Task<RepositoryResponseBase<RepoIncidentSequence>> UpdateAsync(RepoIncidentSequence inObject, CancellationToken cancellationToken)
	{
		RepositoryResponseBase<RepoIncidentSequence> toRet = await base.UpdateAsync(inObject, cancellationToken);
		try
		{
			await UpdateIncidentStartEndTimeAsync(toRet, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (Exception arg)
		{
			LoggerProvider.Instance.Logger.LogError(string.Format("Error happened while updating {0}: {1}", "RepoIncidentSequence", arg));
		}
		return toRet;
	}

	private async Task UpdateIncidentStartEndTimeAsync(RepositoryResponseBase<RepoIncidentSequence> responseIncidentSequence, CancellationToken cancellationToken)
	{
		if (base.db != null && responseIncidentSequence.Value.IncidentId.HasValue && responseIncidentSequence.Value.IncidentId > 0)
		{
			await UpdateIncidentStartEndTimeAsync(responseIncidentSequence.Value.IncidentId.Value, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	private async Task UpdateIncidentStartEndTimeAsync(long incidentId, CancellationToken cancellationToken)
	{
		RepoIncident repoIncident = base.db.Incidents.Find(incidentId);
		if (repoIncident != null && repoIncident.SequencesList != null)
		{
			List<DateTime> list = (from l in repoIncident.SequencesList
				where l.StartTime.HasValue
				select l.StartTime.Value).ToList();
			List<DateTime> list2 = (from o in repoIncident.SequencesList
				where o.EndTime.HasValue
				select o.EndTime.Value).ToList();
			List<DateTime> list3 = new List<DateTime>();
			if (list.Any())
			{
				list3.AddRange(list);
			}
			if (list2.Any())
			{
				list3.AddRange(list2);
			}
			if (list3.Any())
			{
				DateTime value = list3.Min();
				DateTime value2 = list3.Max();
				repoIncident.Start = value;
				repoIncident.End = value2;
				await new RepositoryIncidentDAL(base.db).UpdateAsync(repoIncident, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
	}
}
