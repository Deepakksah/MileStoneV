using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VideoOS.IM.Server.RepositoryModel.ResponseObjectModels.Validations.Helpers;

public class ValidationResultConverter
{
	public static string GetMessageDescription(List<ValidationResult> inValResult)
	{
		string text = "Details: ";
		if (inValResult != null && inValResult.Any())
		{
			foreach (ValidationResult item in inValResult)
			{
				if (item != null)
				{
					text += item.ErrorMessage;
				}
			}
		}
		return text;
	}
}
