using FluentValidation;
using VideoOS.IdentityServer.Features.SCIM.Shared;

namespace VideoOS.IdentityServer.Features.SCIM.PutUser;

public class PutScimUserCommandValidator : AbstractValidator<PutScimUserCommand>
{
	public PutScimUserCommandValidator()
	{
		RuleFor((PutScimUserCommand x) => x.Schemas).Cascade(CascadeMode.Stop).ValidateScimSchemas();
	}
}
