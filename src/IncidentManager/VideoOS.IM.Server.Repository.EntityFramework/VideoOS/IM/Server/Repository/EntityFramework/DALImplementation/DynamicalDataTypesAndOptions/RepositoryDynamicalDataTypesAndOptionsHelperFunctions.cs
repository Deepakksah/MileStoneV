using System.Collections.Generic;
using System.Linq;
using VideoOS.IM.Server.Repository.EntityFramework.Context;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.DynamicalDataTypesAndOptions;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.DynamicalDataTypesAndOptions;

public static class RepositoryDynamicalDataTypesAndOptionsHelperFunctions
{
	public static void CustomGetInclude(this RepoDynamicalDataTypesAndOptions inThis, IncidentManagerContext dbContext)
	{
	}

	public static void CustomGetInclude(this IList<RepoDynamicalDataTypesAndOptions> inThis, IncidentManagerContext dbContext)
	{
		if (inThis == null || !inThis.Any())
		{
			return;
		}
		foreach (RepoDynamicalDataTypesAndOptions inThi in inThis)
		{
			inThi.CustomGetInclude(dbContext);
		}
	}
}
