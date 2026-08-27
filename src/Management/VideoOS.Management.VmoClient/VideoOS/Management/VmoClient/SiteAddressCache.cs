using System;
using System.IO;
using System.Security;
using Microsoft.Win32;

namespace VideoOS.Management.VmoClient;

public class SiteAddressCache : ISiteAddressCache
{
	private const string RegistryKeyPath = "HKEY_CURRENT_USER\\Software\\VideoOS\\Sites\\ConnectionCache";

	public Uri GetSiteAddress(Guid siteId)
	{
		object obj = null;
		try
		{
			obj = Registry.GetValue("HKEY_CURRENT_USER\\Software\\VideoOS\\Sites\\ConnectionCache", siteId.ToString().ToUpperInvariant(), null);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (SecurityException)
		{
		}
		catch (IOException)
		{
		}
		catch (ArgumentException)
		{
		}
		if (obj == null)
		{
			return null;
		}
		string uriString = (string)obj;
		try
		{
			return new Uri(uriString);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (ArgumentNullException)
		{
			return null;
		}
		catch (UriFormatException)
		{
			return null;
		}
	}

	public void SetSiteAddress(Guid siteId, Uri address)
	{
		try
		{
			Registry.SetValue("HKEY_CURRENT_USER\\Software\\VideoOS\\Sites\\ConnectionCache", siteId.ToString().ToUpperInvariant(), address.OriginalString);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (ArgumentException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (SecurityException)
		{
		}
	}
}
