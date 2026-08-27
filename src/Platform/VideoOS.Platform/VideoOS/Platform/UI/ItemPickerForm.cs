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
public class ItemPickerForm : Form
{
	public delegate void ValidateSelectionHandler(ValidateEventArgs e);

	public class ValidateEventArgs
	{
		public Item Item { get; set; }

		public bool AcceptSelection { get; set; }
	}

	public delegate void IsItemVisibleHandler(IsItemVisibleEventArgs e);

	public class IsItemVisibleEventArgs
	{
		public Item Item { get; set; }

		public bool Visible { get; set; }
	}

	private Category _category = Category.VideoIn;

	private Guid _kind;

	private bool _categoryFilter;

	private bool _kindFilter;

	private Item _selectedItem;

	private bool _ignoreClicks;

	private bool _autoAccept;

	private bool _physicalHierarchy;

	private bool _ignoreHierarchy = true;

	private Hashtable _kindParents = new Hashtable();

	private IContainer components;

	private TreeView treeViewItems;

	private Button buttonCancel;

	private Button buttonOK;

	private Label labelTop;

	private CheckBox checkBoxGroupHierarchy;

	public Category CategoryFilter
	{
		get
		{
			return _category;
		}
		set
		{
			_category = value;
			_categoryFilter = true;
		}
	}

	public Guid KindFilter
	{
		get
		{
			return _kind;
		}
		set
		{
			_kind = value;
			_kindFilter = true;
			_kindParents.Clear();
			ItemNode itemNode = PluginManager.GetItemNode(_kind);
			while (itemNode != null && itemNode.ParentKind != Guid.Empty)
			{
				itemNode = PluginManager.GetItemNode(itemNode.ParentKind);
				if (itemNode != null)
				{
					_kindParents[itemNode.ItemKind] = true;
				}
			}
		}
	}

	public string TopLabel
	{
		get
		{
			return labelTop.Text;
		}
		set
		{
			labelTop.Text = value;
		}
	}

	public bool AutoAccept
	{
		get
		{
			return _autoAccept;
		}
		set
		{
			_autoAccept = value;
			buttonOK.Visible = !value;
		}
	}

	public bool AutoExpand { get; set; }

	public Item SelectedItem
	{
		get
		{
			return _selectedItem;
		}
		set
		{
			_selectedItem = value;
		}
	}

	public bool ShowDisabledItems { get; set; }

	public event ValidateSelectionHandler ValidateSelectionEvent;

	public event IsItemVisibleHandler IsItemVisibleEvent;

	public ItemPickerForm()
	{
		InitializeComponent();
		buttonOK.Enabled = false;
		ShowDisabledItems = false;
		AutoExpand = true;
		base.Icon = Images.SDK_general;
		ClientControl.Instance.RegisterUIControlForAutoTheming(this);
		new TrackPageViewData(typeof(ItemPickerForm), deprecated: true).Send();
	}

	public void Init()
	{
		treeViewItems.ImageList = Util.ImageListClone;
		_ignoreHierarchy = false;
		FillTreeView(_physicalHierarchy);
	}

	public void Init(List<Item> topLevelItems)
	{
		checkBoxGroupHierarchy.Visible = false;
		_ignoreHierarchy = true;
		treeViewItems.ImageList = Util.ImageListClone;
		FillTreeView(topLevelItems);
	}

	public void Init(List<Item> topLevelItems, ItemHierarchy itemHierarchy)
	{
		checkBoxGroupHierarchy.Visible = false;
		_ignoreHierarchy = itemHierarchy == ItemHierarchy.Both;
		_physicalHierarchy = itemHierarchy == ItemHierarchy.SystemDefined;
		treeViewItems.ImageList = Util.ImageListClone;
		FillTreeView(topLevelItems);
	}

	private void FillTreeView(bool physicalHierarchy)
	{
		using (new WaitCursor())
		{
			List<Item> items = Configuration.Instance.GetItems(physicalHierarchy ? ItemHierarchy.SystemDefined : ItemHierarchy.UserDefined);
			FillTreeView(items);
		}
	}

