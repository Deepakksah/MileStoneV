using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace VideoOS.Utilities.Security;

public static class NativeMethods
{
	[DllImport("advapi32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "LookupAccountSidW", ExactSpelling = true, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool LookupAccountSid(string systemName, byte[] psid, StringBuilder accountName, ref uint nameLength, StringBuilder domainName, ref uint domainLength, out AccountType accountType);

	public static string SidToName(byte[] sid, out string domainName, out AccountType accountType)
	{
		uint nameLength = 512u;
		uint domainLength = 512u;
		StringBuilder stringBuilder = new StringBuilder((int)nameLength);
		StringBuilder stringBuilder2 = new StringBuilder((int)domainLength);
		if (!LookupAccountSid(null, sid, stringBuilder, ref nameLength, stringBuilder2, ref domainLength, out accountType))
		{
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}
		domainName = stringBuilder2.ToString();
		return stringBuilder.ToString();
	}
}
