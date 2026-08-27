using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace VideoOS.IdentityServer.Features.SCIM.Shared;

public static class ScimValidationRules
{
	public static IRuleBuilderOptions<T, IEnumerable<string>> ValidateScimSchemas<T>(this IRuleBuilder<T, IEnumerable<string>> ruleBuilder)
	{
		return ruleBuilder.NotNull().WithMessage("Schemas cannot be null.").NotEmpty()
			.WithMessage("Schemas cannot be empty.")
			.Must((IEnumerable<string> schemas) => schemas.Contains("urn:ietf:params:scim:schemas:core:2.0:User"))
			.WithMessage("Required schema urn:ietf:params:scim:schemas:core:2.0:User is missing.");
	}
}
