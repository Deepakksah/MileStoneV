using System.Collections.Generic;
using System.Linq;
using VideoOS.IM.Server.Repository.EntityFramework.Context;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentCall;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.IncidentCall;

public static class RepositoryIncidentCallHelperFunctions
{
	public static void CustomGetInclude(this RepoIncidentCall inThis, IncidentManagerContext dbContext)
	{
	}

	public static void CustomGetInclude(this IList<RepoIncidentCall> inThis, IncidentManagerContext dbContext)
	{
		if (inThis == null || !inThis.Any())
		{
			return;
		}
		foreach (RepoIncidentCall inThi in inThis)
		{
			inThi.CustomGetInclude(dbContext);
		}
	}
}
