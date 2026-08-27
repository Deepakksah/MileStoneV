using System;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.BusinessObjects.ManagementServer.SearchTemplate;

namespace VideoOS.Management.Server.ConfigurationApi.BusinessObjects.SearchTemplate;

public abstract class SearchTemplateConfigurationApiBase : ConfigurationApiBase, IConfigurationApiSupport
{
	private class DefaultRepository : ISearchTemplateRepository
	{
		public bool IsLicensed(IGeminiRequestContext requestContext)
		{
			return GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SaveSearches");
		}

		public ISearchTemplateItem CreateSearchTemplate(IGeminiRequestContext requestContext, Guid id, string name, string description, string searchQuery, SearchTemplateAvailability availability, bool includesScopeItems)
		{
			return SearchTemplateItem.CreateSearchTemplate(requestContext, id, name, description, searchQuery, availability, includesScopeItems);
		}

		public ISearchTemplateFind CreateSearchTemplateFind(IGeminiRequestContext requestContext, int count, int startIndex, string nameFilter, bool privateOnly)
		{
			return SearchTemplateFind.CreateSearchTemplateFind(requestContext, count, startIndex, nameFilter, privateOnly);
		}

		public void DeleteSearchTemplate(IGeminiRequestContext requestContext, Guid id)
		{
			SearchTemplateItem.DeleteSearchTemplate(requestContext, id);
		}

		public ISearchTemplateItem[] FindSearchTemplates(IGeminiRequestContext requestContext, Guid id)
		{
			return SearchTemplateFind.FindSearchTemplates(requestContext, id);
		}

		public ISearchTemplateItem[] FindSearchTemplates(IGeminiRequestContext requestContext, int count, int startIndex, string keywordFilter, bool privateOnly)
		{
			return SearchTemplateFind.FindSearchTemplates(requestContext, count, startIndex, keywordFilter, privateOnly);
		}

		public ISearchTemplateItem QuerySearchTemplate(IGeminiRequestContext requestContext, Guid id)
		{
			return SearchTemplateItem.QuerySearchTemplate(requestContext, id);
		}

		public ISearchTemplateFind QuerySearchTemplateFind(IGeminiRequestContext requestContext, Guid id)
		{
			return SearchTemplateFind.QuerySearchTemplateFind(requestContext, id);
		}

		public ISearchTemplateFind[] QuerySearchTemplateFinds(IGeminiRequestContext requestContext)
		{
			return SearchTemplateFind.QuerySearchTemplateFinds(requestContext);
		}

		public ISearchTemplateItem UpdateSearchTemplate(IGeminiRequestContext requestContext, Guid id, string name, string description, string searchQuery, SearchTemplateAvailability availability, bool includesScopeItems)
		{
			return SearchTemplateItem.UpdateSearchTemplate(requestContext, id, name, description, searchQuery, availability, includesScopeItems);
		}

		public ISearchTemplateFind UpdateSearchTemplateFind(IGeminiRequestContext requestContext, Guid id, int count, int startIndex, string nameFilter, bool privateOnly)
		{
			return SearchTemplateFind.UpdateSearchTemplateFind(requestContext, id, count, startIndex, nameFilter, privateOnly);
		}
	}

	protected ISearchTemplateRepository Repository { get; }

	protected SearchTemplateConfigurationApiBase(ISearchTemplateRepository repository)
	{
		Repository = repository ?? new DefaultRepository();
	}

	public abstract ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path);

	public abstract ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method);

	public abstract ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path);

	public abstract ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath);

	public abstract ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath);
}
