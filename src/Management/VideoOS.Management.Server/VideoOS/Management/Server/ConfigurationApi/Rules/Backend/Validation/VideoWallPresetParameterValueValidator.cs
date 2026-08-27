using System;
using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class VideoWallPresetParameterValueValidator : IValueValidator
{
	private readonly IVideoWallPresetProxy _videoWallPresetProxy;

	public VideoWallPresetParameterValueValidator(IVideoWallPresetProxy videoWallPresetProxy)
	{
		_videoWallPresetProxy = videoWallPresetProxy;
	}

	public bool Validate<T>(IGeminiRequestContext requestContext, T value, string expectedType, IDictionary<string, RuleParameter> parameters = null)
	{
		if (expectedType == null)
		{
			return false;
		}
		return string.Equals(GetItemType(requestContext, value.ToString(), parameters), expectedType, StringComparison.OrdinalIgnoreCase);
	}

	public string GetItemType(IGeminiRequestContext requestContext, string itemId, IDictionary<string, RuleParameter> parameters = null)
	{
		if (_videoWallPresetProxy.QueryVideoWallPreset(requestContext, itemId) == null)
		{
			return null;
		}
		return "VideoWallPreset";
	}
}
