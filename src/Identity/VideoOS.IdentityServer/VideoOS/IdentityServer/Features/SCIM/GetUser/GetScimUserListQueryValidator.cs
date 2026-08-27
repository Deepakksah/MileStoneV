using System;
using System.Linq;
using FluentValidation;
using VideoOS.IdentityServer.Features.SCIM.Shared;

namespace VideoOS.IdentityServer.Features.SCIM.GetUser;

public class GetScimUserListQueryValidator : AbstractValidator<GetScimUserListQuery>
{
	private const string _filterPropertyName = "Filter";

	private static readonly string[] _supportedLogicalOperators = new string[1] { "and" };

	public GetScimUserListQueryValidator(GetUserSchemaService userSchemaService)
	{
		GetScimUserListQueryValidator getScimUserListQueryValidator = this;
		RuleFor((GetScimUserListQuery query) => query.Filter).Must(BeValidFilterSyntax).WithMessage("The specified filter syntax was invalid or unsupported logical operator used.").When((GetScimUserListQuery query) => !string.IsNullOrEmpty(query.Filter))
			.WithErrorCode("invalidFilter")
			.DependentRules(delegate
			{
				getScimUserListQueryValidator.RuleForEach((GetScimUserListQuery query) => SplitFilters(query.Filter)).SetValidator(new FilterValidator(userSchemaService)).OverridePropertyName("Filter")
					.When((GetScimUserListQuery query) => !string.IsNullOrEmpty(query.Filter))
					.WithErrorCode("invalidFilter");
			});
	}

	private static bool BeValidFilterSyntax(string filterParameters)
	{
		return SplitFilters(filterParameters).All((string filter) => filter.Trim().Split(' ').Length == 3);
	}

	private static string[] SplitFilters(string filterParameters)
	{
		if (filterParameters != null)
		{
			return filterParameters.Split(_supportedLogicalOperators, StringSplitOptions.RemoveEmptyEntries);
		}
		return Array.Empty<string>();
	}
}
