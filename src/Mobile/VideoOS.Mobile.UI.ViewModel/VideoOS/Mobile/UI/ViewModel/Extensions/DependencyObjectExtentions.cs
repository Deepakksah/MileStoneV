using System;
using System.Windows;
using System.Windows.Threading;

namespace VideoOS.Mobile.UI.ViewModel.Extensions;

public static class DependencyObjectExtentions
{
	public static void ForceRefresh(this DependencyObject objToRefresh)
	{
		objToRefresh.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (Action)delegate
		{
		});
	}
}
