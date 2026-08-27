using System;

namespace VideoOS.Management.Server.BusinessObjects.ManagementServer.SearchTemplate;

public interface ISearchTemplateFind
{
	Guid Id { get; set; }

	int Count { get; set; }

	int StartIndex { get; set; }

	string KeywordFilter { get; set; }

	bool PrivateOnly { get; set; }
}
