using System;

namespace VideoOS.Utilities.Network;

public static class AddressValidation
{
	public static bool ValidateWebAddress(string address)
	{
		return ValidateWebAddress(address, allowHttps: true);
	}

	public static bool ValidateWebAddress(string address, bool allowHttps)
	{
		return ValidateWebAddress(address, allowHttps, allowHttp: true);
	}

	public static bool ValidateWebAddress(string address, bool allowHttps, bool allowHttp)
	{
		if (address == null)
		{
			throw new ArgumentNullException("address");
		}
		if (address.Length == 0)
		{
			throw new ArgumentException("address is an empty string", "address");
		}
		if (!Uri.TryCreate(address, UriKind.Absolute, out var result))
		{
			return false;
		}
		if (!(string.Equals(result.Scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase) && allowHttp) && !(string.Equals(result.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase) && allowHttps))
		{
			return false;
		}
		return true;
	}
}
