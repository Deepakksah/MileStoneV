using System;
using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class ArchiveParameterValueValidator : IValueValidator
{
	private readonly IArchiveProxy _archiveProxy;

	public ArchiveParameterValueValidator(IArchiveProxy archiveProxy)
	{
		_archiveProxy = archiveProxy;
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters)
	{
		if (_archiveProxy.QueryArchiveStorage(requestContext, itemId) == null)
		{
			return null;
		}
		return "ArchiveStorage";
	}

	public bool Validate<T>(IGeminiRequestContext requestContext, T value, string expectedType, IDictionary<string, RuleParameter> parameters)
	{
		if (expectedType == null)
		{
			return false;
		}
		return string.Equals(GetItemType(requestContext, value.ToString(), parameters), expectedType, StringComparison.OrdinalIgnoreCase);
	}
}
