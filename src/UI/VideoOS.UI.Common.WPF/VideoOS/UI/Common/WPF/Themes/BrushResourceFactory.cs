using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace VideoOS.UI.Common.WPF.Themes;

public static class BrushResourceFactory
{
	public static ResourceDictionary CreateColorResourceDictionary(VideoOSBrushes brushes)
	{
		ResourceDictionary resourceDictionary = new ResourceDictionary
		{
			Source = new Uri("pack://application:,,,/VideoOS.UI.Common.WPF;component/Themes/GeneratedBrushes.xaml")
		};
		if (brushes != null)
		{
			AddBrushesToDictionary(brushes, resourceDictionary);
		}
		return resourceDictionary;
	}

	private static void AddBrushesToDictionary(VideoOSBrushes brushes, ResourceDictionary colorsDictionary)
	{
		foreach (PropertyInfo item in (IEnumerable<PropertyInfo>)brushes.GetType().GetProperties())
		{
			Brush brush = (Brush)item.GetValue(brushes);
			brush?.Freeze();
			colorsDictionary.Add(item.Name, brush);
		}
	}
}
