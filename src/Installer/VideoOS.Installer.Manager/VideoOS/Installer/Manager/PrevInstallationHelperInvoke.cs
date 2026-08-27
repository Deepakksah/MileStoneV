using System.Runtime.InteropServices;
using System.Text;

namespace VideoOS.Installer.Manager;

public static class PrevInstallationHelperInvoke
{
	[DllImport("msi.dll", CharSet = CharSet.Unicode)]
	public static extern int MsiGetProductInfo(string product, string property, [Out] StringBuilder valueBuf, ref int len);
}
