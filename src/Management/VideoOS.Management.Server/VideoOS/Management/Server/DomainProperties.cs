using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.Security.Authentication;
using System.Security.Principal;
using System.Text;

namespace VideoOS.Management.Server;

public class DomainProperties : IDisposable
{
	private string _domainName;

	private string _domainSid;

	private string _dnsName;

	private DirectoryEntry _domainDirectoryEntry;

	private DirectoryEntry _directoryEntry;

	private bool _disposed;

	private static object _domainPropertiesLock = new object();

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

	public DomainProperties(string domainSid)
	{
		DomainController domainController = null;
		using Domain domain = Domain.GetComputerDomain();
		domainController = ResolveLocalDomain(domain, domainSid);
		if (domainController == null)
		{
			domainController = ResolveLocalForest(domain, domainSid);
		}
		if (domainController == null)
		{
			domainController = ResolveTrustedDomains(domain, domainSid);
		}
		if (domainController == null)
		{
			GeminiTrace.Warning(string.Format(CultureInfo.InvariantCulture, "Error finding domain controller for domain {0}.", domainSid));
			throw new InvalidDomainException();
		}
		_dnsName = domainController.Name;
		if (domainController.Domain != null)
		{
			_domainDirectoryEntry = domainController.Domain.GetDirectoryEntry();
		}
		if (_domainDirectoryEntry == null)
		{
			throw new InvalidDomainException();
		}
		_domainSid = domainSid;
		ResolveDomainName();
		AssignControllerEntry();
		_disposed = false;
		domainController.Dispose();
	}

	private void ResolveDomainName()
	{
		byte[] sid = _domainDirectoryEntry.Properties["objectSID"].Value as byte[];
		string domainName = null;
		NativeMethods.SidToName(sid, out domainName, out var _);
		_domainName = domainName.ToUpperInvariant();
	}

