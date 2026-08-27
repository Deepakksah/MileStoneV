using System.Windows.Controls.Primitives;

namespace VideoOS.Platform.UI.Utils;

public class CustomPopupVerticalPlacementResult
{
	public CustomPopupPlacement[] Placements { get; }

	public double? SuggestedMaxHeight { get; }

	public CustomPopupVerticalPlacementResult(CustomPopupPlacement[] placements, double? suggestedMaxHeight = null)
	{
		Placements = placements;
		SuggestedMaxHeight = suggestedMaxHeight;
	}
}
