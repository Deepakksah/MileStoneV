using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using VideoOS.Platform.Admin;
using VideoOS.Platform.ConfigurationItems;
using VideoOS.Platform.Data;
using VideoOS.Platform.UI;
using VideoOS.Platform.Util;

namespace VideoOS.Platform;

[Serializable]
public class Item
{
	private FQID _fqid;

	private string _name;

	public HasRelated _hasRelated = HasRelated.Maybe;

	public HasChildren _anyChildren = HasChildren.Maybe;

	private Dictionary<string, string> _properties = new Dictionary<string, string>();

	public List<Item> _related = new List<Item>();

	private Dictionary<string, string> _authorization = new Dictionary<string, string>();

	public List<Item> _children = new List<Item>();

	public bool _isChildrenUsed;

	private Image _image;

	private Guid _mapIconKey = Guid.Empty;

	[NonSerialized]
	private Collection<MapAlarmContextMenu> _contextMenu = new Collection<MapAlarmContextMenu>();

	private PositioningInformation _positioningInformation;

	public virtual FQID FQID
	{
		get
		{
			return _fqid;
		}
		set
		{
			_fqid = value;
		}
	}

	public virtual string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
		}
	}

	public virtual HasChildren HasChildren
	{
		get
		{
			return _anyChildren;
		}
		set
		{
			_anyChildren = value;
		}
	}

	public virtual Image Icon
	{
		get
		{
			if (_image != null)
			{
				return _image;
			}
			return null;
		}
		set
		{
			_image = value;
		}
	}

	public virtual Guid MapIconKey
	{
		get
		{
			return _mapIconKey;
		}
		set
		{
			_mapIconKey = value;
		}
	}

	public virtual HasRelated HasRelated
	{
		get
		{
			return _hasRelated;
		}
		set
		{
			_hasRelated = value;
		}
	}

	public virtual Dictionary<string, string> Properties => _properties;

	public virtual bool Enabled
	{
		get
		{
			bool num = Properties.ContainsKey("Disabled") && (Properties["Disabled"] == "Yes" || Properties["Disabled"] == "True");
			bool flag = Properties.ContainsKey("Enabled") && (Properties["Enabled"] == "No" || Properties["Enabled"] == "False");
			return !(num || flag);
		}
		set
		{
			Properties["Enabled"] = (value ? "Yes" : "No");
			if (Properties.ContainsKey("Disabled"))
			{
				Properties["Disabled"] = (value ? "No" : "Yes");
			}
		}
	}

	public virtual bool Encrypt
	{
		get
		{
			if (Properties == null || !Properties.ContainsKey("__Encrypt"))
			{
				return false;
			}
			if (!bool.TryParse(Properties["__Encrypt"], out var result))
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Value set for {0} property in properties dictionary is not valid.", "__Encrypt"));
			}
			return result;
		}
		set
		{
			Properties["__Encrypt"] = value.ToString();
		}
	}

	public virtual PositioningInformation PositioningInformation
	{
		get
		{
			if (_positioningInformation == null && Properties != null && Properties.ContainsKey("GisPoint"))
			{
				_positioningInformation = new PositioningInformation(Properties["GisPoint"], 0.0, 0.0, 0.0);
			}
			return _positioningInformation;
		}
		set
		{
			_positioningInformation = value;
			if (value == null)
			{
				if (Properties.ContainsKey("GisPoint"))
				{
					Properties.Remove("GisPoint");
				}
			}
			else
			{
				Properties["GisPoint"] = _positioningInformation.ToString();
			}
		}
	}

	public virtual Dictionary<string, string> Authorization => _authorization;

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

	public Item()
	{
		_fqid = null;
		_name = "";
	}

	public Item(FQID fqid, string name)
	{
		_fqid = fqid;
		_name = name;
	}

	public Item(ServerId serverId, string name)
	{
		_fqid = new FQID(serverId);
		_name = name;
	}

	public Item(ServerId serverId, string name, FolderType folderType, Guid kind)
	{
		_fqid = new FQID(serverId, folderType, kind);
		_name = name;
	}

	public Item(ServerId serverId, Guid parentId, Guid objectId, string name, FolderType folderType, Guid kind)
	{
		_fqid = new FQID(serverId, parentId, objectId, folderType, kind);
		_name = name;
	}

	public Item(ServerId serverId, Guid parentId, string objectIdString, string name, FolderType folderType, Guid kind)
	{
		_fqid = new FQID(serverId, parentId, objectIdString, folderType, kind);
		_name = name;
	}

	public Item(MIPItem mipItem)
	{
		Factory.FillMIPItemInItem(mipItem, this);
	}

	public virtual void SetChildren(List<Item> children)
	{
		_children = children;
		_isChildrenUsed = true;
		if (_children != null && _children.Count > 0)
		{
			_anyChildren = HasChildren.Yes;
		}
		else
		{
			_anyChildren = HasChildren.No;
		}
	}

	public virtual Item GetParent()
	{
		try
		{
			if (FQID == null || FQID.ServerId == null || FQID.ParentId == Guid.Empty)
			{
				return null;
			}
			Guid kind = FQID.Kind;
			ItemNode itemNode = PluginManager.GetItemNode(FQID.Kind);
			if (itemNode != null && itemNode.ParentKind != Guid.Empty)
			{
				kind = itemNode.ParentKind;
			}
			if (FQID.Kind == Kind.Preset)
			{
				kind = Kind.Camera;
			}
			if (FQID.Kind == Kind.ViewItemInstance)
			{
				kind = Kind.View;
			}
			Configuration configuration = (EnvironmentManager.UsingUserContext ? FQID.ServerId.UserContext.Configuration : Configuration.Instance);
			Item item = configuration.GetItem(FQID.ParentId, kind);
			if (item != null)
			{
				return item;
			}
			return configuration.GetItem(FQID.ParentId, Kind.Server);
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler("Item:Parent", exception);
			return null;
		}
	}

	public virtual List<Item> GetChildren()
	{
		if (_isChildrenUsed)
		{
			if (_children == null)
			{
				_children = new List<Item>();
			}
			return _children;
		}
		List<Item> list = new List<Item>();
		try
		{
			ItemNode itemNode = PluginManager.GetItemNode(FQID.Kind);
			if (itemNode != null && itemNode.ItemNodes != null)
			{
				foreach (ItemNode itemNode2 in itemNode.ItemNodes)
				{
					List<Item> items = itemNode2.ItemManager.GetItems(this);
					if (items != null)
					{
						list.AddRange(items);
					}
				}
			}
			else if (_children != null && _children.Count > 0)
			{
				list = _children;
			}
		}
		catch (Exception exception)
		{
			EnvironmentManager.Instance.ExceptionHandler(GetType().Name, "GetChildren", exception);
		}
		return list;
	}

	public virtual List<Item> GetChildrenSorted()
	{
		List<Item> children = GetChildren();
		children.Sort((Item i1, Item i2) => Sort.NumericStringCompare(i1.Name, i2.Name));
		return children;
	}

	public virtual void GetChildrenAsync(AsyncItemsHandler asyncItemsHandler, Control control, object callerReference)
	{
		if (control != null)
		{
			control.BeginInvoke(asyncItemsHandler, GetChildren(), callerReference);
		}
		else
		{
			asyncItemsHandler(GetChildren(), callerReference);
		}
	}

	public virtual DataSource GetDataSource()
	{
		return null;
	}

	public virtual List<Item> GetRelated()
	{
		if (_related == null)
		{
			return new List<Item>();
		}
		return _related;
	}

	public virtual void GetRelatedAsync(AsyncItemsHandler asyncItemsHandler, Control control, object callerReference)
	{
		if (control != null)
		{
			control.BeginInvoke(asyncItemsHandler, GetChildren(), callerReference);
		}
		else
		{
			asyncItemsHandler(GetChildren(), callerReference);
		}
	}

	public virtual Image IconWithOverlay()
	{
		if (Enabled)
		{
			return Icon;
		}
		int num = VideoOS.Platform.UI.Util.BuiltInItemToImageIndex(this);
		if (num == 0 && FQID.Kind != Kind.Server)
		{
			return null;
		}
		return Overlay(VideoOS.Platform.UI.Util.ImageList.Images[num], VideoOS.Platform.UI.Util.DisabledOverlay);
	}

	public virtual Image IconWithOverlay(bool attention, bool active)
	{
		if (!Enabled)
		{
			return IconWithOverlay();
		}
		Image icon = Icon;
		if (icon != null)
		{
			if (attention)
			{
				return Overlay(icon, VideoOS.Platform.UI.Util.AttentionOverlay);
			}
			if (active)
			{
				return Overlay(icon, VideoOS.Platform.UI.Util.ActiveOverlay);
			}
			return icon;
		}
		int num = VideoOS.Platform.UI.Util.BuiltInItemToImageIndex(this);
		if (num == 0 && FQID.Kind != Kind.Server)
		{
			return null;
		}
		if (attention)
		{
			return Overlay(VideoOS.Platform.UI.Util.ImageList.Images[num], VideoOS.Platform.UI.Util.AttentionOverlay);
		}
		if (active)
		{
			return Overlay(VideoOS.Platform.UI.Util.ImageList.Images[num], VideoOS.Platform.UI.Util.ActiveOverlay);
		}
		return VideoOS.Platform.UI.Util.ImageList.Images[num];
	}

	private static Image Overlay(Image active, Bitmap overlay)
	{
		Bitmap bitmap = new Bitmap(active, 16, 16);
		Graphics graphics = Graphics.FromImage(bitmap);
		overlay.MakeTransparent(Color.White);
		graphics.DrawImage(overlay, 0, 0);
		graphics.Dispose();
		return bitmap;
	}

	public override string ToString()
	{
		return Name;
	}

	public virtual bool Contains(string searchString)
	{
		if (Name.IndexOf(searchString, StringComparison.InvariantCultureIgnoreCase) != -1)
		{
			return true;
		}
		if (Properties != null)
		{
			Dictionary<string, string> properties = Properties;
			foreach (string key in properties.Keys)
			{
				if (key.Contains(searchString))
				{
					return true;
				}
				if (properties[key] != null && properties[key].IndexOf(searchString, StringComparison.InvariantCultureIgnoreCase) != -1)
				{
					return true;
				}
			}
		}
		if (FQID.Contains(searchString))
		{
			return true;
		}
		return false;
	}

	public string Serialize()
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder, CultureInfo.InvariantCulture));
		Serialize(xmlTextWriter);
		xmlTextWriter.Flush();
		xmlTextWriter.Close();
		return stringBuilder.ToString();
	}

	public void Serialize(XmlWriter xmlWriter)
	{
		xmlWriter.WriteStartElement("Item");
		xmlWriter.WriteElementString("Name", Name);
		FQID.Serialize(xmlWriter);
		if (Properties != null && Properties.Count > 0)
		{
			xmlWriter.WriteStartElement("properties");
			foreach (string key in Properties.Keys)
			{
				xmlWriter.WriteStartElement("property");
				xmlWriter.WriteAttributeString("name", key);
				xmlWriter.WriteAttributeString("value", Properties[key]);
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteEndElement();
		}
		xmlWriter.WriteEndElement();
	}

	public static Item Deserialize(string serializedItem)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(serializedItem);
		return ConfigurationUtil.DeserializeItem(xmlDocument.SelectSingleNode("Item"));
	}
}