	private void FillTreeView(List<Item> topLevelItems)
	{
		Hashtable hashtable = new Hashtable();
		using (new WaitCursor())
		{
			_ignoreClicks = true;
			treeViewItems.Nodes.Clear();
			topLevelItems.Sort((Item i1, Item i2) => Sort.NumericStringCompare(i1.Name, i2.Name));
			foreach (Item topLevelItem in topLevelItems)
			{
				try
				{
					if (IsItemVisible(topLevelItem, topLevel: true) && hashtable[topLevelItem.FQID.ObjectId] == null)
					{
						TreeNode treeNode = new TreeNode(topLevelItem.Name)
						{
							Tag = topLevelItem
						};
						Image image = topLevelItem.IconWithOverlay();
						if (image != null)
						{
							int imageIndex = (treeNode.SelectedImageIndex = treeViewItems.ImageList.Images.Count);
							treeNode.ImageIndex = imageIndex;
							treeViewItems.ImageList.Images.Add(image);
						}
						else
						{
							int imageIndex = (treeNode.SelectedImageIndex = Util.BuiltInItemToImageIndex(topLevelItem));
							treeNode.ImageIndex = imageIndex;
						}
						if (topLevelItem.FQID.FolderType != FolderType.No || topLevelItem.HasChildren != VideoOS.Platform.HasChildren.No)
						{
							treeNode.Nodes.Add(Strings.Loading);
						}
						treeViewItems.Nodes.Add(treeNode);
						if (topLevelItem.FQID.ObjectId != Guid.Empty)
						{
							hashtable[topLevelItem.FQID.ObjectId] = topLevelItem;
						}
					}
				}
				catch (Exception exception)
				{
					EnvironmentManager.Instance.ExceptionHandler("ItemPickerForm::FillTree", exception);
				}
			}
			if (AutoExpand && treeViewItems.Nodes.Count == 1)
			{
				treeViewItems.Nodes[0].Expand();
			}
			_ignoreClicks = false;
		}
	}

	private void OnAfterExpand(object sender, TreeViewEventArgs e)
	{
	}

	private void OnBeforeExpand(object sender, TreeViewCancelEventArgs e)
	{
		if (e.Node.Tag != null && e.Node.Tag is Item)
		{
			Item item = e.Node.Tag as Item;
			if (e.Node.Nodes.Count == 0 || e.Node.Nodes[0].Text == Strings.Loading)
			{
				_ignoreClicks = true;
				e.Node.Nodes.Clear();
				item.GetChildrenAsync(BuildNodes, base.TopLevelControl, e.Node);
			}
		}
	}

