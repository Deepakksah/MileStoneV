using System;

namespace VideoOS.Management.Common;

public static class CorporateProductCodes
{
	public static int GetProductCode(string productSku)
	{
		if (productSku == null)
		{
			throw new ArgumentNullException("productSku");
		}
		if (productSku.Equals("XPLL", StringComparison.OrdinalIgnoreCase))
		{
			return 421;
		}
		if (productSku.Equals("XPLA", StringComparison.OrdinalIgnoreCase))
		{
			return 422;
		}
		if (productSku.Equals("XPESPLUS", StringComparison.OrdinalIgnoreCase))
		{
			return 440;
		}
		if (productSku.Equals("XPEXPLUS", StringComparison.OrdinalIgnoreCase))
		{
			return 460;
		}
		if (productSku.Equals("XPPPLUS", StringComparison.OrdinalIgnoreCase))
		{
			return 480;
		}
		if (productSku.Equals("XPET", StringComparison.OrdinalIgnoreCase))
		{
			return 500;
		}
		if (productSku.Equals("XPCO", StringComparison.OrdinalIgnoreCase))
		{
			return 600;
		}
		return 0;
	}
}
