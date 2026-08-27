using System.Collections.Generic;
using System.Linq;
using VideoOS.IM.Server.Repository.EntityFramework.Context;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.GlobalSettings;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.GlobalSettings;

public static class RepositoryGlobalSettingsHelperFunctions
{
	public static void CustomGetInclude(this RepoGlobalSettings inThis, IncidentManagerContext dbContext)
	{
	}

	public static void CustomGetInclude(this IList<RepoGlobalSettings> inThis, IncidentManagerContext dbContext)
	{
		if (inThis == null || !inThis.Any())
		{
			return;
		}
		foreach (RepoGlobalSettings inThi in inThis)
		{
			inThi.CustomGetInclude(dbContext);
		}
	}
}
