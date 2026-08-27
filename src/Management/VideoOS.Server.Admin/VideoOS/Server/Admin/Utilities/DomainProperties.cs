using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.Security.Principal;
using System.Text;

namespace VideoOS.Server.Admin.Utilities;

public class DomainProperties : IDisposable
{
	private string _domainName;

	private string _domainSid;

	private string _dnsName;

	private DirectoryEntry _domainDirectoryEntry;

	private DirectoryEntry _directoryEntry;

	private bool _disposed;

	private static object _domainPropertiesLock = new object();

	private static Dictionary<string, DomainProperties> _domainNames = new Dictionary<string, DomainProperties>();

	private static Dictionary<string, DomainProperties> _domainSids = new Dictionary<string, DomainProperties>();

	public string DomainName
	{
		get
		{
			CheckDisposed();
			return _domainName;
		}
	}

	public string DomainSid
	{
		get
		{
			CheckDisposed();
			return _domainSid;
		}
	}

	public string DnsName
	{
		get
		{
			CheckDisposed();
			return _dnsName;
		}
	}

	public DirectoryEntry DomainDirectoryEntry
	{
		get
		{
			CheckDisposed();
			return _domainDirectoryEntry;
		}
	}

	public DirectoryEntry DirectoryEntry
	{
		get
		{
			CheckDisposed();
			return new DirectoryEntry(_directoryEntry.Path);
		}
	}

	public DomainProperties(string domain)
	{
		_domainName = domain;
		DirectoryContext context = new DirectoryContext(DirectoryContextType.Domain, _domainName);
		DomainController domainController = null;
		try
		{
			domainController = DomainController.FindOne(context);
		}
		catch (Exception)
		{
		}
		if (domainController == null)
		{
			throw new ActiveDirectoryObjectNotFoundException(domain);
		}
		_dnsName = domainController.Name;
		if (domainController.Domain != null)
		{
			_domainDirectoryEntry = domainController.Domain.GetDirectoryEntry();
		}
		if (_domainDirectoryEntry == null)
		{
			throw new ActiveDirectoryObjectNotFoundException(domain);
		}
		domainController.Dispose();
		SecurityIdentifier securityIdentifier = new SecurityIdentifier(_domainDirectoryEntry.Properties["objectSid"].Value as byte[], 0);
		_domainSid = securityIdentifier.Value;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("LDAP://");
		stringBuilder.Append(_dnsName);
		_directoryEntry = new DirectoryEntry(stringBuilder.ToString());
		_directoryEntry.RefreshCache();
		_disposed = false;
	}

	public static DomainProperties GetDomainProperties(string domain)
	{
		if (_domainNames.TryGetValue(domain, out var value))
		{
			return value;
		}
		lock (_domainPropertiesLock)
		{
			if (!_domainNames.TryGetValue(domain, out value))
			{
				value = new DomainProperties(domain);
				_domainNames[value.DomainName] = value;
				_domainSids[value.DomainSid] = value;
			}
			return value;
		}
	}

	public static DomainProperties GetDomainProperties(byte[] binarySid)
	{
		string value = new SecurityIdentifier(binarySid, 0).Value;
		if (_domainSids.TryGetValue(value, out var value2))
		{
			return value2;
		}
		lock (_domainPropertiesLock)
		{
			if (!_domainNames.TryGetValue(value, out value2))
			{
				NativeMethods.SidToName(binarySid, out var domainName, out var _);
				value2 = new DomainProperties(domainName);
				_domainNames[value2.DomainName] = value2;
				_domainSids[value2.DomainSid] = value2;
			}
			return value2;
		}
	}

	public static void ClearDomainProperties()
	{
		Dictionary<string, DomainProperties> domainNames;
		lock (_domainPropertiesLock)
		{
			domainNames = _domainNames;
			_domainNames = new Dictionary<string, DomainProperties>();
			_domainSids = new Dictionary<string, DomainProperties>();
		}
		foreach (DomainProperties value in domainNames.Values)
		{
			value.Dispose();
		}
	}

	public static void ClearDomainProperties(string domain)
	{
		lock (_domainPropertiesLock)
		{
			if (_domainNames.TryGetValue(domain, out var value))
			{
				using (value)
				{
					_domainNames.Remove(value.DomainName);
					_domainSids.Remove(value.DomainSid);
					return;
				}
			}
		}
	}

	public string ADSIPathFromSID(byte[] binarySid)
	{
		CheckDisposed();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("LDAP://");
		stringBuilder.Append(_dnsName);
		stringBuilder.Append("/<SID=");
		for (int i = 0; i < binarySid.Length; i++)
		{
			stringBuilder.Append(binarySid[i].ToString("X2", CultureInfo.InvariantCulture));
		}
		stringBuilder.Append(">");
		return stringBuilder.ToString();
	}

	private void CheckDisposed()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("DomainProperties");
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (_domainDirectoryEntry != null)
			{
				_domainDirectoryEntry.Dispose();
				_domainDirectoryEntry = null;
			}
			if (_directoryEntry != null)
			{
				_directoryEntry.Dispose();
				_directoryEntry = null;
			}
		}
		_disposed = true;
	}
}
