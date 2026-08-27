using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

public class AutomationIdConverter : MarkupExtension, IValueConverter
{
	public string AutomationIdPropertyName { get; set; } = "AutomationId";

	public string FallbackPropertyName { get; set; }

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value == null)
		{
			return DependencyProperty.UnsetValue;
		}
		object propertyValue = GetPropertyValue(value, AutomationIdPropertyName);
		if (propertyValue == null && FallbackPropertyName != null)
		{
			propertyValue = GetPropertyValue(value, FallbackPropertyName);
		}
		return propertyValue ?? DependencyProperty.UnsetValue;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

	private static object GetPropertyValue(object value, string propertyName)
	{
		PropertyInfo property = value.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
		if (!(property != null))
		{
			return null;
		}
		return property.GetValue(value);
	}
}
