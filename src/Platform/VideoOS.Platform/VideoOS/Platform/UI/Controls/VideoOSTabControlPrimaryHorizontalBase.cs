using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSTabControlPrimaryHorizontalBase : VideoOSTabControlPrimaryBase
{
	public enum ItemSizeModes
	{
		Nonuniform,
		Uniform
	}

	public static readonly DependencyProperty ItemSizeModeProperty = DependencyProperty.Register("ItemSizeMode", typeof(ItemSizeModes), typeof(VideoOSTabControlPrimaryHorizontalBase), new PropertyMetadata(ItemSizeModes.Nonuniform));

	public ItemSizeModes ItemSizeMode
	{
		get
		{
			return (ItemSizeModes)GetValue(ItemSizeModeProperty);
		}
		set
		{
			SetValue(ItemSizeModeProperty, value);
		}
	}

	public VideoOSTabControlPrimaryHorizontalBase()
	{
	}
}
