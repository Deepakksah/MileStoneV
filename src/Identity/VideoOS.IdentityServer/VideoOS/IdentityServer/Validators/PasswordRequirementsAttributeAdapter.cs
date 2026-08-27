using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using VideoOS.IdentityServer.Services;

namespace VideoOS.IdentityServer.Validators;

public class PasswordRequirementsAttributeAdapter : AttributeAdapterBase<PasswordRequirementsAttribute>
{
	private readonly IPasswordRequirements _passwordRequirements;

	public PasswordRequirementsAttributeAdapter(PasswordRequirementsAttribute attribute, IStringLocalizer stringLocalizer, IPasswordRequirements passwordRequirements)
		: base(attribute, stringLocalizer)
	{
		_passwordRequirements = passwordRequirements;
	}

	public override void AddValidation(ClientModelValidationContext context)
	{
		ValidationAttributeAdapter<PasswordRequirementsAttribute>.MergeAttribute(context.Attributes, "data-val", "true");
		ValidationAttributeAdapter<PasswordRequirementsAttribute>.MergeAttribute(context.Attributes, "data-val-global-unicode-regex", GetErrorMessage(context));
		string pattern = _passwordRequirements.Pattern;
		ValidationAttributeAdapter<PasswordRequirementsAttribute>.MergeAttribute(context.Attributes, "data-val-global-unicode-regex-pattern", pattern);
	}

	public override string GetErrorMessage(ModelValidationContextBase validationContext)
	{
		return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName());
	}
}
