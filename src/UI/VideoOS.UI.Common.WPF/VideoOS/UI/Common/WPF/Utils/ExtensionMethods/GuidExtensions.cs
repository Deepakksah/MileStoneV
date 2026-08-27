using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace VideoOS.UI.Common.WPF.Utils.ExtensionMethods;

public static class GuidExtensions
{
	public static string IdString(this Guid id)
	{
		return id.ToString("D", CultureInfo.InvariantCulture);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static Guid Xor(this Guid guid, Guid otherGuid)
	{
		long* ptr = (long*)(&guid);
		long* ptr2 = (long*)(&otherGuid);
		*ptr ^= *ptr2;
		ptr[1] ^= ptr2[1];
		return *(Guid*)ptr;
	}
}
