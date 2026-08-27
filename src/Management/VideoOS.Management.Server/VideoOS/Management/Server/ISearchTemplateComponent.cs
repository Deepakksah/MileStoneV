using System;
using VideoOS.Management.Server.BusinessObjects.ManagementServer.SearchTemplate;

namespace VideoOS.Management.Server;

public interface ISearchTemplateComponent : IDisposable
{
	ISearchTemplateItem QuerySearchTemplate(string authenticatedUserSid, Guid id);

	ISearchTemplateItem CreateSearchTemplate(string authenticatedUserSid, Guid id, string name, string description, string searchQuery, SearchTemplateAvailability availability, bool includesScopeItems, Guid sessionId);

	ISearchTemplateItem UpdateSearchTemplate(string authenticatedUserSid, Guid id, string name, string description, string searchQuery, SearchTemplateAvailability availability, bool includesScopeItems, Guid sessionId);

	void DeleteSearchTemplate(string authenticatedUserSid, Guid id, Guid sessionId);

	ISearchTemplateFind[] QuerySearchTemplateFinds(string authenticatedUserSid);

	ISearchTemplateFind QuerySearchTemplateFind(string authenticatedUserSid, Guid id);

	ISearchTemplateItem[] FindSearchTemplates(string authenticatedUserSid, Guid findId);

	ISearchTemplateItem[] FindSearchTemplates(string authenticatedUserSid, int count, int startIndex, string keywordFilter, bool privateOnly);

	ISearchTemplateFind CreateSearchTemplateFind(string authenticatedUserSid, int count, int startIndex, string keywordFilter, bool privateOnly);

	ISearchTemplateFind UpdateSearchTemplateFind(string authenticatedUserSid, Guid id, int count, int startIndex, string keywordFilter, bool privateOnly);
}
