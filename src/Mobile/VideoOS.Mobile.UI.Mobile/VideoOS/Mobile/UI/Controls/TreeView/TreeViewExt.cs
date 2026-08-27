using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace VideoOS.Mobile.UI.Controls.TreeView;

public partial class TreeViewExt : UserControl, IComponentConnector
{
	public static readonly DependencyProperty InnerContentTemplateProperty = DependencyProperty.Register("InnerContentTemplate", typeof(DataTemplate), typeof(TreeViewExt));

	public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(TreeViewExt));

	public static DependencyProperty CheckedItemsChangedProperty = DependencyProperty.Register("CheckedItemsChanged", typeof(ICommand), typeof(TreeViewExt));

	public DataTemplate InnerContentTemplate
	{
		get
		{
			return (DataTemplate)GetValue(InnerContentTemplateProperty);
		}
		set
		{
			SetValue(InnerContentTemplateProperty, value);
		}
	}

	public IEnumerable ItemsSource
	{
		get
		{
			return (IEnumerable)GetValue(ItemsSourceProperty);
		}
		set
		{
			SetValue(ItemsSourceProperty, value);
		}
	}

	public ICommand CheckedItemsChanged
	{
		get
		{
			return (ICommand)GetValue(CheckedItemsChangedProperty);
		}
		set
		{
			SetValue(CheckedItemsChangedProperty, value);
		}
	}

	public TreeViewExt()
	{
		InitializeComponent();
	}
}
