using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Markup;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.UI.Controls;
using VideoOS.Platform.UI.ItemPicker.ViewModel;
using VideoOS.Platform.UI.Resources;

namespace VideoOS.Platform.UI;

public partial class ItemPickerWpfWindow : VideoOSDialog, IDisposable, IComponentConnector
{
	private readonly ItemPickerWpfUserControl _view;

	private readonly ItemPickerWindowViewModel _windowVM;

	private Action _helpLinkAction;

	private bool _isDisposed;

	public IEnumerable<Item> Items
	{
		set
		{
			_windowVM.Items = value;
		}
	}

	public IEnumerable<Item> SelectedItems
	{
		get
		{
			return _windowVM.SelectedItems;
		}
		set
		{
			_windowVM.SelectedItems = value;
		}
	}

	public Func<Item, bool> IsValidSelectionCallback
	{
		set
		{
			_windowVM.IsValidSelectionCallback = value;
		}
	}

	public Func<Item, Item> ReplaceSelectionCallback
	{
		set
		{
			_windowVM.ReplaceSelectionCallback = value;
		}
	}

	public Func<Item, bool> IsVisibleCallback
	{
		set
		{
			_windowVM.IsVisibleCallback = value;
		}
	}

	public List<Guid> KindsFilter
	{
		get
		{
			return _view.KindsFilter;
		}
		set
		{
			_view.KindsFilter = value;
		}
	}

	public SelectionModeOptions SelectionMode
	{
		get
		{
			return _windowVM.SelectionMode;
		}
		set
		{
			_windowVM.SelectionMode = value;
			SetButtonVisibility(value != SelectionModeOptions.AutoCloseOnSelect);
			SetHelpLinkVisibility();
		}
	}

	public bool AllowGroupSelection
	{
		get
		{
			return _view.AllowGroupSelection;
		}
		set
		{
			_view.AllowGroupSelection = value;
		}
	}

	public bool AutoExpand
	{
		get
		{
			return _windowVM.AutoExpand;
		}
		set
		{
			_windowVM.AutoExpand = value;
		}
	}

	public bool SearchEnabled
	{
		get
		{
			return _windowVM.SearchEnabled;
		}
		set
		{
			_windowVM.SearchEnabled = value;
		}
	}

	public string AcceptButtonText
	{
		get
		{
			return OkBtn.Text;
		}
		set
		{
			OkBtn.Text = value;
		}
	}

	public string CancelButtonText
	{
		get
		{
			return CancelBtn.Text;
		}
		set
		{
			CancelBtn.Text = value;
		}
	}

	public string CustomErrorText
	{
		get
		{
			return _view.ErrorText;
		}
		set
		{
			_view.ErrorText = value;
		}
	}

	public string Description
	{
		get
		{
			return _windowVM.Description;
		}
		set
		{
			_windowVM.Description = value;
		}
	}

	public string Header
	{
		get
		{
			return _windowVM.Header;
		}
		set
		{
			_windowVM.Header = value;
		}
	}

	public string TableHeader
	{
		get
		{
			return _windowVM.TableHeader;
		}
		set
		{
			_windowVM.TableHeader = value;
		}
	}

	public string SearchPlaceholderText
	{
		get
		{
			return _windowVM.SearchPlaceholderText;
		}
		set
		{
			_windowVM.SearchPlaceholderText = value;
		}
	}

	public string RemoveActionTooltipText
	{
		get
		{
			return _windowVM.RemoveActionTooltip;
		}
		set
		{
			_windowVM.RemoveActionTooltip = value;
		}
	}

	public string EmptyListOverlayText
	{
		get
		{
			return _windowVM.EmptyListOverlayText;
		}
		set
		{
			_windowVM.EmptyListOverlayText = value;
		}
	}

	public ItemPickerWpfWindow()
	{
		_windowVM = new ItemPickerWindowViewModel();
		base.DataContext = _windowVM;
		_view = new ItemPickerWpfUserControl();
		_windowVM.Initialize((ItemPickerViewModel)_view.DataContext);
		InitializeComponent();
		ItemPickerContent.Content = _view;
		_windowVM.SelectedItemChanged += HandleSelectedItemChanged;
		OkBtn.Text = Strings.ItemPickerWindow_DefaultAcceptBtnContent;
		CancelBtn.Text = Strings.ItemPickerWindow_DefaultCancelBtnContent;
		new TrackPageViewData(typeof(ItemPickerWpfWindow)).Send();
	}

	public void EnableHelpLink(string linkText, Action linkAction)
	{
		HelpBtn.Text = linkText;
		_helpLinkAction = linkAction;
		SetHelpLinkVisibility();
	}

	private void HandleSelectedItemChanged(object sender, EventArgs args)
	{
		if (_windowVM.Close)
		{
			if (_windowVM.SelectedItems.Any())
			{
				base.DialogResult = true;
			}
			Close();
		}
		else if (_windowVM.SelectedItems.Any() && _windowVM.AutoCloseOnSelectModeReady && _windowVM.SelectionMode == SelectionModeOptions.AutoCloseOnSelect)
		{
			base.DialogResult = true;
			Close();
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_isDisposed && disposing)
		{
			_windowVM.SelectedItemChanged -= HandleSelectedItemChanged;
			_windowVM.Dispose();
			_view.Dispose();
			_isDisposed = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void SetButtonVisibility(bool visible)
	{
		OkBtn.IsVisible = visible;
		CancelBtn.IsVisible = visible;
	}

	private void SetHelpLinkVisibility()
	{
		HelpBtn.IsEnabled = !string.IsNullOrEmpty(HelpBtn.Text) && _helpLinkAction != null;
		HelpBtn.IsVisible = !string.IsNullOrEmpty(HelpBtn.Text) && _helpLinkAction != null && SelectionMode != SelectionModeOptions.AutoCloseOnSelect;
	}

	private void HelpButton_Click(object sender, EventArgs e)
	{
		_helpLinkAction();
	}

	private void AcceptButton_Click(object sender, EventArgs e)
	{
		_windowVM.OkCommand.Execute(null);
	}

	private void CancelButton_Click(object sender, EventArgs e)
	{
		_windowVM.CancelCommand.Execute(null);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "9.0.6.0")]
	public Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
