using System.Windows;
using System.Windows.Media;

namespace VideoOS.Platform.UI.Extensions;

public static class VisualExtensionMethods
{
	private const double FallbackScaling = 1.0;

	public static double GetScreenScaleX(this Visual self)
	{
		if (self == null)
		{
			return 1.0;
		}
		return PresentationSource.FromVisual(self)?.CompositionTarget?.TransformToDevice.M11 ?? 1.0;
	}

	public static double GetScreenScaleY(this Visual self)
	{
		if (self == null)
		{
			return 1.0;
		}
		return PresentationSource.FromVisual(self)?.CompositionTarget?.TransformToDevice.M22 ?? 1.0;
	}
}
