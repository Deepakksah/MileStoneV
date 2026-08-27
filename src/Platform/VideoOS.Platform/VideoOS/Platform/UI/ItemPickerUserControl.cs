using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Resources;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.UI;

[Obsolete("This class is deprecated. Windows Forms controls are no longer supported. Use ItemPickerWpfWindow instead.", false)]
public class ItemPickerUserControl : UserControl
{
	private List<Item> _itemsToSelectFrom = new List<Item>();

	private List<Item> _itemsToSelectFromServer = new List<Item>();

	private List<Item> _itemsToSelectFromGroup = new List<Item>();

	private Hashtable _selectedItems = new Hashtable();

	private Hashtable _categoryFilter = new Hashtable();

	private Hashtable _kindFilterProgram = new Hashtable();

	private Hashtable _kindFilterUser = new Hashtable();

	private Point _panelTreesOriginalLocation = new Point(0, 42);

	private bool _categoryUserSelectable = true;

	private bool _kindUserSelectable = true;

	private bool _singleSelect;

	private Point _labelCategoryLocation;

	private Point _comboBoxCategoryFilterLocation;

	private Point _labelKindLocation;

	private Point _comboBoxKindFilterLocation;

	private int _topPanelComboboxHeight = 42;

	private ImageList _modifiedImageList;

	private bool _buildingDropDowns;

	private IContainer components;

	private Panel panelTrees;

	private SplitContainer splitContainer1;

	private TabControl tabControl1;

	private TabPage tabPageGroup;

	private TabPage tabPageServer;

	private Button buttonRemove;

	private Button buttonAdd;

	private TreeView treeViewSelected;

	private TreeView treeViewGroup;

	private TreeView treeViewServer;

	private Label labelSelected;

	private Panel panelTop;

	private Label labelCategory;

	private Label labelKind;

	private ComboBox comboBoxCategoryFilter;

	private ComboBox comboBoxKind;

	public List<Item> ItemsToSelectFrom
	{
		set
		{
			_itemsToSelectFrom = value;
			treeViewGroup.Nodes.Clear();
			treeViewServer.Nodes.Clear();
			if (value == null)
			{
				return;
			}
			value.Sort((Item i1, Item i2) => Sort.NumericStringCompare(i1.Name, i2.Name));
			foreach (Item item in value)
			{
				try
				{
					if (IsItemVisible(item, topLevel: true, groupTreeViewActive: false))
					{
						TreeNode node = BuildTreeNode(item);
						treeViewServer.Nodes.Add(node);
					}
					if (IsItemVisible(item, topLevel: true, groupTreeViewActive: true))
					{
						TreeNode node2 = BuildTreeNode(item);
						treeViewGroup.Nodes.Add(node2);
					}
				}
				catch (Exception exception)
				{
					EnvironmentManager.Instance.ExceptionHandler("ItemPickerUserControll::ItemsToSelectFrom", exception);
				}
			}
			if (treeViewGroup.Nodes.Count == 0)
			{
				tabPageGroup.SendToBack();
			}
			else
			{
				tabPageServer.SendToBack();
			}
		}
	}

	public List<Item> ItemsToSelectFromGroup
	{
		set
		{
			_itemsToSelectFromGroup = value;
			treeViewGroup.Nodes.Clear();
			if (value == null)
			{
				return;
			}
			value.Sort((Item i1, Item i2) => Sort.NumericStringCompare(i1.Name, i2.Name));
			foreach (Item item in value)
			{
				try
				{
					if (IsItemVisible(item, topLevel: true, groupTreeViewActive: true))
					{
						TreeNode node = BuildTreeNode(item);
						treeViewGroup.Nodes.Add(node);
					}
				}
				catch (Exception exception)
				{
					EnvironmentManager.Instance.ExceptionHandler("ItemPickerUserControll::ItemsToSelectFrom", exception);
				}
			}
		}
	}

	public List<Item> ItemsToSelectFromServer
	{
		set
		{
			_itemsToSelectFromServer = value;
			treeViewServer.Nodes.Clear();
			if (value == null)
			{
				return;
			}
			value.Sort((Item i1, Item i2) => Sort.NumericStringCompare(i1.Name, i2.Name));
			foreach (Item item in value)
			{
				try
				{
					if (IsItemVisible(item, topLevel: true, groupTreeViewActive: false))
					{
						TreeNode node = BuildTreeNode(item);
						treeViewServer.Nodes.Add(node);
					}
				}
				catch (Exception exception)
				{
					EnvironmentManager.Instance.ExceptionHandler("ItemPickerUserControl::ItemsToSelectFromServer", exception);
				}
			}
		}
	}

