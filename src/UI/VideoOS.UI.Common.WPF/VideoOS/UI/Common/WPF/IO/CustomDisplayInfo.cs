namespace VideoOS.UI.Common.WPF.IO;

public class CustomDisplayInfo : DisplayInfo
{
	public CustomDisplayInfo(string displayText)
	{
		base.DisplayText = displayText;
		base.RootDirectory = "";
		base.FullPath = "";
	}
}
