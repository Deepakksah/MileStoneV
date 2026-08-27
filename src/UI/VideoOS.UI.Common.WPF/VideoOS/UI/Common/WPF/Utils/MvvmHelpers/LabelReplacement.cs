using System.Windows;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class LabelReplacement : Freezable
{
	public string OriginalLabel { get; set; }

	public string ReplacedLabel { get; set; }

	protected override Freezable CreateInstanceCore()
	{
		return new LabelReplacement();
	}
}