	public List<Item> ItemsSelected
	{
		get
		{
			List<Item> list = new List<Item>();
			foreach (Item value in _selectedItems.Values)
			{
				list.Add(value);
			}
			return list;
		}
		set
		{
			_selectedItems = new Hashtable();
			treeViewSelected.Nodes.Clear();
			if (value != null)
			{
				foreach (Item item in value)
				{
					TreeNode node = BuildTreeNode(item);
					treeViewSelected.Nodes.Add(node);
					_selectedItems.Add(SelectedItemKey(item), item);
				}
			}
			FireSelectionChangedEvent();
		}
	}

	public List<Category> CategoryFilter
	{
		set
		{
			_categoryFilter = new Hashtable();
			if (value != null)
			{
				foreach (Category item in value)
				{
					_categoryFilter[item] = true;
				}
			}
			RebuildDropdowns();
		}
	}

	public bool CategoryUserSelectable
	{
		get
		{
			return _categoryUserSelectable;
		}
		set
		{
			_categoryUserSelectable = value;
			labelCategory.Visible = value;
			comboBoxCategoryFilter.Visible = value;
			AdjustHeaderLocation();
		}
	}

	public List<Guid> KindFilter
	{
		set
		{
			_kindFilterProgram = new Hashtable();
			_kindFilterUser = new Hashtable();
			if (value != null)
			{
				foreach (Guid item in value)
				{
					_kindFilterProgram[item] = true;
					_kindFilterUser[item] = true;
					ItemNode itemNode = PluginManager.GetItemNode(item);
					while (itemNode != null && itemNode.ParentKind != Guid.Empty)
					{
						itemNode = PluginManager.GetItemNode(itemNode.ParentKind);
						if (itemNode != null)
						{
							_kindFilterUser[itemNode.ItemKind] = true;
						}
					}
				}
			}
			RebuildDropdowns();
		}
	}

	public bool KindUserSelectable
	{
		get
		{
			return _kindUserSelectable;
		}
		set
		{
			_kindUserSelectable = value;
			labelKind.Visible = value;
			comboBoxKind.Visible = value;
			AdjustHeaderLocation();
		}
	}

	public Guid KindSelected
	{
		get
		{
			if (!(comboBoxKind.SelectedItem is TaggedItem taggedItem))
			{
				return Guid.Empty;
			}
			return taggedItem.Tag;
		}
		set
		{
			if (value != Guid.Empty)
			{
				foreach (object item in comboBoxKind.Items)
				{
					if (item is TaggedItem taggedItem && taggedItem.Tag == value)
					{
						comboBoxKind.SelectedItem = taggedItem;
						return;
					}
				}
			}
			if (comboBoxKind.Items.Count > 0)
			{
				comboBoxKind.SelectedIndex = 0;
			}
		}
	}

	public bool GroupTabVisible
	{
		get
		{
			return tabControl1.TabPages.Contains(tabPageGroup);
		}
		set
		{
			if (value)
			{
				if (!tabControl1.TabPages.Contains(tabPageGroup))
				{
					tabControl1.TabPages.Add(tabPageGroup);
				}
			}
			else if (tabControl1.TabPages.Contains(tabPageGroup))
			{
				tabControl1.TabPages.Remove(tabPageGroup);
			}
		}
	}

	public bool ServerTabVisible
	{
		get
		{
			return tabControl1.TabPages.Contains(tabPageServer);
		}
		set
		{
			if (value)
			{
				if (!tabControl1.TabPages.Contains(tabPageServer))
				{
					tabControl1.TabPages.Add(tabPageServer);
				}
			}
			else if (tabControl1.TabPages.Contains(tabPageServer))
			{
				tabControl1.TabPages.Remove(tabPageServer);
			}
		}
	}

	public bool ShowDisabledItems { get; set; }

	public bool SingleSelect
	{
		get
		{
			return _singleSelect;
		}
		set
		{
			_singleSelect = value;
			splitContainer1.Panel2Collapsed = value;
		}
	}

	public event EventHandler CloseDialog;

	public event ItemPickerForm.ValidateSelectionHandler ValidateSelectionEvent;

	public event ItemPickerForm.IsItemVisibleHandler IsItemVisibleEvent;

	public event EventHandler ItemsSelectedChangedEvent;

