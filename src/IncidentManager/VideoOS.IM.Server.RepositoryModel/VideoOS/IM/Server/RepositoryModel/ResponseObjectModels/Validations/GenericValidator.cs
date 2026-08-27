using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VideoOS.IM.Server.RepositoryModel.Model.Abstractions.Common;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.Incident;
using VideoOS.IM.Server.RepositoryModel.ResponseObjectModels.Validations.Incident;

namespace VideoOS.IM.Server.RepositoryModel.ResponseObjectModels.Validations;

public class GenericValidator
{
	public static bool Validate(ARepoBaseEntity inObject, List<ValidationResult> inValResult)
	{
		if (inObject != null && typeof(RepoIncident) == inObject.GetType())
		{
			return RepoIncidentValidator.Validate(inObject as RepoIncident, inValResult);
		}
		return true;
	}
}
