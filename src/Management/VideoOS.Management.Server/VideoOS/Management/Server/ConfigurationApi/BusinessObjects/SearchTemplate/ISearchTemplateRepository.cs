using System;
using VideoOS.Management.Server.BusinessObjects.ManagementServer.SearchTemplate;

namespace VideoOS.Management.Server.ConfigurationApi.BusinessObjects.SearchTemplate;

public interface ISearchTemplateRepository
{
	bool IsLicensed(IGeminiRequestContext requestContext);

	ISearchTemplateItem QuerySearchTemplate(IGeminiRequestContext requestContext, Guid id);

	ISearchTemplateItem CreateSearchTemplate(IGeminiRequestContext requestContext, Guid id, string name, string description, string searchQuery, SearchTemplateAvailability availability, bool includesScopeItems);

	ISearchTemplateItem UpdateSearchTemplate(IGeminiRequestContext requestContext, Guid id, string name, string description, string searchQuery, SearchTemplateAvailability availability, bool includesScopeItems);

	void DeleteSearchTemplate(IGeminiRequestContext requestContext, Guid id);

	ISearchTemplateFind[] QuerySearchTemplateFinds(IGeminiRequestContext requestContext);

	ISearchTemplateFind QuerySearchTemplateFind(IGeminiRequestContext requestContext, Guid id);

	ISearchTemplateItem[] FindSearchTemplates(IGeminiRequestContext requestContext, Guid id);

	ISearchTemplateItem[] FindSearchTemplates(IGeminiRequestContext requestContext, int count, int startIndex, string keywordFilter, bool privateOnly);

	ISearchTemplateFind CreateSearchTemplateFind(IGeminiRequestContext requestContext, int count, int startIndex, string keywordFilter, bool privateOnly);

	ISearchTemplateFind UpdateSearchTemplateFind(IGeminiRequestContext requestContext, Guid id, int count, int startIndex, string keywordFilter, bool privateOnly);
}