	private void AssignControllerEntry()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("LDAP://");
		stringBuilder.Append(_dnsName);
		_directoryEntry = new DirectoryEntry(stringBuilder.ToString());
		_directoryEntry.RefreshCache();
	}

	private static DomainController ResolveLocalDomain(Domain computerDomain, string targetDomainSid)
	{
		SecurityIdentifier securityIdentifier = null;
		using (DirectoryEntry directoryEntry = computerDomain.GetDirectoryEntry())
		{
			securityIdentifier = new SecurityIdentifier(directoryEntry.Properties["objectSID"].Value as byte[], 0);
		}
		if (targetDomainSid.Equals(securityIdentifier.Value, StringComparison.OrdinalIgnoreCase))
		{
			try
			{
				return computerDomain.FindDomainController();
			}
			catch (ActiveDirectoryObjectNotFoundException)
			{
			}
			catch (ActiveDirectoryOperationException)
			{
			}
		}
		GeminiTrace.Verbose(string.Format(CultureInfo.InvariantCulture, "No DC found for matching domain sid in local domain: {0}", targetDomainSid));
		return null;
	}

	private static DomainController ResolveLocalForest(Domain computerDomain, string targetDomainSid)
	{
		DomainCollection domainCollection = null;
		try
		{
			domainCollection = computerDomain.Forest.Domains;
		}
		catch (AuthenticationException)
		{
		}
		catch (ActiveDirectoryOperationException)
		{
		}
		catch (ActiveDirectoryServerDownException)
		{
		}
		catch (ActiveDirectoryObjectNotFoundException)
		{
		}
		if (domainCollection != null)
		{
			foreach (Domain item in domainCollection)
			{
				try
				{
					using DirectoryEntry directoryEntry = item.GetDirectoryEntry();
					SecurityIdentifier securityIdentifier = new SecurityIdentifier(directoryEntry.Properties["objectSID"].Value as byte[], 0);
					if (targetDomainSid.Equals(securityIdentifier.Value, StringComparison.OrdinalIgnoreCase))
					{
						return item.FindDomainController();
					}
				}
				catch (ActiveDirectoryOperationException)
				{
				}
				catch (ActiveDirectoryServerDownException)
				{
				}
				catch (ActiveDirectoryObjectNotFoundException)
				{
				}
			}
		}
		GeminiTrace.Verbose(string.Format(CultureInfo.InvariantCulture, "No DC found for matching domain sid in local forest: {0}", targetDomainSid));
		return null;
	}

	private static DomainController ResolveTrustedDomains(Domain localDomain, string targetDomainSid)
	{
		string text = null;
		text = ResolveTargetDnsNameFromDomainTrusts(localDomain, targetDomainSid);
		if (string.IsNullOrEmpty(text))
		{
			text = ResolveTargetDnsNameFromForestTrusts(localDomain, targetDomainSid);
		}
		if (!string.IsNullOrEmpty(text))
		{
			try
			{
				using Domain domain = Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, text));
				return domain.FindDomainController();
			}
			catch (AuthenticationException)
			{
			}
			catch (ActiveDirectoryObjectNotFoundException)
			{
			}
			catch (PlatformNotSupportedException)
			{
			}
			catch (ActiveDirectoryOperationException)
			{
			}
			catch (ActiveDirectoryServerDownException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
		}
		GeminiTrace.Warning(string.Format(CultureInfo.InvariantCulture, "Error finding external domain controller for domain {0}.", targetDomainSid));
		return null;
	}

	private static string ResolveTargetDnsNameFromDomainTrusts(Domain localDomain, string targetDomainSid)
	{
		TrustRelationshipInformationCollection trustRelationshipInformationCollection = null;
		try
		{
			trustRelationshipInformationCollection = localDomain.GetAllTrustRelationships();
			if (trustRelationshipInformationCollection != null && trustRelationshipInformationCollection.Count > 0)
			{
				return GetDomainDnsFromDomain(targetDomainSid, trustRelationshipInformationCollection);
			}
		}
		catch (AuthenticationException)
		{
		}
		catch (ActiveDirectoryOperationException)
		{
		}
		catch (ActiveDirectoryServerDownException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		GeminiTrace.Verbose(string.Format(CultureInfo.InvariantCulture, "No Domain with match sid found in local domain trusts: {0}", targetDomainSid));
		return null;
	}

	private static string ResolveTargetDnsNameFromForestTrusts(Domain localDomain, string targetDomainSid)
	{
		TrustRelationshipInformationCollection trustRelationshipInformationCollection = null;
		try
		{
			trustRelationshipInformationCollection = localDomain.Forest.GetAllTrustRelationships();
			if (trustRelationshipInformationCollection != null && trustRelationshipInformationCollection.Count > 0)
			{
				return GetDomainDnsFromForest(targetDomainSid, trustRelationshipInformationCollection);
			}
		}
		catch (AuthenticationException)
		{
		}
		catch (ActiveDirectoryOperationException)
		{
		}
		catch (ActiveDirectoryServerDownException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		GeminiTrace.Verbose(string.Format(CultureInfo.InvariantCulture, "No Domain with match sid found in local forest trusts: {0}", targetDomainSid));
		return null;
	}

	private static string GetDomainDnsFromDomain(string domainSid, TrustRelationshipInformationCollection allTrustRelationships)
	{
		foreach (object allTrustRelationship in allTrustRelationships)
		{
			TrustRelationshipInformation trustRelationshipInformation = allTrustRelationship as TrustRelationshipInformation;
			if (string.IsNullOrEmpty(trustRelationshipInformation?.TargetName))
			{
				continue;
			}
			try
			{
				using Domain domain = Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, trustRelationshipInformation.TargetName));
				if (GetDomainSid(domain).Equals(domainSid, StringComparison.OrdinalIgnoreCase))
				{
					return trustRelationshipInformation.TargetName;
				}
			}
			catch (AuthenticationException)
			{
			}
			catch (ActiveDirectoryObjectNotFoundException)
			{
			}
			catch (PlatformNotSupportedException)
			{
			}
			catch (ActiveDirectoryOperationException)
			{
			}
			catch (ActiveDirectoryServerDownException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			GeminiTrace.Warning(string.Format(CultureInfo.InvariantCulture, "Could not reach trusted domain '{0}'.", trustRelationshipInformation.TargetName));
		}
		return null;
	}

	private static string GetDomainDnsFromForest(string domainSid, TrustRelationshipInformationCollection allTrustRelationshipsForest)
	{
		foreach (object item in allTrustRelationshipsForest)
		{
			if (!(item is ForestTrustRelationshipInformation forestTrustRelationshipInformation))
			{
				continue;
			}
			ForestTrustDomainInfoCollection trustedDomainInformation;
			try
			{
				trustedDomainInformation = forestTrustRelationshipInformation.TrustedDomainInformation;
			}
			catch (ActiveDirectoryOperationException)
			{
				continue;
			}
			if (trustedDomainInformation == null)
			{
				continue;
			}
			foreach (object item2 in forestTrustRelationshipInformation.TrustedDomainInformation)
			{
				if (item2 is ForestTrustDomainInformation forestTrustDomainInformation && forestTrustDomainInformation.DomainSid.Equals(domainSid, StringComparison.OrdinalIgnoreCase))
				{
					return forestTrustDomainInformation.DnsName;
				}
			}
		}
		return null;
	}

	private static string GetDomainSid(Domain domain)
	{
		return new SecurityIdentifier(domain.GetDirectoryEntry().Properties["objectSID"].Value as byte[], 0).Value;
	}

	public static DomainProperties GetDomainProperties(string domainSid, bool forceReconnect)
	{
		if (!forceReconnect && _domainSids.TryGetValue(domainSid, out var value))
		{
			return value;
		}
		lock (_domainPropertiesLock)
		{
			if (forceReconnect || !_domainSids.TryGetValue(domainSid, out value))
			{
				value = new DomainProperties(domainSid);
				_domainSids[value.DomainSid] = value;
			}
			return value;
		}
	}

	public static DomainProperties GetDomainProperties(byte[] binarySid)
	{
		return GetDomainProperties(new SecurityIdentifier(binarySid, 0).Value, forceReconnect: false);
	}

	public static void ClearDomainProperties()
	{
		Dictionary<string, DomainProperties> domainSids;
		lock (_domainPropertiesLock)
		{
			domainSids = _domainSids;
			_domainSids = new Dictionary<string, DomainProperties>();
		}
		foreach (DomainProperties value in domainSids.Values)
		{
			value.Dispose();
		}
	}

	public static void ClearDomainProperties(string domainSid)
	{
		lock (_domainPropertiesLock)
		{
			if (_domainSids.TryGetValue(domainSid, out var value))
			{
				using (value)
				{
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
