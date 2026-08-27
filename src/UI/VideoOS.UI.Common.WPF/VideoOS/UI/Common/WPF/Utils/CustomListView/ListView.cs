using System;
using System.Windows;
using System.Windows.Controls;

namespace VideoOS.UI.Common.WPF.Utils.CustomListView;

public class ListView : System.Windows.Controls.ListView
{
	public ListView()
	{
		base.Resources = new ResourceDictionary
		{
			Source = new Uri("pack://application:,,,/VideoOS.UI.Common.WPF;component/Utils/CustomListView/ListViewResource.xaml")
		};
	}
}
