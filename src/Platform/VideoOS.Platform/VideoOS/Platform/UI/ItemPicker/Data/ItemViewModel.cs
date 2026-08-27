using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VideoOS.Platform.UI.Controls;
using VideoOS.Platform.UI.ItemPicker.Utilities;
using VideoOS.Platform.UI.ItemPicker.ViewModel;

namespace VideoOS.Platform.UI.ItemPicker.Data;

public class ItemViewModel : ViewModelBase
{
	private readonly IconSource _iconSource;

	private VideoOSActionActivate _deleteAction;

	private Item _internalItem;

	private ObservableCollection<ItemViewModel> _childItems;

	private bool _autoExpand;

	private bool _isSelected;

	public bool IsEnabled { get; } = true;

	public Item InternalItem
	{
		get
		{
			return _internalItem;
		}
		set
		{
			_internalItem = value;
			OnPropertyChanged("ImageSource");
		}
	}

	public ObservableCollection<ItemViewModel> ChildItems
	{
		get
		{
			if (_childItems == null)
			{
				_childItems = new ObservableCollection<ItemViewModel>();
			}
			return _childItems;
		}
		set
		{
			_childItems = new ObservableCollection<ItemViewModel>(value);
			OnPropertyChanged("ChildItems");
			OnPropertyChanged("HasChildren");
		}
	}

	public bool HasChildren
	{
		get
		{
			if (_childItems != null)
			{
				return _childItems.Count > 0;
			}
			return false;
		}
	}

	public bool AutoExpand
	{
		get
		{
			return _autoExpand;
		}
		set
		{
			_autoExpand = value;
			OnPropertyChanged("AutoExpand");
		}
	}

	public VideoOSIconSourceBase IconSource => _iconSource.GetIconSource(InternalItem);

	public string Name
	{
		get
		{
			if (InternalItem == null)
			{
				return "";
			}
			return InternalItem.Name;
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
			if (!IsFolder() || (ParentViewModel != null && ParentViewModel.AllowGroupSelection))
			{
				_isSelected = value;
				OnPropertyChanged("IsSelected");
			}
		}
	}

	public List<VideoOSActionBase> Actions { get; }

	public ItemPickerViewModel ParentViewModel { get; public set; }

	public bool Prune { get; set; }

	public ItemViewModel(string removeTooltip)
	{
		_iconSource = new IconSource();
		_deleteAction = new VideoOSActionActivate
		{
			IconSource = new VideoOSIconBuiltInSource
			{
				Icon = VideoOSIconBuiltInSource.Icons.Search_Clear_Field
			},
			Appearance = VideoOSActionActivate.Appearances.ButtonTransparent,
			ToolTip = removeTooltip
		};
		_deleteAction.Activated += DeleteAction_Activated;
		Actions = new List<VideoOSActionBase> { _deleteAction };
	}

	private void DeleteAction_Activated(object sender, EventArgs e)
	{
		if (ParentViewModel != null)
		{
			ParentViewModel.RemoveSelectedItem(this);
			ParentViewModel = null;
		}
	}

	public override string ToString()
	{
		return Name;
	}

	public bool IsKindValid(List<Guid> validItemKinds)
	{
		return validItemKinds.Any((Guid x) => x == InternalItem.FQID.Kind);
	}

	public ItemViewModel DeepCopy()
	{
		ItemViewModel itemViewModel = new ItemViewModel(_deleteAction.ToolTip);
		itemViewModel.InternalItem = InternalItem;
		itemViewModel.AutoExpand = AutoExpand;
		foreach (ItemViewModel childItem in ChildItems)
		{
			itemViewModel.ChildItems.Add(childItem.DeepCopy());
		}
		itemViewModel.Prune = Prune;
		return itemViewModel;
	}

	public bool IsFolder()
	{
		Item internalItem = InternalItem;
		if (internalItem == null)
		{
			return true;
		}
		return internalItem.FQID?.FolderType != FolderType.No;
	}
}
