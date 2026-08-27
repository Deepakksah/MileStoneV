using System;
using System.Windows;
using System.Windows.Media;

namespace VideoOS.Platform.UI.Extensions;

public static class ResourceDictionaryExtensions
{
	public static void PopulateColorsFromSolidColorBrushes<TEnum>(this ResourceDictionary dict) where TEnum : struct
	{
		foreach (object value in Enum.GetValues(typeof(TEnum)))
		{
			if (dict.Contains(value) && dict[value] is SolidColorBrush solidColorBrush)
			{
				string text = value.ToString();
				if (text.EndsWith("Brush", StringComparison.Ordinal) && Enum.TryParse<TEnum>(text.Substring(0, text.Length - "Brush".Length) + "Color", ignoreCase: false, out var result) && !dict.Contains(result))
				{
					byte a = (byte)(solidColorBrush.Opacity * 255.0);
					dict[result] = Color.FromArgb(a, solidColorBrush.Color.R, solidColorBrush.Color.G, solidColorBrush.Color.B);
				}
			}
		}
	}
}
