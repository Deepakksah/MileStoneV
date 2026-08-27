using System;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.AccessControl.Internal.Providers.Strategies;

public class Sha1ProviderStrategy : IGuidProviderStrategy
{
	public Guid Create(Guid namespaceId, params string[] names)
	{
		if (names == null)
		{
			throw new ArgumentNullException("names");
		}
		return GuidUtil.Create(namespaceId, names);
	}

	public string GetIconKey(string externalIconKey)
	{
		if (externalIconKey == null)
		{
			throw new ArgumentNullException("externalIconKey");
		}
		return externalIconKey;
	}

	public override int GetHashCode()
	{
		return typeof(Sha1ProviderStrategy).GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (obj is Sha1ProviderStrategy sha1ProviderStrategy)
		{
			return sha1ProviderStrategy.GetType() == GetType();
		}
		return false;
	}
}
