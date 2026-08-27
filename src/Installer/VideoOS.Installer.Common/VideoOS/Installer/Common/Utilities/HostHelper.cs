using System;

namespace VideoOS.Installer.Common.Utilities;

public static class HostHelper
{
	public static bool ValidateAddress(string address, out Uri validatedAddress)
	{
		validatedAddress = null;
		if (string.IsNullOrEmpty(address))
		{
			return false;
		}
		if ((!address.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !address.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) || !Uri.TryCreate(address, UriKind.Absolute, out var result) || result == null)
		{
			return false;
		}
		validatedAddress = result;
		if (!string.IsNullOrEmpty(result.LocalPath))
		{
			return result.LocalPath == "/";
		}
		return true;
	}
}
