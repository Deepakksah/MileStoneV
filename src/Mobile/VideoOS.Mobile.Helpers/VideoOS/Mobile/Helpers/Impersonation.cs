using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;

namespace VideoOS.Mobile.Helpers;

public class Impersonation : IDisposable, IImpersonation
{
	private bool _disposed;

	private string _username;

	private string _domain;

	private SecureString _password;

	private LogonType _logonType;

	private WindowsImpersonationContext _impersonationContext;

	public string Username => _username;

	public string Domain => _domain;

	public LogonType LogonType => _logonType;

	public Impersonation(WellKnownSidType wellKnownSid, LogonType logonType = LogonType.Interactive)
	{
		Tuple<string, string> tuple = ExtractUsernameAndDomain(AccountHelper.GetSystemAccountName(wellKnownSid));
		Init(tuple.Item1, tuple.Item2, null, logonType);
	}

	public Impersonation(string username, string domain, SecureString password, LogonType logonType = LogonType.Interactive)
	{
		WellKnownSidType wellKnownSidType = AccountHelper.MapCommonAccountNameToSid(username);
		if (wellKnownSidType != WellKnownSidType.NullSid)
		{
			Tuple<string, string> tuple = ExtractUsernameAndDomain(AccountHelper.GetSystemAccountName(wellKnownSidType));
			Init(tuple.Item1, tuple.Item2, null, logonType);
		}
		else if (string.IsNullOrEmpty(domain))
		{
			Tuple<string, string> tuple2 = ExtractUsernameAndDomain(username);
			Init(tuple2.Item1, tuple2.Item2, null, logonType);
		}
		else
		{
			Init(username, domain, password, logonType);
		}
	}

	public void Impersonate()
	{
		IntPtr phToken = IntPtr.Zero;
		IntPtr duplication = IntPtr.Zero;
		if (!NativeMethods.LogonUser(_username, _domain, (_password == null) ? "" : _password.ToString(), (int)_logonType, 0, ref phToken))
		{
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}
		try
		{
			if (NativeMethods.DuplicateToken(phToken, 2, ref duplication))
			{
				WindowsIdentity windowsIdentity = new WindowsIdentity(duplication);
				_impersonationContext = windowsIdentity.Impersonate();
				return;
			}
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}
		finally
		{
			if (!duplication.Equals(IntPtr.Zero))
			{
				NativeMethods.CloseHandle(duplication);
				duplication = IntPtr.Zero;
			}
			if (!phToken.Equals(IntPtr.Zero))
			{
				NativeMethods.CloseHandle(phToken);
				phToken = IntPtr.Zero;
			}
		}
	}

	public void Revert()
	{
		if (_impersonationContext != null)
		{
			_impersonationContext.Undo();
			_impersonationContext.Dispose();
			_impersonationContext = null;
		}
	}

	private void Init(string username, string domain, SecureString password, LogonType logonType)
	{
		if (string.IsNullOrEmpty(username))
		{
			throw new ArgumentException("Username must not be empty!");
		}
		if (string.IsNullOrEmpty(domain))
		{
			throw new ArgumentException("Domain must not be empty!");
		}
		_username = username;
		_domain = domain;
		_password = password;
		_logonType = logonType;
	}

	private static Tuple<string, string> ExtractUsernameAndDomain(string usernameAndDomain)
	{
		string[] array = usernameAndDomain.Split('\\');
		if (array.Length != 2)
		{
			throw new ArgumentException("Unable to find domain");
		}
		return Tuple.Create(array[1], array[0]);
	}

	~Impersonation()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			Revert();
			_disposed = true;
		}
	}
}
