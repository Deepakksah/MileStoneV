using System;
using System.Collections.Generic;
using System.Net.Http;

namespace VideoOS.Management.Server.RestApi;

public interface IRestApiRequestParameters
{
	Guid ServerId { get; set; }

	bool Disabled { get; set; }

	bool Resources { get; }

	bool Tasks { get; set; }

	bool NoData { get; }

	bool Count { get; }

	bool Definition { get; }

	string Task { get; }

	string Language { get; }

	List<KeyValuePair<string, string>> Parameters { get; }

	int PageStart { get; }

	int PageSize { get; }

	object SessionData { get; set; }

	FilterHandler FilterHandler { get; }

	HttpRequestMessage HttpRequest { get; }
}
