using System;
using VideoOS.Management.Common;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.BusinessObjects.ManagementServer.SearchTemplate;

public sealed class SearchTemplateFind : ISearchTemplateFind
{
	private static readonly Guid _defaultId = new Guid("15FDE99A-A90F-4C58-9160-DDC6233A8A64");

	private const int _defaultCount = 1000;

	private const int _defaultStartIndex = 0;

	public Guid Id { get; set; }

	public int Count { get; set; }

	public int StartIndex { get; set; }

	public string KeywordFilter { get; set; }

	public bool PrivateOnly { get; set; }

	public static ISearchTemplateFind Default { get; } = new SearchTemplateFind
	{
		Id = _defaultId,
		Count = 1000,
		StartIndex = 0,
		KeywordFilter = string.Empty,
		PrivateOnly = false
	};

	public static ISearchTemplateFind[] QuerySearchTemplateFinds(IGeminiRequestContext requestContext)
	{
		requestContext.NullCheck("", "QuerySearchTemplateFinds", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\BusinessObjects\\ManagementServer\\SearchTemplate\\SearchTemplateFind.cs", 28);
		using ISearchTemplateComponent searchTemplateComponent = requestContext.CreateSearchTemplateDataComponent();
		return searchTemplateComponent.QuerySearchTemplateFinds(requestContext.AuthenticatedUserSid);
	}

	public static ISearchTemplateFind QuerySearchTemplateFind(IGeminiRequestContext requestContext, Guid id)
	{
		requestContext.NullCheck("", "QuerySearchTemplateFind", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\BusinessObjects\\ManagementServer\\SearchTemplate\\SearchTemplateFind.cs", 37);
		using ISearchTemplateComponent searchTemplateComponent = requestContext.CreateSearchTemplateDataComponent();
		return searchTemplateComponent.QuerySearchTemplateFind(requestContext.AuthenticatedUserSid, id);
	}

	public static ISearchTemplateItem[] FindSearchTemplates(IGeminiRequestContext requestContext, Guid id)
	{
		requestContext.NullCheck("", "FindSearchTemplates", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\BusinessObjects\\ManagementServer\\SearchTemplate\\SearchTemplateFind.cs", 46);
		using ISearchTemplateComponent searchTemplateComponent = requestContext.CreateSearchTemplateDataComponent();
		if (!searchTemplateComponent.QuerySearchTemplateFind(requestContext.AuthenticatedUserSid, id).PrivateOnly)
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.SaveSearch, "READ_PUBLIC_SEARCH", "/");
		}
		return searchTemplateComponent.FindSearchTemplates(requestContext.AuthenticatedUserSid, id);
	}

	public static ISearchTemplateItem[] FindSearchTemplates(IGeminiRequestContext requestContext, int count, int startIndex, string keywordFilter, bool privateOnly)
	{
		requestContext.NullCheck("", "FindSearchTemplates", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\BusinessObjects\\ManagementServer\\SearchTemplate\\SearchTemplateFind.cs", 61);
		if (!privateOnly)
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.SaveSearch, "READ_PUBLIC_SEARCH", "/");
		}
		using ISearchTemplateComponent searchTemplateComponent = requestContext.CreateSearchTemplateDataComponent();
		return searchTemplateComponent.FindSearchTemplates(requestContext.AuthenticatedUserSid, count, startIndex, keywordFilter, privateOnly);
	}

	public static ISearchTemplateFind CreateSearchTemplateFind(IGeminiRequestContext requestContext, int count, int startIndex, string keywordFilter, bool privateOnly)
	{
		requestContext.NullCheck("", "CreateSearchTemplateFind", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\BusinessObjects\\ManagementServer\\SearchTemplate\\SearchTemplateFind.cs", 74);
		if (!privateOnly)
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.SaveSearch, "READ_PUBLIC_SEARCH", "/");
		}
		using ISearchTemplateComponent searchTemplateComponent = requestContext.CreateSearchTemplateDataComponent();
		return searchTemplateComponent.CreateSearchTemplateFind(requestContext.AuthenticatedUserSid, count, startIndex, keywordFilter, privateOnly);
	}

	public static ISearchTemplateFind UpdateSearchTemplateFind(IGeminiRequestContext requestContext, Guid id, int count, int startIndex, string keywordFilter, bool privateOnly)
	{
		requestContext.NullCheck("", "UpdateSearchTemplateFind", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\BusinessObjects\\ManagementServer\\SearchTemplate\\SearchTemplateFind.cs", 87);
		if (!privateOnly)
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.SaveSearch, "READ_PUBLIC_SEARCH", "/");
		}
		using ISearchTemplateComponent searchTemplateComponent = requestContext.CreateSearchTemplateDataComponent();
		return searchTemplateComponent.UpdateSearchTemplateFind(requestContext.AuthenticatedUserSid, id, count, startIndex, keywordFilter, privateOnly);
	}
}
