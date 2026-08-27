using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using VideoOS.IdentityServer.Features.SCIM.GetSchema;
using VideoOS.IdentityServer.Features.SCIM.Shared;

namespace VideoOS.IdentityServer.Features.SCIM.GetUser;

public class FilterValidator : AbstractValidator<string>
{
	private readonly GetUserSchemaService _userSchemaService;

	private static readonly string[] SupportedAttributeOperators = new string[1] { "eq" };

	public FilterValidator(GetUserSchemaService userSchemaService)
	{
		_userSchemaService = userSchemaService;
		RuleFor((string filter) => filter).Must(HasValidAttributeName).WithMessage((string filter) => "The attribute name '" + GetParts(filter)[0] + "' is not part of ScimUser.").WithErrorCode("invalidFilter");
		RuleFor((string filter) => filter).Must(HasValidOperator).WithMessage((string filter) => "The attribute operator '" + GetParts(filter)[1] + "' is not supported.").WithErrorCode("invalidFilter");
	}

	private bool HasValidAttributeName(string filter)
	{
		string[] parts = GetParts(filter);
		return BeValidAttributeName(parts[0]);
	}

	private bool HasValidOperator(string filter)
	{
		string[] parts = GetParts(filter);
		return BeValidOperator(parts[1]);
	}

	private static string[] GetParts(string filter)
	{
		return filter.Trim().Split(' ');
	}

	private bool BeValidAttributeName(string attributeName)
	{
		return GetAllPropertyPaths().Any((string p) => p.Equals(attributeName, StringComparison.OrdinalIgnoreCase));
	}

	private static bool BeValidOperator(string operatorSymbol)
	{
		return SupportedAttributeOperators.Any((string s) => s.Equals(operatorSymbol));
	}

	private List<string> GetAllPropertyPaths()
	{
		SchemaResponse.Attribute[] userSchemaAttributes = _userSchemaService.GetUserSchemaAttributes();
		return userSchemaAttributes.SelectMany((SchemaResponse.Attribute attr) => (attr.SubAttributes == null || attr.SubAttributes.Length == 0) ? new _003C_003Ez__ReadOnlySingleElementList<string>(attr.Name) : attr.SubAttributes.Select((SchemaResponse.Attribute sub) => attr.Name + "." + sub.Name).Concat(new _003C_003Ez__ReadOnlySingleElementList<string>(attr.Name))).ToList();
	}
}
