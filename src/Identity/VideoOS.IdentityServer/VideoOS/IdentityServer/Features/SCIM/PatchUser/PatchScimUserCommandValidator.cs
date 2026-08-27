using System;
using System.Linq;
using System.Text.Json;
using FluentValidation;

namespace VideoOS.IdentityServer.Features.SCIM.PatchUser;

public class PatchScimUserCommandValidator : AbstractValidator<PatchScimUserCommand>
{
	private class OperationsValidator : AbstractValidator<ScimOperation[]>
	{
		public OperationsValidator()
		{
			RuleForEach((ScimOperation[] x) => x).Cascade(CascadeMode.Stop).NotNull().Must((ScimOperation o) => o.Path != null)
				.WithMessage("Invalid operation. 'path' cannot be null.")
				.WithErrorCode("invalidPath")
				.Must((ScimOperation o) => !string.IsNullOrEmpty(o.Path))
				.WithMessage("Invalid operation. 'path' cannot be empty.")
				.WithErrorCode("invalidPath")
				.Must((ScimOperation o) => o.Path.Equals("active", StringComparison.OrdinalIgnoreCase))
				.WithMessage("Invalid operation. Only 'active' path can be patched.")
				.WithErrorCode("noTarget")
				.Must((ScimOperation o) => o.Op != null)
				.WithMessage("Invalid operation. 'op' cannot be null.")
				.Must((ScimOperation o) => !string.IsNullOrEmpty(o.Op))
				.WithMessage("Invalid operation. 'op' cannot be empty.")
				.Must((ScimOperation o) => o.Op.Equals("replace", StringComparison.OrdinalIgnoreCase))
				.WithMessage("Invalid operation. 'op' only support 'replace'.")
				.Must((ScimOperation o) => o.Value.HasValue && (o.Value.Value.ValueKind == JsonValueKind.True || o.Value.Value.ValueKind == JsonValueKind.False))
				.WithMessage("Invalid operation. 'value' must be a boolean.");
		}
	}

	private static readonly string[] SupportedSchemas = new string[1] { "urn:ietf:params:scim:api:messages:2.0:PatchOp" };

	public PatchScimUserCommandValidator()
	{
		base.RuleLevelCascadeMode = CascadeMode.Stop;
		RuleFor((PatchScimUserCommand x) => x.Schemas).NotNull().WithMessage("Schemas cannot be null.").NotEmpty()
			.WithMessage("Schemas cannot be empty.")
			.Must((string[] schemas) => schemas.All((string s) => Enumerable.Contains(SupportedSchemas, s)))
			.WithMessage("The request must contain the SCIM 2.0 PatchOp schema.");
		RuleFor((PatchScimUserCommand x) => x.Id).NotEmpty().WithMessage("Id cannot be null or empty.");
		RuleFor((PatchScimUserCommand x) => x.Operations).NotNull().WithMessage("Operations cannot be null.").NotEmpty()
			.WithMessage("Operations cannot be empty.")
			.SetValidator(new OperationsValidator());
	}
}
