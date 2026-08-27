using System;
using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class VideoWallParameterValueValidator : IValueValidator
{
	private readonly IVideoWallProxy _videoWallProxy;

	public VideoWallParameterValueValidator(IVideoWallProxy videoWallProxy)
	{
		_videoWallProxy = videoWallProxy;
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters = null)
	{
		if (_videoWallProxy.QueryVideoWall(requestContext, itemId) == null)
		{
			return null;
		}
		return "VideoWall";
	}

	public bool Validate<T>(IGeminiRequestContext requestContext, T value, string expectedType, IDictionary<string, RuleParameter> parameters = null)
	{
		if (expectedType == null)
		{
			return false;
		}
		return string.Equals(GetItemType(requestContext, value.ToString()), expectedType, StringComparison.OrdinalIgnoreCase);
	}
}
