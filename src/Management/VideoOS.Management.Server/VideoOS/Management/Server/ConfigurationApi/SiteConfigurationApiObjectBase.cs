using System;
using System.Collections.Generic;
using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.ConfigurationApi;

public abstract class SiteConfigurationApiObjectBase : SecurityConfigurationApi, IConfigurationApiSupport
{
	private class DefaultRepository : ISiteRepository
	{
		public static ISiteRepository Instance { get; } = new DefaultRepository();

		private DefaultRepository()
		{
		}

		public Site[] QuerySlaveSites(IGeminiRequestContext requestContext)
		{
			return Site.QuerySlaveSites(requestContext);
		}

		public Site[] QuerySlaveSites(IGeminiRequestContext requestContext, string parentId)
		{
			return Site.QuerySlaveSites(requestContext, parentId);
		}

		public Site QuerySite(IGeminiRequestContext requestContext, string id)
		{
			return Site.QuerySite(requestContext, id);
		}

		public Site QueryHomeSite(IGeminiRequestContext requestContext)
		{
			return Site.QueryHomeSite(requestContext);
		}

		public ServerInformation GetServerInformation(IGeminiRequestContext requestContext)
		{
			return ServerInformation.GetServerInformation(requestContext);
		}
	}

	private static Dictionary<string, List<string>> _actionsToExclude = new Dictionary<string, List<string>> { 
	{
		"Site",
		new List<string>()
	} };

	protected ISiteRepository Repository { get; }

	protected override Dictionary<string, List<string>> ItemTypeActionsToExclude => _actionsToExclude;

	protected SiteConfigurationApiObjectBase(ISiteRepository repository)
	{
		Repository = repository ?? DefaultRepository.Instance;
	}

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
	}

	public abstract ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path);

	public abstract ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path);

	public abstract ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId);

	public override bool SetSecurityOnRoleOnly()
	{
		return true;
	}
}