	public ItemPickerUserControl()
	{
		InitializeComponent();
		ShowDisabledItems = false;
		_labelCategoryLocation = labelCategory.Location;
		_labelKindLocation = labelKind.Location;
		_comboBoxCategoryFilterLocation = comboBoxCategoryFilter.Location;
		_comboBoxKindFilterLocation = comboBoxKind.Location;
		_modifiedImageList = Util.ImageListClone;
		treeViewGroup.ImageList = _modifiedImageList;
		treeViewServer.ImageList = _modifiedImageList;
		treeViewSelected.ImageList = _modifiedImageList;
	}

	public void Init()
	{
		RebuildDropdowns();
		new TrackPageViewData(typeof(ItemPickerUserControl), deprecated: true).Send();
	}

	private TreeNode BuildTreeNode(Item item)
	{
		return BuildTreeNode(item, addLoading: true);
	}

	private TreeNode BuildTreeNode(Item item, bool addLoading)
	{
		TreeNode treeNode = new TreeNode(item.Name)
		{
			Tag = item
		};
		Image image = item.IconWithOverlay();
		if (image != null)
		{
			int imageIndex = (treeNode.SelectedImageIndex = treeViewGroup.ImageList.Images.Count);
			treeNode.ImageIndex = imageIndex;
			_modifiedImageList.Images.Add(image);
		}
		else
		{
			int imageIndex = (treeNode.SelectedImageIndex = Util.BuiltInItemToImageIndex(item));
			treeNode.ImageIndex = imageIndex;
		}
		if (item.HasChildren != VideoOS.Platform.HasChildren.No && addLoading)
		{
			treeNode.Nodes.Add(Strings.Loading);
		}
		return treeNode;
	}

	private void AdjustHeaderLocation()
	{
		if (_categoryUserSelectable || _kindUserSelectable)
		{
			panelTop.Visible = true;
			panelTrees.Location = _panelTreesOriginalLocation;
			if (!_kindUserSelectable)
			{
				panelTop.Height = _topPanelComboboxHeight;
				labelCategory.Location = _labelKindLocation;
				comboBoxCategoryFilter.Location = _comboBoxKindFilterLocation;
			}
			if (!_categoryUserSelectable)
			{
				panelTop.Height = _topPanelComboboxHeight;
				labelKind.Location = _labelKindLocation;
				comboBoxKind.Location = _comboBoxKindFilterLocation;
			}
			else
			{
				labelCategory.Location = _labelCategoryLocation;
				comboBoxCategoryFilter.Location = _comboBoxCategoryFilterLocation;
			}
			panelTrees.Location = new Point(0, panelTop.Height);
			panelTrees.Size = new Size(base.Width, base.Height - panelTop.Height);
		}
		else
		{
			panelTop.Visible = false;
			panelTrees.Size = base.Size;
			panelTrees.Location = new Point(0, 0);
		}
	}

	private void RebuildDropdowns()
	{
		try
		{
			_buildingDropDowns = true;
			SuspendLayout();
			Dictionary<Category, bool> dictionary = new Dictionary<Category, bool>();
			if (comboBoxKind.Visible)
			{
				Guid kindSelected = KindSelected;
				comboBoxKind.Items.Clear();
				comboBoxKind.Items.Add(Strings.All);
				List<Guid> list = new List<Guid>();
				Guid[] array = new Guid[Util.KindToImageIndex.Keys.Count];
				Util.KindToImageIndex.Keys.CopyTo(array, 0);
				Guid[] array2 = array;
				foreach (Guid guid in array2)
				{
					if (_kindFilterProgram != null && _kindFilterProgram.Count > 0)
					{
						if (_kindFilterProgram.ContainsKey(guid))
						{
							list.Add(guid);
							if (Util.KindToCategory.ContainsKey(guid))
							{
								dictionary[Util.KindToCategory[guid]] = true;
							}
						}
					}
					else
					{
						list.Add(guid);
						if (Util.KindToCategory.ContainsKey(guid))
						{
							dictionary[Util.KindToCategory[guid]] = true;
						}
					}
				}
				foreach (Guid item in list)
				{
					string text = (string)Kind.DefaultTypeToNameTable[item];
					if (text == null)
					{
						ItemNode itemNode = PluginManager.GetItemNode(item);
						if (itemNode != null)
						{
							text = itemNode.Name;
						}
					}
					comboBoxKind.Items.Add(new TaggedItem(item, text));
				}
				KindSelected = kindSelected;
			}
			else
			{
				Guid[] array3 = new Guid[Util.KindToImageIndex.Keys.Count];
				Util.KindToImageIndex.Keys.CopyTo(array3, 0);
				Guid[] array2 = array3;
				foreach (Guid key in array2)
				{
					if (Util.KindToCategory.ContainsKey(key))
					{
						dictionary[Util.KindToCategory[key]] = true;
					}
				}
			}
			if (comboBoxCategoryFilter.Visible)
			{
				comboBoxCategoryFilter.Items.Clear();
				comboBoxCategoryFilter.Items.Add(Strings.All);
				foreach (Category key2 in dictionary.Keys)
				{
					comboBoxCategoryFilter.Items.Add(key2);
				}
				comboBoxCategoryFilter.SelectedIndex = 0;
			}
			ResumeLayout();
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionDialog("ItemPickerUserControl", exception);
		}
		_buildingDropDowns = false;
	}

