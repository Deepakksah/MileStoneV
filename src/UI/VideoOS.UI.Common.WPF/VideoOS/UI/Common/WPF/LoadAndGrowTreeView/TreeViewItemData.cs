using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using VideoOS.UI.Common.WPF.Resources;

namespace VideoOS.UI.Common.WPF.LoadAndGrowTreeView;

public class TreeViewItemData : DependencyObject, INotifyPropertyChanged
{
	private List<TreeViewItemData> _subItems = new List<TreeViewItemData>();

	private object _header;

	private object _identifier;

	private TreeViewItemData _parent;

	private readonly string _automationId;

	private bool _isSelected;

	private ContextMenu _contextMenu;

	public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(TreeViewItemData), new PropertyMetadata(false));

	public ContextMenu _groupModificationContextMenu = new ContextMenu();

	public List<TreeViewItemData> SubItems
	{
		get
		{
			return _subItems;
		}
		public set
		{
			_subItems = value;
			NotifyPropertyChanged("SubItems");
		}
	}

	public object Identifier => _identifier;

	public string AutomationId => _automationId;

	public object Header
	{
		get
		{
			return _header;
		}
		set
		{
			_header = value;
			NotifyPropertyChanged("Header");
		}
	}

	public TreeViewItemData Parent
	{
		get
		{
			return _parent;
		}
		set
		{
			_parent = value;
		}
	}

	public bool IsExpanded
	{
		get
		{
			return (bool)GetValue(IsExpandedProperty);
		}
		set
		{
			SetValue(IsExpandedProperty, value);
		}
	}

	public bool IsSelected
	{
		get
		{
			return _isSelected;
		}
		set
		{
			_isSelected = value;
			NotifyPropertyChanged("IsSelected");
		}
	}

	public ContextMenu ContextMenu
	{
		get
		{
			return _contextMenu;
		}
		set
		{
			_contextMenu = value;
			NotifyPropertyChanged("ContextMenu");
		}
	}

	public virtual bool IsChecked
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public virtual bool CanBeChecked
	{
		get
		{
			return false;
		}
		protected set
		{
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public TreeViewItemData(object identifier, object header, bool hasPossibleSubItems, TreeViewItemData parent, string automationId = null)
	{
		if (hasPossibleSubItems)
		{
			TreeViewItemData item = new TreeViewItemData(null, Strings.TreeViewItemLoading, hasPossibleSubItems: false, parent);
			_subItems.Add(item);
		}
		_identifier = identifier;
		_header = header;
		_parent = parent;
		_automationId = automationId ?? (_header as string);
	}

	public override string ToString()
	{
		return "";
	}

	protected void NotifyPropertyChanged(string propertyName)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
