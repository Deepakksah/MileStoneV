using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using VideoOS.IdentityServer.Validators;

namespace VideoOS.IdentityServer.Services.Providers;

public class CustomValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
{
	private readonly IValidationAttributeAdapterProvider _baseProvider = new ValidationAttributeAdapterProvider();

	private readonly IPasswordRequirements _passwordConfgiration;

	public CustomValidationAttributeAdapterProvider(IPasswordRequirements passwordRequirements)
	{
		_passwordConfgiration = passwordRequirements;
	}

	public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
	{
		if (attribute is PasswordRequirementsAttribute attribute2)
		{
			return new PasswordRequirementsAttributeAdapter(attribute2, stringLocalizer, _passwordConfgiration);
		}
		return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);
	}
}
