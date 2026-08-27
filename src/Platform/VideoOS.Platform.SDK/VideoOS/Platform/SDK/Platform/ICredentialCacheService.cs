using System;
using System.Net;
using VideoOS.Platform.Login;

namespace VideoOS.Platform.SDK.Platform;

public interface ICredentialCacheService
{
	Uri TryCreateParentUri(Item siteItem, CredentialCache credentialCache);

	void MergeCredentialCache(Uri inUri, CredentialCache inCredentialCache, Uri outUri, CredentialCache outCredentialCache);

	void Remove(CredentialCache cache, LoginSettings ls);
}
