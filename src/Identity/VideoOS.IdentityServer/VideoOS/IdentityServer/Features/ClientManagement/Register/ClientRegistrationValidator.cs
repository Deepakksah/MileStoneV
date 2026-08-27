using FluentValidation;

namespace VideoOS.IdentityServer.Features.ClientManagement.Register;

public sealed class ClientRegistrationValidator : AbstractValidator<RegisterClientCommand>
{
	public ClientRegistrationValidator()
	{
		RuleFor((RegisterClientCommand x) => x.ClientName).NotEmpty().WithState<RegisterClientCommand, string>((RegisterClientCommand _) => ClientRegistrationErrors.ClientNameRequired());
		RuleFor((RegisterClientCommand x) => x.ClientName).Length(1, 40).WithState<RegisterClientCommand, string>((RegisterClientCommand _) => ClientRegistrationErrors.ClientNameLengthValidation());
		RuleFor((RegisterClientCommand x) => x.ClientScopes).NotEmpty().WithState<RegisterClientCommand, string[]>((RegisterClientCommand _) => ClientRegistrationErrors.ClientScopesRequired());
		RuleFor((RegisterClientCommand x) => x.ClientGrantTypes).NotEmpty().WithState<RegisterClientCommand, string[]>((RegisterClientCommand _) => ClientRegistrationErrors.ClientGrantTypesRequired());
	}
}
