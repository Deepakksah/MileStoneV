using System.Collections.Generic;
using System.Linq;
using VideoOS.IM.Server.Repository.EntityFramework.Context;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.DraftSequence;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.DraftSequence;

public static class RepositoryDraftSequenceHelperFunctions
{
	public static void CustomGetInclude(this RepoDraftSequence inThis, IncidentManagerContext dbContext)
	{
	}

	public static void CustomGetInclude(this IList<RepoDraftSequence> inThis, IncidentManagerContext dbContext)
	{
		if (inThis == null || !inThis.Any())
		{
			return;
		}
		foreach (RepoDraftSequence inThi in inThis)
		{
			inThi.CustomGetInclude(dbContext);
		}
	}
}
