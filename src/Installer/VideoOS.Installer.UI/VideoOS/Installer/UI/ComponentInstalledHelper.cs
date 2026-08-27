using System.Runtime.InteropServices;
using System.Text;
using VideoOS.Installer.Manager;

namespace VideoOS.Installer.UI;

public static class ComponentInstalledHelper
{
	public enum InstallationState
	{
		InstalledByOtherSource,
		InstalledByThisSource,
		NotInstalled
	}

	[DllImport("msi.dll", CharSet = CharSet.Unicode)]
	private static extern int MsiGetProductInfo(string product, string property, [Out] StringBuilder valueBuf, ref int len);

	public static InstallationState ComponentIsInstalled(string productCode, string fileName, ref string productName)
	{
		int len = 512;
		StringBuilder stringBuilder = new StringBuilder(len);
		if (MsiGetProductInfo(productCode, "ProductName", stringBuilder, ref len) == 0)
		{
			if (fileName != null)
			{
				int num;
				if (string.Compare(InstallerManager.Instance.ClMsiHandler.GetProductName(fileName), stringBuilder.ToString()) != 0)
				{
					num = 0;
					if (num == 0)
					{
						productName = stringBuilder.ToString();
					}
				}
				else
				{
					num = 1;
				}
				return (InstallationState)num;
			}
			productName = stringBuilder.ToString();
			return InstallationState.InstalledByOtherSource;
		}
		int num2 = 1024;
		StringBuilder pProductId = new StringBuilder(num2);
		StringBuilder stringBuilder2 = new StringBuilder(num2);
		InstallerManager.Instance.ClMsiHandler.FindProductAndUpgradeCode(fileName, pProductId, num2, stringBuilder2, num2);
		StringBuilder stringBuilder3 = InstallerManager.Instance.ClMsiHandler.EnumRelatedProducts(stringBuilder2, 0u);
		if (stringBuilder3 != null)
		{
			return ComponentIsInstalled(stringBuilder3.ToString(), null, ref productName);
		}
		return InstallationState.NotInstalled;
	}
}
