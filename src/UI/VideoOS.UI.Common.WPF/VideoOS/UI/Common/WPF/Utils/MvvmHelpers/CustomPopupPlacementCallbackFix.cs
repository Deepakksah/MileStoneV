using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using Microsoft.Xaml.Behaviors;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class CustomPopupPlacementCallbackFix : Behavior<Popup>
{
	protected override void OnAttached()
	{
		((Behavior)this).OnAttached();
		Popup associatedObject = base.AssociatedObject;
		associatedObject.CustomPopupPlacementCallback = (CustomPopupPlacementCallback)Delegate.Combine(associatedObject.CustomPopupPlacementCallback, new CustomPopupPlacementCallback(CustomPopupPlacementCallback));
	}

	private CustomPopupPlacement[] CustomPopupPlacementCallback(Size popupSize, Size targetSize, Point offset)
	{
		return new CustomPopupPlacement[1]
		{
			new CustomPopupPlacement(default(Point), PopupPrimaryAxis.None)
		};
	}
}
