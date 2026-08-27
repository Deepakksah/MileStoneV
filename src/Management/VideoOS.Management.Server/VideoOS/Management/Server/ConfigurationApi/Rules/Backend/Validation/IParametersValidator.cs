using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Definitions;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public interface IParametersValidator
{
	ValidationError[] Validate(IGeminiRequestContext requestContext, IDictionary<string, RuleParameter> parameters, IDictionary<string, RuleParameterDefinition> validationModels);
}
