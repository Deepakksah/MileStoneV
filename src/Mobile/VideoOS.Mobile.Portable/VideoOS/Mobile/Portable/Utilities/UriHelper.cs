using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Mobile.Portable.Utilities;

public static class UriHelper
{
	public static readonly IEnumerable<Tuple<string, Func<string, bool>>> LocalAddressElements = new List<Tuple<string, Func<string, bool>>>
	{
		new Tuple<string, Func<string, bool>>("10.", null),
		new Tuple<string, Func<string, bool>>("172.", IsInRange16To31),
		new Tuple<string, Func<string, bool>>("192.168.", null),
		new Tuple<string, Func<string, bool>>("fc00", null),
		new Tuple<string, Func<string, bool>>("fe80", null),
		new Tuple<string, Func<string, bool>>("fec0", null)
	};

	public static bool StartsWith(string address, string start)
	{
		return address.ToLower().StartsWith(start);
	}

	public static bool IsInRange(string address, int start, int end)
	{
		try
		{
			string[] array = address.Split(new char[1] { '.' });
			if (array.Any())
			{
				int num = Convert.ToInt32(array[0]);
				return num >= start && num <= end;
			}
		}
		catch (Exception)
		{
		}
		return false;
	}

	public static bool IsInRange16To31(string address)
	{
		return IsInRange(address, 16, 31);
	}

	public static bool IsLocalAddress(Uri address)
	{
		return LocalAddressElements.Any((Tuple<string, Func<string, bool>> localAddressElement) => StartsWith(address.Host, localAddressElement.Item1) && (localAddressElement.Item2 == null || localAddressElement.Item2(address.Host.Substring(localAddressElement.Item1.Length))));
	}

	public static IEnumerable<Uri> RemoveLocalAddresses(IEnumerable<Uri> addresses)
	{
		return addresses.Where((Uri address) => !IsLocalAddress(address)).ToList();
	}

	public static IEnumerable<string> RemoveLocalAddresses(IEnumerable<string> addresses)
	{
		return from uri in RemoveLocalAddresses(addresses.Select((string address) => new Uri(address)))
			select uri.ToString();
	}

	public static Uri AddPath(this Uri uri, string path)
	{
		return new UriBuilder(uri)
		{
			Path = uri.AbsolutePath.ToString().TrimEnd("/\\".ToCharArray()) + "/" + path.Trim("/\\".ToCharArray())
		}.Uri;
	}
}
