using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class MatrixParameterValueValidator : IValueValidator
{
	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters)
	{
		if (MatrixViewData.QueryMatrix(requestContext, itemId) != null)
		{
			return "Matrix";
		}
		return string.Empty;
	}

	public bool Validate<T>(IGeminiRequestContext requestContext, T value, string expectedType, IDictionary<string, RuleParameter> parameters)
	{
		return MatrixViewData.QueryMatrix(requestContext, value.ToString()) != null;
	}
}
