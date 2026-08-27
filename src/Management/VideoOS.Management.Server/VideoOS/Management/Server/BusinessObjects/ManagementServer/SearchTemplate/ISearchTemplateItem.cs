using System;

namespace VideoOS.Management.Server.BusinessObjects.ManagementServer.SearchTemplate;

public interface ISearchTemplateItem
{
	Guid Id { get; set; }

	string Name { get; set; }

	string Description { get; set; }

	string SearchQuery { get; set; }

	SearchTemplateAvailability Availability { get; set; }

	bool IncludesScopeItems { get; set; }
}