	private void OnBeforeExpand(object sender, TreeViewCancelEventArgs e)
	{
		if (e.Node.Tag != null && e.Node.Tag is Item)
		{
			Item item = e.Node.Tag as Item;
			buttonRemove.Enabled = false;
			if (e.Node.Nodes.Count == 0 || e.Node.Nodes[0].Text == Strings.Loading)
			{
				e.Node.Nodes.Clear();
				item.GetChildrenAsync(BuildNodes, base.TopLevelControl, e.Node);
			}
		}
	}

	private void BuildNodes(List<Item> children, object callerReference)
	{
		TreeNode treeNode = (TreeNode)callerReference;
		if (children == null || children.Count <= 0)
		{
			return;
		}
		children.Sort((Item i1, Item i2) => Sort.NumericStringCompare(i1.Name, i2.Name));
		foreach (Item child in children)
		{
			if (!IsItemVisible(child, topLevel: false, treeNode.TreeView == treeViewGroup))
			{
				continue;
			}
			TreeNode treeNode2 = BuildTreeNode(child, addLoading: false);
			if (child.HasChildren != VideoOS.Platform.HasChildren.No)
			{
				treeNode2.Nodes.Add(Strings.Loading);
			}
			else if (child.FQID.Kind == Kind.Camera)
			{
				List<Item> children2 = child.GetChildren();
				if (children2 != null)
				{
					children2.Sort((Item i1, Item i2) => Sort.NumericStringCompare(i1.Name, i2.Name));
					foreach (Item item in children2)
					{
						TreeNode treeNode3 = new TreeNode(item.Name);
						treeNode3.Tag = item;
						if (Util.KindToImageIndex.ContainsKey(item.FQID.Kind))
						{
							int imageIndex = (treeNode3.SelectedImageIndex = Util.KindToImageIndex[item.FQID.Kind]);
							treeNode3.ImageIndex = imageIndex;
						}
						treeNode2.Nodes.Add(treeNode3);
					}
				}
			}
			treeNode.Nodes.Add(treeNode2);
			treeNode.Expand();
		}
	}

	private void OnNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
	{
		if (e.Node.Tag != null && e.Node.Tag is Item)
		{
			if ((tabControl1.SelectedTab == tabPageGroup && treeViewGroup.SelectedNode == e.Node) || (tabControl1.SelectedTab == tabPageServer && treeViewServer.SelectedNode == e.Node))
			{
				Item node = e.Node.Tag as Item;
				SelectItem(node);
			}
			buttonRemove.Enabled = false;
		}
	}

	private void SelectItem(Item node)
	{
		Category category = Category.Unknown;
		if (Util.KindToCategory.ContainsKey(node.FQID.Kind))
		{
			category = Util.KindToCategory[node.FQID.Kind];
		}
		buttonRemove.Enabled = false;
		if (this.ValidateSelectionEvent != null)
		{
			ItemPickerForm.ValidateEventArgs e = new ItemPickerForm.ValidateEventArgs();
			e.Item = node;
			e.AcceptSelection = false;
			this.ValidateSelectionEvent(e);
			if (e.AcceptSelection)
			{
				buttonAdd.Enabled = true;
			}
			else
			{
				buttonAdd.Enabled = false;
			}
		}
		else if (node.FQID.FolderType == FolderType.No && (_categoryFilter.Count == 0 || _categoryFilter[category] != null) && (_kindFilterProgram.Count == 0 || _kindFilterProgram[node.FQID.Kind] != null))
		{
			Item item = (Item)_selectedItems[SelectedItemKey(node)];
			FQID fQID = null;
			if (item != null)
			{
				fQID = item.FQID;
			}
			bool flag = false;
			if (fQID != null)
			{
				flag = fQID.Equals(node.FQID);
			}
			buttonAdd.Enabled = !flag;
			if (buttonAdd.Enabled && _singleSelect)
			{
				_selectedItems.Add(SelectedItemKey(node), node);
				if (this.CloseDialog != null)
				{
					this.CloseDialog(this, new EventArgs());
				}
				FireSelectionChangedEvent();
			}
		}
		else
		{
			buttonAdd.Enabled = false;
		}
	}

