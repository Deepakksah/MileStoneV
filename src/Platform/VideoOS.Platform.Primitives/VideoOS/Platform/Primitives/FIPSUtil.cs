using System.Runtime.InteropServices;

namespace VideoOS.Platform.Primitives;

public class FIPSUtil
{
	[return: MarshalAs(UnmanagedType.U1)]
	public static bool FIPS140Enabled()
	{
		return global::_003CModule_003E.NmFIPSUtil_002ECmFIPSUtil_002EFIPS140Enabled();
	}
}
