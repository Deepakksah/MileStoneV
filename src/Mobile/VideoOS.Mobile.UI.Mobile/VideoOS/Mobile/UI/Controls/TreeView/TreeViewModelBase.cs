using System;
using System.Collections.Generic;
using System.ComponentModel;
using Prism.Commands;
using Prism.Mvvm;
using VideoOS.Mobile.UI.ViewModel.MVVM;

namespace VideoOS.Mobile.UI.Controls.TreeView;

public class TreeViewModelBase<T> : ViewModelBase where T : ITreeViewModelItem
{
	private readonly TreeViewModelBase<T> _parent;

	private readonly T _item;

	private bool? _isChecked = false;

	public List<TreeViewModelBase<T>> Children { get; private set; }

	public bool IsInitiallySelected { get; private set; }

	public string Name => _item.DisplayName;

	public Guid Id => _item.Id;

	public T InnerObject => _item;

	public DelegateCommand<TreeViewModelBase<T>> SelectedChangedCommand { get; private set; }

	public bool? IsChecked
	{
		get
		{
			return _isChecked;
		}
		set
		{
			SetIsChecked(value, updateChildren: true, updateParent: true);
		}
	}

	public event EventHandler SelectedNodeChangedEvent;

	private void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
	{
		if (value == _isChecked)
		{
			return;
		}
		_isChecked = value;
		if (updateChildren && _isChecked.HasValue)
		{
			Children.ForEach(delegate(TreeViewModelBase<T> c)
			{
				c.SetIsChecked(_isChecked, updateChildren: true, updateParent: false);
			});
		}
		if (updateParent && _parent != null)
		{
			_parent.VerifyCheckState();
		}
		((BindableBase)this).OnPropertyChanged(new PropertyChangedEventArgs("IsChecked"));
	}

	private void VerifyCheckState()
	{
		bool? flag = null;
		for (int i = 0; i < Children.Count; i++)
		{
			bool? isChecked = Children[i].IsChecked;
			if (i == 0)
			{
				flag = isChecked;
			}
			else if (flag != isChecked)
			{
				flag = null;
				break;
			}
		}
		SetIsChecked(flag, updateChildren: false, updateParent: true);
	}

	protected virtual void OnSelectedNodeChangedEvent()
	{
		this.SelectedNodeChangedEvent?.Invoke(this, EventArgs.Empty);
	}

	private void SelectionChanged(TreeViewModelBase<T> treeModelItem)
	{
		OnSelectedNodeChangedEvent();
	}

	public TreeViewModelBase(TreeViewModelBase<T> parent, T item, bool isSelected = false)
	{
		_parent = parent;
		_item = item;
		IsInitiallySelected = isSelected;
		Children = new List<TreeViewModelBase<T>>();
		SelectedChangedCommand = (DelegateCommand<TreeViewModelBase<T>>)(object)new DelegateCommand<TreeViewModelBase<TreeViewModelBase<T>>>((Action<TreeViewModelBase<TreeViewModelBase<T>>>)new Action<TreeViewModelBase<T>>(SelectionChanged));
	}
}
