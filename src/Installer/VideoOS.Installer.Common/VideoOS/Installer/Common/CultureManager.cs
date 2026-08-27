using System.Globalization;
using System.Threading;

namespace VideoOS.Installer.Common;

public class CultureManager
{
	private static CultureManager _instance = new CultureManager();

	public CultureInfo CurrentCulture => Thread.CurrentThread.CurrentUICulture;

	public static CultureManager Instance => _instance;

	public event CultureChangedEventHandler CultureChanged;

	public void Language_CultureChanged(object sender, CultureChangedEventArgs e)
	{
		if (this.CultureChanged != null)
		{
			this.CultureChanged(this, new CultureChangedEventArgs(e.Culture));
		}
	}
}
