using System;
using System.Globalization;
using System.Threading;
using VideoOS.Management.Common;
using VideoOS.Server.Properties;

namespace VideoOS.Server;

public sealed class VmoSecurityManager : IDisposable
{
	private static VmoSecurityManager _securityManagerInstance;

	private static object _securityManagerLock = new object();

	private Timer timer;

	public static VmoSecurityManager Instance
	{
		get
		{
			if (_securityManagerInstance == null)
			{
				lock (_securityManagerLock)
				{
					if (_securityManagerInstance == null)
					{
						_securityManagerInstance = new VmoSecurityManager();
					}
				}
			}
			return _securityManagerInstance;
		}
	}

	private VmoSecurityManager()
	{
		timer = new Timer(TimerCallback, null, TimeSpan.FromMinutes(5.0), TimeSpan.FromMinutes(10.0));
	}

	private void TimerCallback(object state)
	{
		IdentityCache.ClearMemoryCache();
	}

	public static void ParseIdentityName(string name, out string domain, out string accountName)
	{
		domain = null;
		accountName = null;
		int num = name.IndexOfAny(new char[2] { '\\', '/' });
		if (num < 0)
		{
			accountName = name;
			return;
		}
		domain = name.Substring(0, num).Trim();
		accountName = name.Substring(num + 1);
	}

	public void CheckAdministrator(string userSid)
	{
		if (!IsAdministrator(userSid))
		{
			string nameFromSid = GetNameFromSid(userSid);
			throw new UnauthorizedAccessException(string.Format(CultureInfo.CurrentCulture, Resources.UnauthorizedAccessException, nameFromSid));
		}
	}

	public void CheckMembership(string userSid)
	{
		if (!IsMember(userSid))
		{
			string nameFromSid = GetNameFromSid(userSid);
			throw new UnauthorizedAccessException(string.Format(CultureInfo.CurrentCulture, Resources.UnauthorizedAccessException, nameFromSid));
		}
	}

	public static bool IsMember(string userSid)
	{
		return AuthorizationAccessor.IsMember(userSid);
	}

	public static bool IsAdministrator(string userSid)
	{
		return AuthorizationAccessor.IsAdministrator(userSid);
	}

	private string GetNameFromSid(string sid)
	{
		try
		{
			Identity identity = IdentityCache.ReadIdentityFromCache(sid, QueryMembership.None);
			if (identity == null)
			{
				return sid;
			}
			return identity.DomainAccountName;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			return sid;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public void Dispose(bool disposing)
	{
		if (disposing && timer != null)
		{
			timer.Dispose();
			timer = null;
		}
	}
}
