using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using VideoOS.IM.Server.Logger;
using VideoOS.IM.Server.RepositoryModel.Model.Implementations.Incident;

namespace VideoOS.IM.Server.RepositoryModel.ResponseObjectModels.Validations.Incident;

public class RepoIncidentValidator
{
	public static bool Validate(RepoIncident inIncident, List<ValidationResult> inValResult)
	{
		bool result = false;
		try
		{
			result = Validator.TryValidateObject(inIncident, new ValidationContext(inIncident), inValResult, validateAllProperties: true);
		}
		catch (Exception arg)
		{
			LoggerProvider.Instance.Logger.LogError($"Error happened while validating incident: {arg}");
		}
		return result;
	}
}
