using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class ViewLocator : System.Windows.Controls.DataTemplateSelector
{
	public static readonly DependencyProperty ContextProperty = DependencyProperty.RegisterAttached("Context", typeof(string), typeof(ViewLocator), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

	private Collection<ViewLocatorEntry> _viewLocatorEntries = new Collection<ViewLocatorEntry>();

	public Collection<ViewLocatorEntry> ViewLocatorEntries => _viewLocatorEntries;

	public static void SetContext(DependencyObject target, string value)
	{
		target.SetValue(ContextProperty, value);
	}

	public static string GetContext(DependencyObject target)
	{
		return (string)target.GetValue(ContextProperty);
	}

	public override DataTemplate SelectTemplate(object item, DependencyObject container)
	{
		if (item != null)
		{
			string text = ((DesignTimeDataTypeAttribute)item.GetType().GetCustomAttributes(typeof(DesignTimeDataTypeAttribute), inherit: false).FirstOrDefault())?.OriginalTypeFullName;
			string text2 = ((container != null) ? GetContext(container) : null);
			ViewLocatorEntry viewLocatorEntry = null;
			foreach (ViewLocatorEntry viewLocatorEntry2 in _viewLocatorEntries)
			{
				if (viewLocatorEntry2 != null && !(viewLocatorEntry2.ViewModelType == null) && ((text != null) ? (viewLocatorEntry2.ViewModelType.FullName == text) : viewLocatorEntry2.ViewModelType.IsAssignableFrom(item.GetType())))
				{
					if (text2 == viewLocatorEntry2.Context)
					{
						return BuildTemplate(container, viewLocatorEntry2);
					}
					if (viewLocatorEntry2.Context == null)
					{
						viewLocatorEntry = viewLocatorEntry2;
					}
				}
			}
			if (viewLocatorEntry != null)
			{
				return BuildTemplate(container, viewLocatorEntry);
			}
		}
		return base.SelectTemplate(item, container);
	}

	private static DataTemplate BuildTemplate(DependencyObject container, ViewLocatorEntry viewLocatorEntry)
	{
		Type type = viewLocatorEntry.ViewType;
		if (container != null && type.Assembly != container.GetType().Assembly)
		{
			Type type2 = container.GetType().Assembly.GetType(viewLocatorEntry.ViewType.FullName);
			if (type2 != null)
			{
				type = type2;
			}
		}
		DataTemplate dataTemplate = new DataTemplate();
		dataTemplate.VisualTree = new FrameworkElementFactory(type);
		dataTemplate.Seal();
		return dataTemplate;
	}
}
