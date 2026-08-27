using System;
using VideoOS.SqlUpdate.Core.Common;

namespace VideoOS.SqlUpdate.Core.Services;

public static class VmsDbTypeMapper
{
	public static VmsDbType Map(string product)
	{
		if (string.IsNullOrEmpty(product))
		{
			return VmsDbType.Unknown;
		}
		if (!Enum.TryParse<VmsDbType>(product, ignoreCase: true, out var result))
		{
			return VmsDbType.XPCO;
		}
		return result;
	}
}
