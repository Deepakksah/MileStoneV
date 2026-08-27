using FluentValidation;
using VideoOS.IdentityServer.Features.SCIM.Shared;

namespace VideoOS.IdentityServer.Features.SCIM.CreateUser;

public class CreateScimUserCommandValidator : AbstractValidator<CreateScimUserCommand>
{
	public CreateScimUserCommandValidator()
	{
		RuleFor((CreateScimUserCommand x) => x.Schemas).Cascade(CascadeMode.Stop).ValidateScimSchemas();
		RuleFor((CreateScimUserCommand x) => x.UserName).NotEmpty().WithMessage("UserName cannot be empty.").Matches("^[^?/[\\]\\\\]+$")
			.WithMessage("UserName contains disallowed characters.");
		RuleFor((CreateScimUserCommand x) => x.ExternalId).NotEmpty().WithMessage("ExternalId cannot be null or empty.");
	}
}
