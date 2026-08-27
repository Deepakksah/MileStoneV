using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using VideoOS.Platform.UI.Controls.DropDowns.Internal;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls;

public class VideoOSDropDownBorderlessContentForegroundExtension : MarkupExtension
{
	private static IMultiValueConverter ContentForegroundBrushConverter { get; } = new LambdaConverter3<Brush, object, DependencyObject>((Brush contentForegroundBrush, object resourceKey, DependencyObject self) => contentForegroundBrush ?? self.FindResource(resourceKey), "ContentForegroundBrushConverter");

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget provideValueTarget)
		{
			if (provideValueTarget.TargetObject.GetType().FullName == "System.Windows.SharedDp")
			{
				return this;
			}
			if (provideValueTarget.TargetObject is DependencyObject target && provideValueTarget.TargetProperty is DependencyProperty dp)
			{
				BindingBase bindingBase = CreateResourceBinding();
				BindingOperations.SetBinding(target, dp, bindingBase);
				return bindingBase;
			}
		}
		return null;
	}

	public static BindingBase CreateResourceBinding()
	{
		return new MultiBinding
		{
			Bindings = 
			{
				(BindingBase)new Binding
				{
					Path = new PropertyPath(VideoOSDropDownAttachedProperties.ContentForegroundProperty),
					RelativeSource = new RelativeSource(RelativeSourceMode.Self)
				},
				(BindingBase)new Binding(".")
				{
					Source = "VideoOSResourceKeysInternalForegroundBrush"
				},
				(BindingBase)new Binding(".")
				{
					RelativeSource = new RelativeSource(RelativeSourceMode.Self)
				},
				(BindingBase)new Binding("SelectedTheme")
				{
					Source = ThemeManager.Instance
				},
				(BindingBase)new Binding("IsVisible")
				{
					RelativeSource = new RelativeSource(RelativeSourceMode.Self)
				}
			},
			Converter = ContentForegroundBrushConverter
		};
	}
}
