using System;
using System.Windows;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSDropDownBorderedBase : VideoOSDropDownBase
{
	public static class VideoOSDropDownBorderedBaseInternals
	{
		public static IMultiValueConverter BorderBrushConverter { get; } = new LambdaConverter6<DependencyObject, bool, bool, bool, bool, States>(delegate(DependencyObject self, bool isEnabled, bool isMouseOver, bool isOpen, bool isFocused, States state)
		{
			string resourceKey;
			switch (state)
			{
			case States.Normal:
				resourceKey = "VideoOSResourceKeysInternalVideoOSDropDownBorderNormalIdleBrush";
				if (!isEnabled)
				{
					resourceKey = "VideoOSResourceKeysInternalVideoOSDropDownBorderNormalDisabledBrush";
				}
				else if (isOpen || isFocused)
				{
					resourceKey = "VideoOSResourceKeysInternalVideoOSDropDownBorderNormalFocusedBrush";
				}
				else if (isMouseOver)
				{
					resourceKey = "VideoOSResourceKeysInternalVideoOSDropDownBorderNormalHoveredBrush";
				}
				break;
			case States.Error:
				resourceKey = "VideoOSResourceKeysInternalVideoOSDropDownBorderErrorIdleBrush";
				if (!isEnabled)
				{
					resourceKey = "VideoOSResourceKeysInternalVideoOSDropDownBorderErrorDisabledBrush";
				}
				else if (isOpen || isFocused)
				{
					resourceKey = "VideoOSResourceKeysInternalVideoOSDropDownBorderErrorFocusedBrush";
				}
				else if (isMouseOver)
				{
					resourceKey = "VideoOSResourceKeysInternalVideoOSDropDownBorderErrorHoveredBrush";
				}
				break;
			default:
				throw new ArgumentOutOfRangeException("state", state, null);
			}
			return self.FindResource(resourceKey);
		}, "BorderBrushConverter");
	}

	public enum States
	{
		Normal,
		Error
	}

	public static readonly DependencyProperty StateProperty = DependencyProperty.Register("State", typeof(States), typeof(VideoOSDropDownBase), new PropertyMetadata(States.Normal));

	public States State
	{
		get
		{
			return (States)GetValue(StateProperty);
		}
		set
		{
			SetValue(StateProperty, value);
		}
	}

	public VideoOSDropDownBorderedBase()
	{
	}
}
