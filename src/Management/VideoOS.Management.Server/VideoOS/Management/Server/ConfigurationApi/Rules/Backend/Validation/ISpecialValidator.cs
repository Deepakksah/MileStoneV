using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public interface ISpecialValidator<T>
{
	bool Validate(T model, IDictionary<string, RuleParameter> parameters);
}
