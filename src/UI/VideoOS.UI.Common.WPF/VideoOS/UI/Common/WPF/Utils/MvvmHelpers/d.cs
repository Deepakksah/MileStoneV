using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class d : FrameworkElement
{
	public static readonly DependencyProperty PropertiesProperty = DependencyProperty.RegisterAttached("Properties", typeof(d), typeof(d), new FrameworkPropertyMetadata(null, PropertiesChanged));

	public static readonly DependencyProperty IgnoreDesignTimePropertiesProperty = DependencyProperty.RegisterAttached("IgnoreDesignTimeProperties", typeof(bool), typeof(d), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

	private static bool? inDesignMode;

	private static bool InDesignMode
	{
		get
		{
			if (!inDesignMode.HasValue)
			{
				DependencyProperty isInDesignModeProperty = DesignerProperties.IsInDesignModeProperty;
				inDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(isInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;
				if (!(inDesignMode ?? false) && Process.GetCurrentProcess().ProcessName.StartsWith("devenv", StringComparison.Ordinal))
				{
					inDesignMode = true;
				}
			}
			return inDesignMode ?? false;
		}
	}

	public static void SetProperties(DependencyObject instance, d value)
	{
		instance.SetValue(PropertiesProperty, value);
	}

	public static d GetProperties(DependencyObject associatedObject)
	{
		return (d)associatedObject.GetValue(PropertiesProperty);
	}

	private static void PropertiesChanged(DependencyObject associatedObject, DependencyPropertyChangedEventArgs e)
	{
		if (!InDesignMode)
		{
			return;
		}
		FrameworkElement element = associatedObject as FrameworkElement;
		if (element == null)
		{
			return;
		}
		element.Loaded += delegate
		{
			if (!GetIgnoreDesignTimeProperties(associatedObject))
			{
				d properties = GetProperties(associatedObject);
				if (properties != null)
				{
					LocalValueEnumerator localValueEnumerator = properties.GetLocalValueEnumerator();
					while (localValueEnumerator.MoveNext())
					{
						associatedObject.SetValue(localValueEnumerator.Current.Property, localValueEnumerator.Current.Value);
					}
					IDictionaryEnumerator enumerator = properties.Resources.GetEnumerator();
					while (enumerator.MoveNext())
					{
						element.Resources[enumerator.Key] = enumerator.Value;
					}
					foreach (ResourceDictionary mergedDictionary in properties.Resources.MergedDictionaries)
					{
						element.Resources.MergedDictionaries.Add(mergedDictionary);
					}
				}
			}
		};
	}

	public static bool GetIgnoreDesignTimeProperties(DependencyObject dependencyObject)
	{
		return (bool)dependencyObject.GetValue(IgnoreDesignTimePropertiesProperty);
	}

	public static void SetIgnoreDesignTimeProperties(DependencyObject dependencyObject, bool value)
	{
		dependencyObject.SetValue(IgnoreDesignTimePropertiesProperty, value);
	}
}
