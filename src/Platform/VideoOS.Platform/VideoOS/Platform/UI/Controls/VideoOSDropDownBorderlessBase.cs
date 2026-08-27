using System.Windows;
using System.Windows.Controls;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSDropDownBorderlessBase : VideoOSDropDownBase
{
	private Grid _textAndChevron;

	public override double? ContextMenuDesiredWidth => null;

	public override double? ContextMenuVerticalOffset => 2.0;

	public override UIElement ContextMenuPlacementTarget => _textAndChevron;

	public override void OnApplyTemplate()
	{
		_textAndChevron = this.GetTemplateChild<Grid>("PART_TextAndChevron");
	}

	public VideoOSDropDownBorderlessBase()
	{
	}
}
