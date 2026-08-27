using System.Collections.Generic;
using System.Linq;
using VideoOS.IM.Server.Repository.EntityFramework.Context;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.IncidentComment;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.IncidentComment;

public static class RepositoryIncidentCommentHelperFunctions
{
	public static void CustomGetInclude(this RepoIncidentComment inThis, IncidentManagerContext dbContext)
	{
	}

	public static void CustomGetInclude(this IList<RepoIncidentComment> inThis, IncidentManagerContext dbContext)
	{
		if (inThis == null || !inThis.Any())
		{
			return;
		}
		foreach (RepoIncidentComment inThi in inThis)
		{
			inThi.CustomGetInclude(dbContext);
		}
	}
}
