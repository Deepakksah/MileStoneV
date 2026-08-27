using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace VideoOS.Platform.UI.Controls.TreeView.Internal;

public class CustomTreeViewItem : TreeViewItem
{
	public static readonly DependencyProperty IconSourceProperty = DependencyProperty.Register("IconSource", typeof(VideoOSIconSourceBase), typeof(CustomTreeViewItem), new PropertyMetadata(null));

	public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(CustomTreeViewItem), new PropertyMetadata(null));

	public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register("Actions", typeof(ICollection<VideoOSActionBase>), typeof(CustomTreeViewItem), new PropertyMetadata((object)null));

	public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(CustomTreeViewItem), new PropertyMetadata((object)null));

	public static readonly RoutedEvent CustomRequestBringIntoViewEvent = EventManager.RegisterRoutedEvent("CustomRequestBringIntoView", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CustomTreeViewItem));

	public VideoOSIconSourceBase IconSource
	{
		get
		{
			return (VideoOSIconSourceBase)GetValue(IconSourceProperty);
		}
		set
		{
			SetValue(IconSourceProperty, value);
		}
	}

	public object Data
	{
		get
		{
			return GetValue(DataProperty);
		}
		set
		{
			SetValue(DataProperty, value);
		}
	}

	public ICollection<VideoOSActionBase> Actions
	{
		get
		{
			return (ICollection<VideoOSActionBase>)GetValue(ActionsProperty);
		}
		set
		{
			SetValue(ActionsProperty, value);
		}
	}

	public DataTemplate ContentTemplate
	{
		get
		{
			return (DataTemplate)GetValue(ContentTemplateProperty);
		}
		set
		{
			SetValue(ContentTemplateProperty, value);
		}
	}

	public VideoOSTreeView VideoOSTreeView { get; }

	public static void AddCustomRequestBringIntoViewHandler(DependencyObject dependencyObject, RoutedEventHandler handler)
	{
		if (dependencyObject is UIElement uIElement)
		{
			uIElement.AddHandler(CustomRequestBringIntoViewEvent, handler);
		}
	}

	public static void RemoveCustomRequestBringIntoViewHandler(DependencyObject dependencyObject, RoutedEventHandler handler)
	{
		if (dependencyObject is UIElement uIElement)
		{
			uIElement.RemoveHandler(CustomRequestBringIntoViewEvent, handler);
		}
	}

	public CustomTreeViewItem(VideoOSTreeView videoOsTreeView)
	{
		VideoOSTreeView = videoOsTreeView;
		base.RequestBringIntoView += VideoOSTreeView_OnRequestBringIntoView;
	}

	private void VideoOSTreeView_OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
	{
		e.Handled = true;
		RaiseEvent(new RoutedEventArgs(CustomRequestBringIntoViewEvent, this));
	}

	private void VideoOSTreeView_OnItemPropertyNameChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		DependencyProperty dependencyProperty = null;
		if (e.Property == VideoOSTreeView.ItemIsExpandedPropertyNameProperty)
		{
			dependencyProperty = TreeViewItem.IsExpandedProperty;
		}
		if (e.Property == VideoOSTreeView.ItemIsEnabledPropertyNameProperty)
		{
			dependencyProperty = UIElement.IsEnabledProperty;
		}
		if (e.Property == VideoOSTreeView.ItemIsSelectedPropertyNameProperty)
		{
			dependencyProperty = TreeViewItem.IsSelectedProperty;
		}
		if (e.Property == VideoOSTreeView.ItemIconSourcePropertyNameProperty)
		{
			dependencyProperty = IconSourceProperty;
		}
		if (e.Property == VideoOSTreeView.ItemDataPropertyNameProperty)
		{
			dependencyProperty = DataProperty;
		}
		if (e.Property == VideoOSTreeView.ItemChildrenPropertyNameProperty)
		{
			dependencyProperty = ItemsControl.ItemsSourceProperty;
		}
		if (e.Property == VideoOSTreeView.ItemActionsPropertyNameProperty)
		{
			dependencyProperty = ActionsProperty;
		}
		if (e.Property == VideoOSTreeView.ContentTemplateProperty)
		{
			dependencyProperty = ContentTemplateProperty;
		}
		if (dependencyProperty != null)
		{
			SetupBindings(dependencyProperty);
		}
	}

	private void SetupBinding(DependencyProperty dependencyProperty, string propertyName, BindingMode mode = BindingMode.OneWay, object fallbackValue = null)
	{
		if (!string.IsNullOrWhiteSpace(propertyName))
		{
			Binding binding = new Binding(propertyName)
			{
				Mode = mode
			};
			if (fallbackValue != null)
			{
				binding.FallbackValue = fallbackValue;
			}
			BindingOperations.SetBinding(this, dependencyProperty, binding);
		}
		else if (fallbackValue != null)
		{
			SetCurrentValue(dependencyProperty, fallbackValue);
		}
		else
		{
			BindingOperations.ClearBinding(this, dependencyProperty);
		}
	}

	public void SetupListeners()
	{
		VideoOSTreeView.ItemPropertyNameChanged += VideoOSTreeView_OnItemPropertyNameChanged;
	}

	public void RemoveListeners()
	{
		VideoOSTreeView.ItemPropertyNameChanged += VideoOSTreeView_OnItemPropertyNameChanged;
	}

	private string GetPropertyName(string videoOSTreeViewItemPropertyName, string customPropertyName, string customPropertyNameFallBack = null)
	{
		string text;
		if (!(base.DataContext is VideoOSTreeViewItem))
		{
			text = customPropertyName;
			if (text == null)
			{
				return customPropertyNameFallBack;
			}
		}
		else
		{
			text = videoOSTreeViewItemPropertyName;
		}
		return text;
	}

	public void SetupBindings(params DependencyProperty[] properties)
	{
		string propertyName = GetPropertyName("IsExpanded", VideoOSTreeView.ItemIsExpandedPropertyName);
		string propertyName2 = GetPropertyName("IsEnabled", VideoOSTreeView.ItemIsEnabledPropertyName);
		string propertyName3 = GetPropertyName("IsSelected", VideoOSTreeView.ItemIsSelectedPropertyName);
		string propertyName4 = GetPropertyName("IconSource", VideoOSTreeView.ItemIconSourcePropertyName);
		string propertyName5 = GetPropertyName("Data", VideoOSTreeView.ItemDataPropertyName, ".");
		string propertyName6 = GetPropertyName("Children", VideoOSTreeView.ItemChildrenPropertyName);
		string propertyName7 = GetPropertyName("Actions", VideoOSTreeView.ItemActionsPropertyName);
		string propertyName8 = GetPropertyName("ContentTemplate", VideoOSTreeView.ItemContentTemplatePropertyName);
		if (properties == null || properties.Length == 0)
		{
			SetupBinding(TreeViewItem.IsExpandedProperty, propertyName, BindingMode.TwoWay);
			SetupBinding(UIElement.IsEnabledProperty, propertyName2, BindingMode.TwoWay);
			SetupBinding(TreeViewItem.IsSelectedProperty, propertyName3, BindingMode.TwoWay);
			SetupBinding(IconSourceProperty, propertyName4);
			SetupBinding(DataProperty, propertyName5, BindingMode.OneWay, base.DataContext?.ToString());
			SetupBinding(ItemsControl.ItemsSourceProperty, propertyName6);
			SetupBinding(ActionsProperty, propertyName7);
			SetupBinding(ContentTemplateProperty, propertyName8, BindingMode.OneWay, VideoOSTreeView.ContentTemplate);
			return;
		}
		foreach (DependencyProperty dependencyProperty in properties)
		{
			if (dependencyProperty == TreeViewItem.IsExpandedProperty)
			{
				SetupBinding(TreeViewItem.IsExpandedProperty, propertyName, BindingMode.TwoWay);
			}
			else if (dependencyProperty == UIElement.IsEnabledProperty)
			{
				SetupBinding(UIElement.IsEnabledProperty, propertyName2, BindingMode.TwoWay);
			}
			else if (dependencyProperty == TreeViewItem.IsSelectedProperty)
			{
				SetupBinding(TreeViewItem.IsSelectedProperty, propertyName3, BindingMode.TwoWay);
			}
			else if (dependencyProperty == IconSourceProperty)
			{
				SetupBinding(IconSourceProperty, propertyName4);
			}
			else if (dependencyProperty == DataProperty)
			{
				SetupBinding(DataProperty, propertyName5, BindingMode.OneWay, base.DataContext?.ToString());
			}
			else if (dependencyProperty == ItemsControl.ItemsSourceProperty)
			{
				SetupBinding(ItemsControl.ItemsSourceProperty, propertyName6);
			}
			else if (dependencyProperty == ActionsProperty)
			{
				SetupBinding(ActionsProperty, propertyName7);
			}
			else if (dependencyProperty == ContentTemplateProperty)
			{
				SetupBinding(ContentTemplateProperty, propertyName8, BindingMode.OneWay, VideoOSTreeView.ContentTemplate);
			}
		}
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