	private void OnAdd(object sender, EventArgs e)
	{
		if (tabControl1.SelectedTab == tabPageGroup)
		{
			if (treeViewGroup.SelectedNode != null && treeViewGroup.SelectedNode.Tag is Item)
			{
				Item item = (Item)treeViewGroup.SelectedNode.Tag;
				_selectedItems[SelectedItemKey(item)] = item;
				TreeNode treeNode = new TreeNode(item.Name);
				treeNode.Tag = item;
				int imageIndex = (treeNode.SelectedImageIndex = treeViewGroup.SelectedNode.ImageIndex);
				treeNode.ImageIndex = imageIndex;
				treeViewSelected.Nodes.Add(treeNode);
				FireSelectionChangedEvent();
			}
		}
		else if (treeViewServer.SelectedNode != null && treeViewServer.SelectedNode.Tag is Item)
		{
			Item item2 = (Item)treeViewServer.SelectedNode.Tag;
			_selectedItems[SelectedItemKey(item2)] = item2;
			TreeNode treeNode2 = new TreeNode(item2.Name);
			treeNode2.Tag = item2;
			int imageIndex = (treeNode2.SelectedImageIndex = treeViewServer.SelectedNode.ImageIndex);
			treeNode2.ImageIndex = imageIndex;
			treeViewSelected.Nodes.Add(treeNode2);
			FireSelectionChangedEvent();
		}
		buttonAdd.Enabled = false;
		buttonRemove.Enabled = true;
	}

	private void OnTreeViewDoubleClick(object sender, EventArgs e)
	{
		if (buttonAdd.Enabled)
		{
			OnAdd(null, null);
		}
		else if (buttonRemove.Enabled)
		{
			OnRemove(null, null);
		}
	}

	private void OnAfterSelect(object sender, TreeViewEventArgs e)
	{
		if (tabControl1.SelectedTab == tabPageGroup)
		{
			if (treeViewGroup.SelectedNode != null && treeViewGroup.SelectedNode.Tag is Item)
			{
				Item node = (Item)treeViewGroup.SelectedNode.Tag;
				SelectItem(node);
			}
		}
		else if (treeViewServer.SelectedNode != null && treeViewServer.SelectedNode.Tag is Item)
		{
			Item node2 = (Item)treeViewServer.SelectedNode.Tag;
			SelectItem(node2);
		}
	}

	private void OnMouseDownSelected(object sender, MouseEventArgs e)
	{
		buttonAdd.Enabled = false;
		buttonRemove.Enabled = treeViewSelected.SelectedNode != null;
	}

	private void OnRemove(object sender, EventArgs e)
	{
		if (treeViewSelected.SelectedNode != null)
		{
			Item item = (Item)treeViewSelected.SelectedNode.Tag;
			if (item != null && _selectedItems[SelectedItemKey(item)] != null)
			{
				_selectedItems.Remove(SelectedItemKey(item));
			}
			treeViewSelected.Nodes.Remove(treeViewSelected.SelectedNode);
			if (treeViewSelected.Nodes.Count == 0)
			{
				buttonRemove.Enabled = false;
			}
			FireSelectionChangedEvent();
		}
	}

	private void OnSelectedDoubleClick(object sender, EventArgs e)
	{
		if (buttonRemove.Enabled)
		{
			OnRemove(null, null);
		}
	}

	private void OnSelect(object sender, TreeNodeMouseClickEventArgs e)
	{
		buttonAdd.Enabled = false;
		buttonRemove.Enabled = true;
	}

	private void OnCategoryChanged(object sender, EventArgs e)
	{
		if (!_buildingDropDowns)
		{
			object selectedItem = comboBoxCategoryFilter.SelectedItem;
			if (selectedItem != null && selectedItem is Category)
			{
				_categoryFilter = new Hashtable();
				_categoryFilter[selectedItem] = true;
			}
			else
			{
				_categoryFilter = new Hashtable();
			}
			if (_itemsToSelectFrom.Count != 0)
			{
				ItemsToSelectFrom = _itemsToSelectFrom;
				return;
			}
			ItemsToSelectFromServer = _itemsToSelectFromServer;
			ItemsToSelectFromGroup = _itemsToSelectFromGroup;
		}
	}

