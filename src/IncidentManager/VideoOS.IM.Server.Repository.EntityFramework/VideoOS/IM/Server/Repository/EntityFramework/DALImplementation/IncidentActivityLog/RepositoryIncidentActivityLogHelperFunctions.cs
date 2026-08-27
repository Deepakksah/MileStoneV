using System.Collections.Generic;
using System.Linq;
using VideoOS.IM.Server.Repository.EntityFramework.Context;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentActivityLog;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.IncidentActivityLog;

public static class RepositoryIncidentActivityLogHelperFunctions
{
	public static void CustomGetInclude(this RepoIncidentActivityLog inThis, IncidentManagerContext dbContext)
	{
	}

	public static void CustomGetInclude(this IList<RepoIncidentActivityLog> inThis, IncidentManagerContext dbContext)
	{
		if (inThis == null || !inThis.Any())
		{
			return;
		}
		foreach (RepoIncidentActivityLog inThi in inThis)
		{
			inThi.CustomGetInclude(dbContext);
		}
	}
}
