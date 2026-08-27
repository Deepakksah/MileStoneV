using System.Runtime.InteropServices;
using System.Text;

namespace VideoOS.UI.Common.WPF.IO;

public class NativeMethods
{
	[DllImport("mpr.dll", CharSet = CharSet.Unicode, SetLastError = true, ThrowOnUnmappableChar = true)]
	public static extern int WNetGetConnection(string localName, StringBuilder remoteName, ref int length);
}
