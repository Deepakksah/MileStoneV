using System.Collections.Generic;
using System.Linq;
using VideoOS.IM.Server.Repository.EntityFramework.Context;
using VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.Incident;
using VideoOS.IM.Server.RepositoryModel.Model.Abstractions.Common;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.Incident;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.Common;

public class RepositoryCommonHelperFunctions
{
	public static void CustomGetInclude(ARepoBaseEntity inThis, IncidentManagerContext dbContext)
	{
		if (inThis != null && typeof(RepoIncident) == inThis.GetType())
		{
			(inThis as RepoIncident).CustomGetInclude(dbContext);
		}
	}

	public static void CustomGetInclude(List<ARepoBaseEntity> inThis, IncidentManagerContext dbContext)
	{
		if (inThis == null || !inThis.Any())
		{
			return;
		}
		foreach (ARepoBaseEntity inThi in inThis)
		{
			CustomGetInclude(inThi, dbContext);
		}
	}
}
