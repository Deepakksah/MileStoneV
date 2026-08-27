using System.Windows;
using System.Windows.Controls;

namespace VideoOS.Platform.UI.Controls;

public class IconResourceImage : Image
{
	public static readonly DependencyProperty ResourceNameProperty = DependencyProperty.Register("ResourceName", typeof(string), typeof(IconResourceImage), new PropertyMetadata(null, delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
	{
		((IconResourceImage)o).ResourceNameChanged();
	}));

	public string ResourceName
	{
		get
		{
			return (string)GetValue(ResourceNameProperty);
		}
		set
		{
			SetValue(ResourceNameProperty, value);
		}
	}

	private void ResourceNameChanged()
	{
		if (!string.IsNullOrWhiteSpace(ResourceName))
		{
			SetResourceReference(Image.SourceProperty, ResourceName);
		}
		else
		{
			base.Source = null;
		}
	}
}
