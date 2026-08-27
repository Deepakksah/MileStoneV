using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public interface IValueValidator
{
	bool Validate<T>(IGeminiRequestContext requestContext, T value, string expectedType, IDictionary<string, RuleParameter> parameters);

	string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters);
}
