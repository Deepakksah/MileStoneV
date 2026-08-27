using System;
using System.Security.Cryptography;
using System.Text;

namespace VideoOS.Platform.AccessControl.Internal.Providers.Strategies;

public class Md5ProviderStrategy : IGuidProviderStrategy
{
	private static MD5 _md5;

	private static readonly object LockObject = new object();

	private const string _iconSeparator = "_";

	public Guid Create(Guid namespaceId, params string[] names)
	{
		if (names == null)
		{
			throw new ArgumentNullException("names");
		}
		lock (LockObject)
		{
			_md5 = _md5 ?? MD5.Create();
		}
		string s = ((namespaceId != Guid.Empty) ? namespaceId.ToString() : string.Empty) + string.Join(string.Empty, names);
		return new Guid(_md5.ComputeHash(Encoding.Default.GetBytes(s)));
	}

	public string GetIconKey(string externalIconKey)
	{
		if (externalIconKey == null)
		{
			throw new ArgumentNullException("externalIconKey");
		}
		return "_" + externalIconKey;
	}

	public override int GetHashCode()
	{
		return typeof(Md5ProviderStrategy).GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (obj is Md5ProviderStrategy md5ProviderStrategy)
		{
			return md5ProviderStrategy.GetType() == GetType();
		}
		return false;
	}
}
