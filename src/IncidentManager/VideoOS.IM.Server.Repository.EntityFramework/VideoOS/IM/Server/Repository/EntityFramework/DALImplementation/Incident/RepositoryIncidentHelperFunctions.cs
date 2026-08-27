using System.Collections.Generic;
using System.Linq;
using VideoOS.IM.Server.Repository.EntityFramework.Context;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.Incident;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.Incident;

public static class RepositoryIncidentHelperFunctions
{
	public static void CustomGetInclude(this RepoIncident inThis, IncidentManagerContext dbContext)
	{
		if (inThis != null)
		{
			dbContext.Entry(inThis).Collection((RepoIncident o) => o.SequencesList).Load();
		}
	}

	public static void CustomGetInclude(this IList<RepoIncident> inThis, IncidentManagerContext dbContext)
	{
		if (inThis == null || !inThis.Any())
		{
			return;
		}
		foreach (RepoIncident inThi in inThis)
		{
			inThi.CustomGetInclude(dbContext);
		}
	}
}
