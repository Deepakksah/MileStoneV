using System;
using System.Linq;
using System.Windows;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public static class ViewLocatorExtensions
{
	public static void RegisterOnViewLocator(this FrameworkElement uiElement, string viewLocatorResourceName, Type viewModelType, Type viewType, string context = null)
	{
		ViewLocator viewLocator = (ViewLocator)uiElement.TryFindResource(viewLocatorResourceName);
		if (viewLocator == null)
		{
			throw new InvalidOperationException("Could not find view locator.");
		}
		if (!viewLocator.ViewLocatorEntries.Any((ViewLocatorEntry vle) => vle.ViewModelType == viewModelType && vle.ViewType == viewType && vle.Context == context))
		{
			viewLocator.ViewLocatorEntries.Add(new ViewLocatorEntry
			{
				ViewModelType = viewModelType,
				ViewType = viewType,
				Context = context
			});
		}
	}
}
