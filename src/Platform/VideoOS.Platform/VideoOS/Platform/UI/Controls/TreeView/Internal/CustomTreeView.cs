using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls.TreeView.Internal;

public class CustomTreeView : System.Windows.Controls.TreeView
{
	public static readonly DependencyProperty VideoOSTreeViewProperty = DependencyProperty.Register("VideoOSTreeView", typeof(VideoOSTreeView), typeof(CustomTreeView), new PropertyMetadata((object)null));

	public static readonly DependencyProperty ScrollViewerViewportWidthProperty = DependencyProperty.Register("ScrollViewerViewportWidth", typeof(double), typeof(CustomTreeView), new PropertyMetadata(0.0));

	private ItemsPresenter _itemsPresenter;

	public VideoOSTreeView VideoOSTreeView
	{
		get
		{
			return (VideoOSTreeView)GetValue(VideoOSTreeViewProperty);
		}
		set
		{
			SetValue(VideoOSTreeViewProperty, value);
		}
	}

	public double ScrollViewerViewportWidth
	{
		get
		{
			return (double)GetValue(ScrollViewerViewportWidthProperty);
		}
		set
		{
			SetValue(ScrollViewerViewportWidthProperty, value);
		}
	}

	public double GetHorizontalOffsetFromContentContainer(FrameworkElement child)
	{
		return child.TranslatePoint(new Point(0.0, 0.0), _itemsPresenter).X;
	}

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();
		_itemsPresenter = this.GetTemplateChild<ItemsPresenter>("PART_ItemsPresenter");
		ScrollViewer source = this.FindDescendant<ScrollViewer>();
		Binding binding = new Binding("ViewportWidth")
		{
			Source = source
		};
		SetBinding(ScrollViewerViewportWidthProperty, binding);
	}

	protected override DependencyObject GetContainerForItemOverride()
	{
		return new CustomTreeViewItem(VideoOSTreeView);
	}

	protected override bool IsItemItsOwnContainerOverride(object item)
	{
		return item is CustomTreeViewItem;
	}

	protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
	{
		base.PrepareContainerForItemOverride(element, item);
		CustomTreeViewItem obj = (CustomTreeViewItem)element;
		obj.SetupListeners();
		obj.SetupBindings();
	}

	protected override void ClearContainerForItemOverride(DependencyObject element, object item)
	{
		base.ClearContainerForItemOverride(element, item);
		((CustomTreeViewItem)element).RemoveListeners();
	}
}
