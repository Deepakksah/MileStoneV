using System;
using System.Net;
using VideoOS.Platform.Login;

namespace VideoOS.Platform.SDK.Platform;

public class CredentialCacheService : ICredentialCacheService
{
	public static readonly string[] Urlkeys = new string[2] { "Address0", "Address1" };

	private readonly ISDKEnvironmentManager _environmentManager;

	public CredentialCacheService(ISDKEnvironmentManager environmentManager)
	{
		_environmentManager = environmentManager;
	}

	public Uri TryCreateParentUri(Item siteItem, CredentialCache credentialCache)
	{
		string[] urlkeys = Urlkeys;
		foreach (string key in urlkeys)
		{
			try
			{
				if (siteItem.Properties.TryGetValue(key, out var value))
				{
					Uri uri = CreateParentUri(credentialCache, value);
					if (uri != null)
					{
						return uri;
					}
				}
			}
			catch
			{
			}
		}
		return null;
	}

	public void MergeCredentialCache(Uri inUri, CredentialCache inCredentialCache, Uri outUri, CredentialCache outCredentialCache)
	{
		try
		{
			if (!AddCredential(inUri, outUri, "Negotiate", inCredentialCache, outCredentialCache))
			{
				AddCredential(inUri, outUri, "Basic", inCredentialCache, outCredentialCache);
			}
		}
		catch (Exception ex)
		{
			_environmentManager.Log(error: true, "MergeCredentialCache", "Unable to merge CredentialCache: " + ex.Message);
		}
	}

	public void Remove(CredentialCache cache, LoginSettings ls)
	{
		string authType = GetAuthType(ls);
		if (cache.GetCredential(ls.Uri, authType) != null)
		{
			cache.Remove(ls.Uri, authType);
		}
	}

	private static bool AddCredential(Uri inUri, Uri outUri, string authType, CredentialCache inCredentialCache, CredentialCache outCredentialCache)
	{
		NetworkCredential credential = inCredentialCache.GetCredential(inUri, authType);
		if (credential == null)
		{
			return false;
		}
		if (outCredentialCache.GetCredential(outUri, authType) == null)
		{
			outCredentialCache.Add(outUri, authType, credential);
		}
		return true;
	}

	private static string GetAuthType(LoginSettings ls)
	{
		if (!ls.IsBasicUser)
		{
			return "Negotiate";
		}
		return "Basic";
	}

	private static Uri CreateParentUri(CredentialCache credentialCache, string url)
	{
		return CreateParentUri(credentialCache, url, "Negotiate") ?? CreateParentUri(credentialCache, url, "Basic");
	}

	private static Uri CreateParentUri(CredentialCache credentialCache, string url, string authType)
	{
		Uri uri = new Uri(url);
		if (credentialCache.GetCredential(uri, authType) == null)
		{
			return null;
		}
		return uri;
	}
}
