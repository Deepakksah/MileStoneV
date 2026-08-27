using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.RestApi;

public static class RestApiCache
{
	private static ConcurrentDictionary<string, MethodInfo> _methods = null;

	private static readonly ConcurrentDictionary<string, Dictionary<string, string>> Languages = new ConcurrentDictionary<string, Dictionary<string, string>>();

	private static bool _initialized = false;

	public static IConfigurationApiManager ConfigurationApiManager { get; set; } = new ConfigurationApiManager();

	public static Guid SiteId { get; private set; } = Guid.Empty;

	public static MethodInfo GetMethodInfo(string methodId)
	{
		if (_methods == null)
		{
			MethodInfo[] methodDefinitions = ConfigurationApiManager.GetMethodDefinitions();
			_methods = new ConcurrentDictionary<string, MethodInfo>();
			MethodInfo[] array = methodDefinitions;
			foreach (MethodInfo methodInfo in array)
			{
				_methods[methodInfo.MethodId] = methodInfo;
			}
		}
		if (_methods.TryGetValue(methodId, out var value))
		{
			return value;
		}
		return null;
	}

	public static Dictionary<string, string> GetTranslations(IRestApiRequestContext context, string language)
	{
		if (!Languages.TryGetValue(language, out var value))
		{
			value = ConfigurationApiManager.GetTranslations(context.RestApiRequestParameters.Language);
			Languages[language] = value;
		}
		return value;
	}

	public static string GetTranslationString(IRestApiRequestContext context, string language, string id)
	{
		Dictionary<string, string> translations = GetTranslations(context, language);
		if (translations == null)
		{
			return null;
		}
		if (translations.TryGetValue(id, out var value))
		{
			return value;
		}
		return null;
	}

	public static void Initialize(IRestApiRequestContext requestContext)
	{
		if (!_initialized)
		{
			if (SiteId == Guid.Empty)
			{
				SiteId = new Guid(ServerInformation.GetServerInformation(requestContext).Id);
			}
			_initialized = true;
		}
	}
}
