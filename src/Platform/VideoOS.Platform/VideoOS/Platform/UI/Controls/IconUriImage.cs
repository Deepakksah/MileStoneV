using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace VideoOS.Platform.UI.Controls;

public class IconUriImage : Image
{
	public static readonly DependencyProperty UriProperty = DependencyProperty.Register("Uri", typeof(Uri), typeof(IconUriImage), new PropertyMetadata(null, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
	{
		((IconUriImage)o).UriChanged();
	}));

	public Uri Uri
	{
		get
		{
			return (Uri)GetValue(UriProperty);
		}
		set
		{
			SetValue(UriProperty, value);
		}
	}

	private void UriChanged()
	{
		if (Uri != null)
		{
			base.Source = new BitmapImage(Uri);
		}
		else
		{
			base.Source = null;
		}
	}
}
