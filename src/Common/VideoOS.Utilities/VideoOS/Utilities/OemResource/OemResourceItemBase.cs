using System;
using System.Globalization;
using System.Text;
using System.Xml;

namespace VideoOS.Utilities.OemResource;

public abstract class OemResourceItemBase
{
	protected string _name = "";

	protected bool _locked;

	protected string _description = "";

	protected byte[] _valueBytes = new byte[0];

	public string Name
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

	public bool Locked
	{
		get
		{
			return _locked;
		}
		set
		{
			_locked = value;
		}
	}

	public string Description
	{
		get
		{
			return _description;
		}
		set
		{
			_description = value;
		}
	}

	public abstract object ResourceValueAsObject { get; set; }

	public abstract string ResourceTypeString { get; }

	public OemResourceItemBase()
	{
	}

	public OemResourceItemBase(XmlNode oemResourceItemNode)
	{
		_name = oemResourceItemNode.Attributes["Name"].InnerText;
		_locked = Convert.ToBoolean(oemResourceItemNode.Attributes["Locked"].InnerText, CultureInfo.InvariantCulture);
		XmlNode xmlNode = oemResourceItemNode.SelectSingleNode("Description");
		_description = Encoding.UTF8.GetString(Convert.FromBase64String(xmlNode.InnerText));
		XmlNode xmlNode2 = oemResourceItemNode.SelectSingleNode("Value");
		_valueBytes = Convert.FromBase64String(xmlNode2.InnerText);
	}

	public virtual void Serialize(XmlTextWriter xmlTextWriter)
	{
		xmlTextWriter.WriteAttributeString("Type", ResourceTypeString);
		xmlTextWriter.WriteAttributeString("Name", _name);
		xmlTextWriter.WriteAttributeString("Locked", Convert.ToString(_locked, CultureInfo.InvariantCulture));
		xmlTextWriter.WriteElementString("Description", Convert.ToBase64String(Encoding.UTF8.GetBytes(_description)));
		xmlTextWriter.WriteElementString("Value", Convert.ToBase64String(_valueBytes));
	}

	public static OemResourceItemBase GenerateResourceItem(XmlNode oemResourceItemNode)
	{
		OemResourceItemBase result = null;
		switch (oemResourceItemNode.Attributes["Type"].InnerText)
		{
		case "System.String":
			result = new OemResourceItemString(oemResourceItemNode);
			break;
		case "System.String (RTF)":
			result = new OemResourceItemRichText(oemResourceItemNode);
			break;
		case "System.Drawing.Image":
			result = new OemResourceItemImage(oemResourceItemNode);
			break;
		case "System.Drawing.Icon":
			result = new OemResourceItemIcon(oemResourceItemNode);
			break;
		case "System.Int32":
			result = new OemResourceItemInteger(oemResourceItemNode);
			break;
		case "VectorGraphicsResource":
			result = new OemResourceItemVectorImage(oemResourceItemNode);
			break;
		}
		return result;
	}
}
