using System.IO;

namespace VideoOS.UI.Common.WPF.IO;

public class DirectoryDisplayInfo : DisplayInfo
{
	public DirectoryInfo DirectoryInfo { get; private set; }

	public DirectoryDisplayInfo(DirectoryInfo directoryInfo, string displayName)
	{
		DirectoryInfo = directoryInfo;
		base.DisplayText = displayName;
		base.RootDirectory = directoryInfo.Root.Name;
		base.FullPath = directoryInfo.FullName;
	}
}