	private void BuildNodes(List<Item> children, object callerReference)
	{
		try
		{
			TreeNode treeNode = (TreeNode)callerReference;
			if (children != null && children.Count > 0)
			{
				TreeNode selectedNode = null;
				Hashtable hashtable = new Hashtable();
				children.Sort((Item i1, Item i2) => Sort.NumericStringCompare(i1.Name, i2.Name));
				foreach (Item child in children)
				{
					if (!IsItemVisible(child, topLevel: false) || hashtable[child.FQID.ObjectId] != null)
					{
						continue;
					}
					TreeNode treeNode2 = new TreeNode(child.Name);
					treeNode2.Tag = child;
					Image image = child.IconWithOverlay();
					if (image != null)
					{
						TreeNode treeNode3 = treeNode2;
						int imageIndex = (treeNode2.SelectedImageIndex = treeViewItems.ImageList.Images.Count);
						treeNode3.ImageIndex = imageIndex;
						treeViewItems.ImageList.Images.Add(image);
					}
					else
					{
						TreeNode treeNode4 = treeNode2;
						int imageIndex = (treeNode2.SelectedImageIndex = Util.BuiltInItemToImageIndex(child));
						treeNode4.ImageIndex = imageIndex;
					}
					if (child.FQID.FolderType != FolderType.No || child.HasChildren != VideoOS.Platform.HasChildren.No)
					{
						treeNode2.Nodes.Add(Strings.Loading);
					}
					else if (child.FQID.Kind == Kind.Camera && (_category == Category.TriggerOut || _kind == Kind.Preset || _kind == Guid.Empty))
					{
						List<Item> children2 = child.GetChildren();
						if (children2 != null && children2.Count > 0)
						{
							children2.Sort((Item i1, Item i2) => Sort.NumericStringCompare(i1.Name, i2.Name));
							foreach (Item item in children2)
							{
								TreeNode treeNode5 = new TreeNode(item.Name);
								treeNode5.Tag = item;
								if (Util.KindToImageIndex.ContainsKey(item.FQID.Kind))
								{
									int imageIndex = (treeNode5.SelectedImageIndex = Util.KindToImageIndex[item.FQID.Kind]);
									treeNode5.ImageIndex = imageIndex;
								}
								treeNode2.Nodes.Add(treeNode5);
								if (_selectedItem != null && _selectedItem.FQID.Kind == Kind.Preset && item.FQID.ObjectIdString == _selectedItem.FQID.ObjectIdString)
								{
									selectedNode = treeNode5;
								}
							}
						}
						else if (_category == Category.TriggerOut || _kind == Kind.Preset)
						{
							treeNode2 = null;
						}
					}
					if (treeNode2 != null)
					{
						treeNode.Nodes.Add(treeNode2);
						if (_selectedItem != null && child.FQID.ObjectId == _selectedItem.FQID.ObjectId)
						{
							selectedNode = treeNode2;
						}
						if (child.FQID.ObjectId != Guid.Empty)
						{
							hashtable[child.FQID.ObjectId] = child;
						}
					}
				}
				_ignoreClicks = true;
				treeNode.Expand();
				treeViewItems.SelectedNode = selectedNode;
				if (AutoExpand && treeNode.Nodes.Count == 1)
				{
					treeNode.Nodes[0].Expand();
				}
				else
				{
					_ignoreClicks = false;
				}
			}
			else
			{
				_ignoreClicks = false;
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionDialog("ItemPickerForm", exception);
		}
	}

	private void OnNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
	{
		if (e.Node.Tag != null && e.Node.Tag is Item && (!_ignoreClicks || treeViewItems.Nodes.Count == 1))
		{
			Item item = e.Node.Tag as Item;
			TrySelect(item);
		}
		_ignoreClicks = false;
	}

	private void OnAfterSelect(object sender, TreeViewEventArgs e)
	{
		if (treeViewItems.SelectedNode != null && !_ignoreClicks && !AutoAccept && treeViewItems.SelectedNode.Tag is Item item)
		{
			TrySelect(item);
		}
	}

	private void TrySelect(Item item)
	{
		bool flag = false;
		if (this.ValidateSelectionEvent != null)
		{
			ValidateEventArgs e = new ValidateEventArgs();
			e.Item = item;
			e.AcceptSelection = false;
			this.ValidateSelectionEvent(e);
			if (e.AcceptSelection)
			{
				flag = true;
			}
		}
		else if (item.FQID.FolderType == FolderType.No)
		{
			Category category = Category.Unknown;
			if (Util.KindToCategory.ContainsKey(item.FQID.Kind))
			{
				category = Util.KindToCategory[item.FQID.Kind];
			}
			if ((_categoryFilter && _category == category) || (_kindFilter && _kind == item.FQID.Kind) || (!_categoryFilter && !_kindFilter))
			{
				flag = true;
			}
		}
		else if (item.FQID.FolderType == FolderType.SystemDefined && item.FQID.Kind == Kind.Server && ((_categoryFilter && _category == Category.Server) || (_kindFilter && _kind == Kind.Server)))
		{
			flag = true;
		}
		if (flag)
		{
			_selectedItem = item;
			buttonOK.Enabled = true;
			if (AutoAccept)
			{
				base.DialogResult = DialogResult.OK;
				Close();
			}
			else
			{
				buttonOK.Enabled = true;
			}
		}
		else
		{
			buttonOK.Enabled = false;
		}
	}

	private void OnOK(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void OnCancel(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.Cancel;
		Close();
	}

	private void OnPhysicalHierarchyChanged(object sender, EventArgs e)
	{
		_physicalHierarchy = !checkBoxGroupHierarchy.Checked;
		FillTreeView(_physicalHierarchy);
	}

	private bool IsItemVisible(Item item, bool topLevel)
	{
		bool flag = IsItemVisibleBeforePluginCheck(item, topLevel);
		if (flag && this.IsItemVisibleEvent != null)
		{
			IsItemVisibleEventArgs e = new IsItemVisibleEventArgs
			{
				Item = item,
				Visible = true
			};
			this.IsItemVisibleEvent(e);
			flag = e.Visible;
		}
		return flag;
	}

	private bool IsItemVisibleBeforePluginCheck(Item item, bool topLevel)
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
		if (item.FQID.FolderType == FolderType.No || (item.FQID.FolderType == FolderType.SystemDefined && (_physicalHierarchy || _ignoreHierarchy)) || (item.FQID.FolderType == FolderType.UserDefined && (!_physicalHierarchy || _ignoreHierarchy)) || (item.FQID.Kind == Kind.Server && topLevel))
		{
			Category category = Category.Unknown;
			if (Util.KindToCategory.ContainsKey(item.FQID.Kind))
			{
				category = Util.KindToCategory[item.FQID.Kind];
			}
			bool flag2 = _categoryFilter && _category == category;
			bool flag3 = (_kindFilter && _kind == item.FQID.Kind) || (!topLevel && Kind.Folder == item.FQID.Kind);
			bool flag4 = _kindFilter && _kindParents.ContainsKey(item.FQID.Kind);
			bool flag5 = _categoryFilter && _category == Category.TriggerOut && item.FQID.Kind == Kind.Camera;
			bool flag6 = item.FQID.Kind == Kind.Camera && _kind == Kind.Preset;
			bool flag7 = category == Category.Server && item.FQID.Kind == Kind.Server;
			if (!_categoryFilter && !_kindFilter)
			{
				return true;
			}
			return flag2 || flag5 || flag3 || flag6 || flag7 || flag4;
		}
		return false;
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Platform.UI.ItemPickerForm));
		this.treeViewItems = new System.Windows.Forms.TreeView();
		this.buttonCancel = new System.Windows.Forms.Button();
		this.buttonOK = new System.Windows.Forms.Button();
		this.labelTop = new System.Windows.Forms.Label();
		this.checkBoxGroupHierarchy = new System.Windows.Forms.CheckBox();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.treeViewItems, "treeViewItems");
		this.treeViewItems.Name = "treeViewItems";
		this.treeViewItems.Tag = "";
		this.treeViewItems.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(OnBeforeExpand);
		this.treeViewItems.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(OnAfterExpand);
		this.treeViewItems.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(OnAfterSelect);
		this.treeViewItems.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(OnNodeMouseClick);
		componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
		this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.buttonCancel.Name = "buttonCancel";
		this.buttonCancel.UseVisualStyleBackColor = true;
		this.buttonCancel.Click += new System.EventHandler(OnCancel);
		componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
		this.buttonOK.Name = "buttonOK";
		this.buttonOK.UseVisualStyleBackColor = true;
		this.buttonOK.Click += new System.EventHandler(OnOK);
		componentResourceManager.ApplyResources(this.labelTop, "labelTop");
		this.labelTop.Name = "labelTop";
		componentResourceManager.ApplyResources(this.checkBoxGroupHierarchy, "checkBoxGroupHierarchy");
		this.checkBoxGroupHierarchy.Checked = true;
		this.checkBoxGroupHierarchy.CheckState = System.Windows.Forms.CheckState.Checked;
		this.checkBoxGroupHierarchy.Name = "checkBoxGroupHierarchy";
		this.checkBoxGroupHierarchy.UseVisualStyleBackColor = true;
		this.checkBoxGroupHierarchy.CheckedChanged += new System.EventHandler(OnPhysicalHierarchyChanged);
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.checkBoxGroupHierarchy);
		base.Controls.Add(this.labelTop);
		base.Controls.Add(this.buttonOK);
		base.Controls.Add(this.buttonCancel);
		base.Controls.Add(this.treeViewItems);
		base.Name = "ItemPickerForm";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
