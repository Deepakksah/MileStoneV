using System.Collections.Generic;
using System.Linq;
using VideoOS.IM.Server.Repository.EntityFramework.Context;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentSequence;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.IncidentSequence;

public static class RepositoryIncidentSequenceHelperFunctions
{
	public static void CustomGetInclude(this RepoIncidentSequence inThis, IncidentManagerContext dbContext)
	{
	}

	public static void CustomGetInclude(this IList<RepoIncidentSequence> inThis, IncidentManagerContext dbContext)
	{
		if (inThis == null || !inThis.Any())
		{
			return;
		}
		foreach (RepoIncidentSequence inThi in inThis)
		{
			inThi.CustomGetInclude(dbContext);
		}
	}
}
