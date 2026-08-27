using System;
using System.Net;

namespace VideoOS.Platform.SDK.Util;

public interface ISiteUtil
{
	Item LoadSiteItem(bool secureOnly, Uri uri, CredentialCache credentialCache);

	Item LoadSiteItem(bool secureOnly, Uri uri, CredentialCache credentialCache, string serverType);

	Item LoadSiteItem(bool secureOnly, Uri uri);

	Item LoadSiteItem(bool secureOnly, Uri uri, UserContext userContext);
}
