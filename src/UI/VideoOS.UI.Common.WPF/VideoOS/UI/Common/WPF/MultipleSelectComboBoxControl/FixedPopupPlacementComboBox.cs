using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace VideoOS.UI.Common.WPF.MultipleSelectComboBoxControl;

public class FixedPopupPlacementComboBox : ComboBox
{
	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		if (base.Template != null)
		{
			Popup popup = (Popup)base.Template.FindName("PART_Popup", this);
			if (popup != null)
			{
				popup.Placement = PlacementMode.Custom;
				popup.CustomPopupPlacementCallback = PlacePopup;
			}
		}
	}

	private CustomPopupPlacement[] PlacePopup(Size popupSize, Size targetSize, Point offset)
	{
		CustomPopupPlacement[] array = new CustomPopupPlacement[1];
		array[0].Point = new Point(0.0, targetSize.Height);
		return array;
	}
}