	private void OnTypeFilterChanged(object sender, EventArgs e)
	{
		if (_buildingDropDowns)
		{
			return;
		}
		object selectedItem = comboBoxKind.SelectedItem;
		if (selectedItem != null && selectedItem is TaggedItem)
		{
			Guid tag = ((TaggedItem)selectedItem).Tag;
			_kindFilterUser = new Hashtable();
			_kindFilterUser[tag] = true;
			ItemNode itemNode = PluginManager.GetItemNode(tag);
			while (itemNode != null && itemNode.ParentKind != Guid.Empty)
			{
				itemNode = PluginManager.GetItemNode(itemNode.ParentKind);
				if (itemNode != null)
				{
					_kindFilterUser[itemNode.ItemKind] = true;
				}
			}
		}
		else
		{
			_kindFilterUser = (Hashtable)_kindFilterProgram.Clone();
			foreach (Guid key in _kindFilterProgram.Keys)
			{
				ItemNode itemNode2 = PluginManager.GetItemNode(key);
				while (itemNode2 != null && itemNode2.ParentKind != Guid.Empty)
				{
					itemNode2 = PluginManager.GetItemNode(itemNode2.ParentKind);
					if (itemNode2 != null)
					{
						_kindFilterUser[itemNode2.ItemKind] = true;
					}
				}
			}
		}
		if (_itemsToSelectFrom.Count != 0)
		{
			ItemsToSelectFrom = _itemsToSelectFrom;
			return;
		}
		ItemsToSelectFromServer = _itemsToSelectFromServer;
		ItemsToSelectFromGroup = _itemsToSelectFromGroup;
	}

	private string SelectedItemKey(Item item)
	{
		string text = item.FQID.ServerId.Id.ToString() + item.FQID.Kind.ToString() + item.FQID.ParentId.ToString();
		if (item.FQID.ObjectIdString == null)
		{
			return text + item.FQID.ObjectId.ToString();
		}
		return text + item.FQID.ObjectId.ToString() + item.FQID.ObjectIdString;
	}

	private bool IsItemVisible(Item item, bool topLevel, bool groupTreeViewActive)
	{
		bool flag = IsItemVisibleBeforePluginCheck(item, topLevel, groupTreeViewActive);
		if (flag && this.IsItemVisibleEvent != null)
		{
			ItemPickerForm.IsItemVisibleEventArgs e = new ItemPickerForm.IsItemVisibleEventArgs
			{
				Item = item,
				Visible = true
			};
			this.IsItemVisibleEvent(e);
			flag = e.Visible;
		}
		return flag;
	}

