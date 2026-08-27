using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.IdentityServer.Common.Exceptions;

namespace VideoOS.Management.VmoClient;

public class SiteConnectorExceptionHandler : ISiteConnectorExceptionHandler
{
	public SiteAddressWithError GetHighPriorityError(IEnumerable<SiteAddressWithError> errorsFound, ServerVersion siteVersion)
	{
		if (errorsFound == null)
		{
			return null;
		}
		bool flag = siteVersion == null || siteVersion < ServerVersions.v21_1;
		SiteAddressWithError siteAddressWithError = errorsFound.FirstOrDefault((SiteAddressWithError se) => se.ExceptionFound is VmoUnauthorizedAccessException);
		if (siteAddressWithError == null)
		{
			siteAddressWithError = errorsFound.FirstOrDefault((SiteAddressWithError se) => se.ExceptionFound is VmoInsufficientPermissionsException);
		}
		if (siteAddressWithError == null)
		{
			siteAddressWithError = errorsFound.FirstOrDefault((SiteAddressWithError se) => se.ExceptionFound is UserNamePasswordException);
		}
		if (siteAddressWithError == null)
		{
			siteAddressWithError = errorsFound.FirstOrDefault((SiteAddressWithError se) => se.ExceptionFound is InvalidServerIdException);
		}
		if (siteAddressWithError == null)
		{
			siteAddressWithError = errorsFound.FirstOrDefault((SiteAddressWithError se) => se.ExceptionFound is UnauthorizedException);
		}
		if (siteAddressWithError == null)
		{
			siteAddressWithError = errorsFound.FirstOrDefault((SiteAddressWithError se) => se.ExceptionFound is ForbiddenException);
		}
		if (siteAddressWithError == null && !flag)
		{
			siteAddressWithError = errorsFound.FirstOrDefault((SiteAddressWithError se) => se.ExceptionFound is IdpOfflineException);
		}
		if (siteAddressWithError == null)
		{
			siteAddressWithError = errorsFound.FirstOrDefault((SiteAddressWithError se) => se.ExceptionFound is UnsecureConnectionNotAllowedException);
		}
		return siteAddressWithError;
	}

	public Exception GetHighPriorityException(IEnumerable<Exception> exceptions, ServerVersion siteVersion)
	{
		if (exceptions == null || exceptions.Count() == 0)
		{
			return null;
		}
		bool isLegacySite = siteVersion == null || siteVersion < ServerVersions.v21_1;
		return exceptions.FirstOrDefault((Exception e) => e is VmoUnauthorizedAccessException) ?? exceptions.FirstOrDefault((Exception e) => e is VmoInsufficientPermissionsException) ?? exceptions.FirstOrDefault((Exception e) => e is UserNamePasswordException) ?? exceptions.FirstOrDefault((Exception e) => e is InvalidServerIdException) ?? exceptions.FirstOrDefault((Exception e) => e is UnauthorizedException) ?? exceptions.FirstOrDefault((Exception e) => e is ForbiddenException) ?? exceptions.FirstOrDefault((Exception e) => !isLegacySite && e is IdpOfflineException) ?? exceptions.FirstOrDefault((Exception e) => e is UnsecureConnectionNotAllowedException) ?? exceptions.First();
	}
}
