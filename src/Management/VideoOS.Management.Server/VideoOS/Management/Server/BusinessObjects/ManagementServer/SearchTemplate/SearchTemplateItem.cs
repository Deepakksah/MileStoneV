using System;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.BusinessObjects.ManagementServer.SearchTemplate;

public sealed class SearchTemplateItem : ISearchTemplateItem
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string SearchQuery { get; set; }

	public SearchTemplateAvailability Availability { get; set; }

	public bool IncludesScopeItems { get; set; }

	public static ISearchTemplateItem QuerySearchTemplate(IGeminiRequestContext requestContext, Guid id)
	{
		requestContext.NullCheck("", "QuerySearchTemplate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\BusinessObjects\\ManagementServer\\SearchTemplate\\SearchTemplateItem.cs", 25);
		using ISearchTemplateComponent searchTemplateComponent = requestContext.CreateSearchTemplateDataComponent();
		ISearchTemplateItem searchTemplateItem = searchTemplateComponent.QuerySearchTemplate(requestContext.AuthenticatedUserSid, id);
		if (searchTemplateItem.Availability != SearchTemplateAvailability.Private)
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.SaveSearch, "READ_PUBLIC_SEARCH", "/");
		}
		return searchTemplateItem;
	}

	public static ISearchTemplateItem CreateSearchTemplate(IGeminiRequestContext requestContext, Guid id, string name, string description, string searchQuery, SearchTemplateAvailability availability, bool includesScopeItems)
	{
		requestContext.NullCheck("", "CreateSearchTemplate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\BusinessObjects\\ManagementServer\\SearchTemplate\\SearchTemplateItem.cs", 39);
		if (availability != SearchTemplateAvailability.Private)
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.SaveSearch, "CREATE_PUBLIC_SEARCH", "/");
		}
		Guid sessionId = Guid.NewGuid();
		ISearchTemplateItem result;
		using (ISearchTemplateComponent searchTemplateComponent = requestContext.CreateSearchTemplateDataComponent())
		{
			result = searchTemplateComponent.CreateSearchTemplate(requestContext.AuthenticatedUserSid, id, name, description, searchQuery, availability, includesScopeItems, sessionId);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "SaveSearches", id.ToString(), EventAction.Added, name);
		return result;
	}

	public static ISearchTemplateItem UpdateSearchTemplate(IGeminiRequestContext requestContext, Guid id, string name, string description, string searchQuery, SearchTemplateAvailability availability, bool includesScopeItems)
	{
		requestContext.NullCheck("", "UpdateSearchTemplate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\BusinessObjects\\ManagementServer\\SearchTemplate\\SearchTemplateItem.cs", 62);
		if (availability != SearchTemplateAvailability.Private)
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.SaveSearch, "EDIT_PUBLIC_SEARCH", "/");
		}
		Guid sessionId = Guid.NewGuid();
		ISearchTemplateItem result;
		using (ISearchTemplateComponent searchTemplateComponent = requestContext.CreateSearchTemplateDataComponent())
		{
			result = searchTemplateComponent.UpdateSearchTemplate(requestContext.AuthenticatedUserSid, id, name, description, searchQuery, availability, includesScopeItems, sessionId);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "SaveSearches", id.ToString(), EventAction.Updated, name);
		return result;
	}

	public static void DeleteSearchTemplate(IGeminiRequestContext requestContext, Guid id)
	{
		requestContext.NullCheck("", "DeleteSearchTemplate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\BusinessObjects\\ManagementServer\\SearchTemplate\\SearchTemplateItem.cs", 85);
		Guid sessionId = Guid.NewGuid();
		using (ISearchTemplateComponent searchTemplateComponent = requestContext.CreateSearchTemplateDataComponent())
		{
			try
			{
				if (searchTemplateComponent.QuerySearchTemplate(requestContext.AuthenticatedUserSid, id).Availability != SearchTemplateAvailability.Private)
				{
					requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.SaveSearch, "DELETE_PUBLIC_SEARCH", "/");
				}
			}
			catch (SearchTemplateNotExistException)
			{
				throw new SearchTemplateDeleteException();
			}
			searchTemplateComponent.DeleteSearchTemplate(requestContext.AuthenticatedUserSid, id, sessionId);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "SaveSearches", id.ToString(), EventAction.Deleted, string.Empty);
	}
}
