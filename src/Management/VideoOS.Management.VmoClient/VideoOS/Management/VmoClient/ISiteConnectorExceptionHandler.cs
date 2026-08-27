using System;
using System.Collections.Generic;

namespace VideoOS.Management.VmoClient;

public interface ISiteConnectorExceptionHandler
{
	SiteAddressWithError GetHighPriorityError(IEnumerable<SiteAddressWithError> errorsFound, ServerVersion siteVersion);

	Exception GetHighPriorityException(IEnumerable<Exception> exceptions, ServerVersion siteVersion);
}
