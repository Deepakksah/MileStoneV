using System;
using System.Collections.Generic;

namespace VideoOS.Management.VmoClient;

public interface ISiteConnectionResult
{
	IVmoClient VmoClient { get; }

	Uri ServerAddress { get; }

	IEnumerable<SiteAddressWithError> ExceptionsFound { get; }

	SiteAddressWithError HighPriorityException { get; }

	bool IsSuccess { get; }

	bool IsFail { get; }

	string FormatExceptions();
}
