using System.Windows.Media;

namespace VideoOS.UI.Common.WPF.IO;

public abstract class DisplayInfo
{
	public bool MightHaveChildren { get; set; }

	public string DisplayText { get; protected set; }

	public string RootDirectory { get; protected set; }

	public string FullPath { get; protected set; }

	public ImageSource IconCollapsed { get; set; }

	protected DisplayInfo()
	{
		MightHaveChildren = true;
	}
}
