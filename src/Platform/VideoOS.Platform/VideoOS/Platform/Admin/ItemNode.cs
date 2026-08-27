using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace VideoOS.Platform.Admin;

public class ItemNode
{
	private Guid _itemKind;

	private Guid _parentKind;

	private Image _icon;

	private string _name;

	private Image _iconMultiple;

	private string _nameMultiple;

	private ItemsAllowed _itemsAllowed;

	private ItemManager _itemManager;

	private List<ItemNode> _itemNodes;

	private Category _category;

	private bool _selectable;

	private List<SecurityAction> _securityActions;

	private Collection<MapAlarmContextMenu> _contextMenu = new Collection<MapAlarmContextMenu>();

	private object _customData;

	private Icon _mapIcon;

	private int _sortKey = 1000;

	private PlacementHint _placementHint;

	private Dictionary<Guid, Icon> _mapIconDictionary = new Dictionary<Guid, Icon>();

	private bool _gisMapEnabled = true;

	public Guid ItemKind => _itemKind;

	public Guid ParentKind => _parentKind;

	public virtual string Name => _name;

	public virtual Image Icon => _icon;

	public virtual Icon MapIcon
	{
		get
		{
			return _mapIcon;
		}
		set
		{
			_mapIcon = value;
		}
	}

	public virtual Dictionary<Guid, Icon> MapIconDictionary
	{
		get
		{
			return _mapIconDictionary;
		}
		set
		{
			_mapIconDictionary = value;
		}
	}

	public virtual string NameMultiple
	{
		get
		{
			if (string.IsNullOrEmpty(_nameMultiple))
			{
				return _name;
			}
			return _nameMultiple;
		}
	}

	public virtual string PropertyHeading { get; set; }

	public virtual Image IconMultiple
	{
		get
		{
			if (_iconMultiple == null)
			{
				return _icon;
			}
			return _iconMultiple;
		}
	}

	public virtual Category Category => _category;

	public bool Selectable => _selectable;

	public virtual ItemsAllowed ItemsAllowed => _itemsAllowed;

	public virtual ItemManager ItemManager => _itemManager;

	public virtual List<ItemNode> ItemNodes => _itemNodes;

	public virtual List<SecurityAction> SecurityActions
	{
		get
		{
			return _securityActions;
		}
		set
		{
			_securityActions = value;
		}
	}

	public virtual Collection<MapAlarmContextMenu> ContextMenu
	{
		get
		{
			return _contextMenu;
		}
		set
		{
			_contextMenu = value;
		}
	}

	public virtual object CustomData
	{
		get
		{
			return _customData;
		}
		set
		{
			_customData = value;
		}
	}

	public virtual int SortKey
	{
		get
		{
			return _sortKey;
		}
		set
		{
			_sortKey = value;
		}
	}

	public virtual PlacementHint PlacementHint
	{
		get
		{
			return _placementHint;
		}
		set
		{
			_placementHint = value;
		}
	}

	public virtual bool GisMapEnabled
	{
		get
		{
			return _gisMapEnabled;
		}
		set
		{
			_gisMapEnabled = value;
		}
	}

	public ItemNode()
	{
	}

	public ItemNode(Guid kind, Guid parentId, string nodeText, Image image, string nodeTextMultiple, Image imageMultiple, ItemsAllowed itemsAllowed, Category category, bool selectable)
	{
		_itemKind = kind;
		_parentKind = parentId;
		_icon = image;
		_name = nodeText;
		_iconMultiple = imageMultiple;
		_nameMultiple = nodeTextMultiple;
		_itemsAllowed = itemsAllowed;
		_category = category;
		_selectable = selectable;
	}

	public ItemNode(Guid id, Guid parentId, string nodeText, Image image, string nodeTextMultiple, Image imageMultiple, Category category, bool selectable, ItemsAllowed itemsAllowed, ItemManager itemManager, List<ItemNode> itemNodes)
	{
		_itemKind = id;
		_parentKind = parentId;
		_icon = image;
		_name = nodeText;
		_iconMultiple = imageMultiple;
		_nameMultiple = nodeTextMultiple;
		_itemsAllowed = itemsAllowed;
		_category = category;
		_selectable = selectable;
		_itemManager = itemManager;
		_itemNodes = itemNodes;
	}

	public ItemNode(Guid id, Guid parentId, string nodeText, Image image, string nodeTextMultiple, Image imageMultiple, Category category, bool selectable, ItemsAllowed itemsAllowed, ItemManager itemManager, List<ItemNode> itemNodes, List<SecurityAction> securityActions)
	{
		_itemKind = id;
		_parentKind = parentId;
		_icon = image;
		_name = nodeText;
		_iconMultiple = imageMultiple;
		_nameMultiple = nodeTextMultiple;
		_itemsAllowed = itemsAllowed;
		_category = category;
		_selectable = selectable;
		_itemManager = itemManager;
		_itemNodes = itemNodes;
		_securityActions = securityActions;
	}

	public ItemNode(Guid id, Guid parentId, string nodeText, Image image, string nodeTextMultiple, Image imageMultiple, Category category, bool selectable, ItemsAllowed itemsAllowed, ItemManager itemManager, List<ItemNode> itemNodes, List<SecurityAction> securityActions, Collection<MapAlarmContextMenu> contextMenu)
	{
		_itemKind = id;
		_parentKind = parentId;
		_icon = image;
		_name = nodeText;
		_iconMultiple = imageMultiple;
		_nameMultiple = nodeTextMultiple;
		_itemsAllowed = itemsAllowed;
		_category = category;
		_selectable = selectable;
		_itemManager = itemManager;
		_itemNodes = itemNodes;
		_securityActions = securityActions;
		_contextMenu = contextMenu;
	}

	public ItemNode(Guid id, Guid parentId, string nodeText, Image image, string nodeTextMultiple, Image imageMultiple, Category category, bool selectable, ItemsAllowed itemsAllowed, ItemManager itemManager, List<ItemNode> itemNodes, List<SecurityAction> securityActions, Collection<MapAlarmContextMenu> contextMenu, object customData)
	{
		_itemKind = id;
		_parentKind = parentId;
		_icon = image;
		_name = nodeText;
		_iconMultiple = imageMultiple;
		_nameMultiple = nodeTextMultiple;
		_itemsAllowed = itemsAllowed;
		_category = category;
		_selectable = selectable;
		_itemManager = itemManager;
		_itemNodes = itemNodes;
		_securityActions = securityActions;
		_contextMenu = contextMenu;
		_customData = customData;
	}

	public ItemNode(Guid id, Guid parentId, string nodeText, Image image, string nodeTextMultiple, Image imageMultiple, Category category, bool selectable, ItemsAllowed itemsAllowed, ItemManager itemManager, List<ItemNode> itemNodes, List<SecurityAction> securityActions, Collection<MapAlarmContextMenu> contextMenu, object customData, PlacementHint placementHint)
	{
		_itemKind = id;
		_parentKind = parentId;
		_icon = image;
		_name = nodeText;
		_iconMultiple = imageMultiple;
		_nameMultiple = nodeTextMultiple;
		_itemsAllowed = itemsAllowed;
		_category = category;
		_selectable = selectable;
		_itemManager = itemManager;
		_itemNodes = itemNodes;
		_securityActions = securityActions;
		_contextMenu = contextMenu;
		_customData = customData;
		_placementHint = placementHint;
	}

	public override string ToString()
	{
		return _name;
	}
}
