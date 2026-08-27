using System.Windows;
using System.Windows.Media;

namespace VideoOS.Platform.UI.Controls.DropDowns.Internal;

public static class VideoOSDropDownAttachedProperties
{
	public static readonly DependencyProperty ContentForegroundProperty = DependencyProperty.RegisterAttached("ContentForeground", typeof(Brush), typeof(VideoOSDropDownAttachedProperties), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

	public static void SetContentForeground(DependencyObject element, Brush value)
	{
		element.SetValue(ContentForegroundProperty, value);
	}

	public static Brush GetContentForeground(DependencyObject element)
	{
		return (Brush)element.GetValue(ContentForegroundProperty);
	}
}