	private bool IsItemVisibleBeforePluginCheck(Item item, bool topLevel, bool groupTreeViewActive)
	{
		bool flag = true;
		if (item.Properties != null && item.Properties.ContainsKey("Enabled"))
		{
			string text = item.Properties["Enabled"];
			if (text != null && (string.Compare(text, "No", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(text, "False", StringComparison.OrdinalIgnoreCase) == 0))
			{
				flag = false;
			}
		}
		if (!ShowDisabledItems && !flag)
		{
			return false;
		}
		if (item.FQID.FolderType == FolderType.No || (item.FQID.FolderType == FolderType.SystemDefined && !groupTreeViewActive) || (item.FQID.FolderType == FolderType.UserDefined && groupTreeViewActive) || (item.FQID.Kind == Kind.Server && topLevel))
		{
			Category category = Category.Unknown;
			if (Util.KindToCategory.ContainsKey(item.FQID.Kind))
			{
				category = Util.KindToCategory[item.FQID.Kind];
			}
			bool flag2 = _categoryFilter != null && Kind.DefaultTypeToCategoryTable[item.FQID.Kind] != null && _categoryFilter[Kind.DefaultTypeToCategoryTable[item.FQID.Kind]] != null;
			bool flag3 = (_kindFilterUser != null && _kindFilterUser.ContainsKey(item.FQID.Kind)) || (!topLevel && Kind.Folder == item.FQID.Kind);
			bool flag4 = _categoryFilter != null && _categoryFilter.ContainsKey(Category.TriggerOut) && item.FQID.Kind == Kind.Camera;
			bool flag5 = item.FQID.Kind == Kind.Camera && _kindFilterUser != null && _kindFilterUser.ContainsKey(Kind.Preset);
			bool flag6 = category == Category.Server && item.FQID.Kind == Kind.Server && item.HasChildren != VideoOS.Platform.HasChildren.No;
			if ((_categoryFilter == null || _categoryFilter.Count == 0) && (_kindFilterUser == null || _kindFilterUser.Count == 0))
			{
				return true;
			}
			return flag2 || flag4 || flag3 || flag5 || flag6;
		}
		return false;
	}

	private void FireSelectionChangedEvent()
	{
		if (this.ItemsSelectedChangedEvent != null)
		{
			this.ItemsSelectedChangedEvent(this, new EventArgs());
		}
	}

	private void OnResize(object sender, EventArgs e)
	{
		int num = buttonRemove.Location.Y + buttonRemove.Height - buttonAdd.Location.Y;
		int num2 = buttonRemove.Location.Y - buttonAdd.Location.Y;
		int num3 = treeViewSelected.Height - num;
		buttonAdd.Location = new Point(buttonAdd.Location.X, num3 / 2 + treeViewSelected.Location.Y);
		buttonRemove.Location = new Point(buttonRemove.Location.X, num3 / 2 + num2 + treeViewSelected.Location.Y);
	}

	private void OnClick(object sender, EventArgs e)
	{
		OnClick(e);
	}

	private void OnDoubleClick(object sender, EventArgs e)
	{
		OnDoubleClick(e);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Platform.UI.ItemPickerUserControl));
		this.panelTrees = new System.Windows.Forms.Panel();
		this.splitContainer1 = new System.Windows.Forms.SplitContainer();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPageGroup = new System.Windows.Forms.TabPage();
		this.treeViewGroup = new System.Windows.Forms.TreeView();
		this.tabPageServer = new System.Windows.Forms.TabPage();
		this.treeViewServer = new System.Windows.Forms.TreeView();
		this.labelSelected = new System.Windows.Forms.Label();
		this.treeViewSelected = new System.Windows.Forms.TreeView();
		this.buttonRemove = new System.Windows.Forms.Button();
		this.buttonAdd = new System.Windows.Forms.Button();
		this.panelTop = new System.Windows.Forms.Panel();
		this.labelCategory = new System.Windows.Forms.Label();
		this.labelKind = new System.Windows.Forms.Label();
		this.comboBoxCategoryFilter = new System.Windows.Forms.ComboBox();
		this.comboBoxKind = new System.Windows.Forms.ComboBox();
		this.panelTrees.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
		this.splitContainer1.Panel1.SuspendLayout();
		this.splitContainer1.Panel2.SuspendLayout();
		this.splitContainer1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPageGroup.SuspendLayout();
		this.tabPageServer.SuspendLayout();
		this.panelTop.SuspendLayout();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.panelTrees, "panelTrees");
		this.panelTrees.Controls.Add(this.splitContainer1);
		this.panelTrees.Name = "panelTrees";
		componentResourceManager.ApplyResources(this.splitContainer1, "splitContainer1");
		this.splitContainer1.Name = "splitContainer1";
		this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
		this.splitContainer1.Panel1.Click += new System.EventHandler(OnClick);
		this.splitContainer1.Panel1.DoubleClick += new System.EventHandler(OnDoubleClick);
		this.splitContainer1.Panel2.Controls.Add(this.labelSelected);
		this.splitContainer1.Panel2.Controls.Add(this.treeViewSelected);
		this.splitContainer1.Panel2.Controls.Add(this.buttonRemove);
		this.splitContainer1.Panel2.Controls.Add(this.buttonAdd);
		this.splitContainer1.Panel2.Click += new System.EventHandler(OnClick);
		this.splitContainer1.Panel2.DoubleClick += new System.EventHandler(OnDoubleClick);
		this.splitContainer1.TabStop = false;
		componentResourceManager.ApplyResources(this.tabControl1, "tabControl1");
		this.tabControl1.Controls.Add(this.tabPageGroup);
		this.tabControl1.Controls.Add(this.tabPageServer);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabPageGroup.Controls.Add(this.treeViewGroup);
		componentResourceManager.ApplyResources(this.tabPageGroup, "tabPageGroup");
		this.tabPageGroup.Name = "tabPageGroup";
		this.tabPageGroup.UseVisualStyleBackColor = true;
		this.treeViewGroup.BorderStyle = System.Windows.Forms.BorderStyle.None;
		componentResourceManager.ApplyResources(this.treeViewGroup, "treeViewGroup");
		this.treeViewGroup.Name = "treeViewGroup";
		this.treeViewGroup.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(OnBeforeExpand);
		this.treeViewGroup.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(OnAfterSelect);
		this.treeViewGroup.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(OnNodeMouseClick);
		this.treeViewGroup.DoubleClick += new System.EventHandler(OnTreeViewDoubleClick);
		this.tabPageServer.Controls.Add(this.treeViewServer);
		componentResourceManager.ApplyResources(this.tabPageServer, "tabPageServer");
		this.tabPageServer.Name = "tabPageServer";
		this.tabPageServer.UseVisualStyleBackColor = true;
		this.treeViewServer.BorderStyle = System.Windows.Forms.BorderStyle.None;
		componentResourceManager.ApplyResources(this.treeViewServer, "treeViewServer");
		this.treeViewServer.Name = "treeViewServer";
		this.treeViewServer.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(OnBeforeExpand);
		this.treeViewServer.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(OnAfterSelect);
		this.treeViewServer.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(OnNodeMouseClick);
		this.treeViewServer.DoubleClick += new System.EventHandler(OnTreeViewDoubleClick);
		componentResourceManager.ApplyResources(this.labelSelected, "labelSelected");
		this.labelSelected.Name = "labelSelected";
		this.labelSelected.Click += new System.EventHandler(OnClick);
		this.labelSelected.DoubleClick += new System.EventHandler(OnDoubleClick);
		componentResourceManager.ApplyResources(this.treeViewSelected, "treeViewSelected");
		this.treeViewSelected.HideSelection = false;
		this.treeViewSelected.Name = "treeViewSelected";
		this.treeViewSelected.ShowLines = false;
		this.treeViewSelected.ShowPlusMinus = false;
		this.treeViewSelected.ShowRootLines = false;
		this.treeViewSelected.Sorted = true;
		this.treeViewSelected.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(OnSelect);
		this.treeViewSelected.DoubleClick += new System.EventHandler(OnSelectedDoubleClick);
		this.treeViewSelected.MouseDown += new System.Windows.Forms.MouseEventHandler(OnMouseDownSelected);
		this.treeViewSelected.Resize += new System.EventHandler(OnResize);
		componentResourceManager.ApplyResources(this.buttonRemove, "buttonRemove");
		this.buttonRemove.Name = "buttonRemove";
		this.buttonRemove.UseVisualStyleBackColor = true;
		this.buttonRemove.Click += new System.EventHandler(OnRemove);
		componentResourceManager.ApplyResources(this.buttonAdd, "buttonAdd");
		this.buttonAdd.Name = "buttonAdd";
		this.buttonAdd.UseVisualStyleBackColor = true;
		this.buttonAdd.Click += new System.EventHandler(OnAdd);
		componentResourceManager.ApplyResources(this.panelTop, "panelTop");
		this.panelTop.Controls.Add(this.labelCategory);
		this.panelTop.Controls.Add(this.labelKind);
		this.panelTop.Controls.Add(this.comboBoxCategoryFilter);
		this.panelTop.Controls.Add(this.comboBoxKind);
		this.panelTop.Name = "panelTop";
		this.panelTop.Click += new System.EventHandler(OnClick);
		this.panelTop.DoubleClick += new System.EventHandler(OnDoubleClick);
		componentResourceManager.ApplyResources(this.labelCategory, "labelCategory");
		this.labelCategory.Name = "labelCategory";
		this.labelCategory.Click += new System.EventHandler(OnClick);
		this.labelCategory.DoubleClick += new System.EventHandler(OnDoubleClick);
		componentResourceManager.ApplyResources(this.labelKind, "labelKind");
		this.labelKind.Name = "labelKind";
		this.labelKind.Click += new System.EventHandler(OnClick);
		this.labelKind.DoubleClick += new System.EventHandler(OnDoubleClick);
		this.comboBoxCategoryFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBoxCategoryFilter.FormattingEnabled = true;
		componentResourceManager.ApplyResources(this.comboBoxCategoryFilter, "comboBoxCategoryFilter");
		this.comboBoxCategoryFilter.Name = "comboBoxCategoryFilter";
		this.comboBoxCategoryFilter.SelectedIndexChanged += new System.EventHandler(OnCategoryChanged);
		this.comboBoxKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBoxKind.FormattingEnabled = true;
		componentResourceManager.ApplyResources(this.comboBoxKind, "comboBoxKind");
		this.comboBoxKind.Name = "comboBoxKind";
		this.comboBoxKind.SelectedIndexChanged += new System.EventHandler(OnTypeFilterChanged);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		componentResourceManager.ApplyResources(this, "$this");
		this.BackColor = System.Drawing.Color.WhiteSmoke;
		base.Controls.Add(this.panelTop);
		base.Controls.Add(this.panelTrees);
		base.Name = "ItemPickerUserControl";
		this.panelTrees.ResumeLayout(false);
		this.splitContainer1.Panel1.ResumeLayout(false);
		this.splitContainer1.Panel2.ResumeLayout(false);
		this.splitContainer1.Panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
		this.splitContainer1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPageGroup.ResumeLayout(false);
		this.tabPageServer.ResumeLayout(false);
		this.panelTop.ResumeLayout(false);
		this.panelTop.PerformLayout();
		base.ResumeLayout(false);
	}
}
