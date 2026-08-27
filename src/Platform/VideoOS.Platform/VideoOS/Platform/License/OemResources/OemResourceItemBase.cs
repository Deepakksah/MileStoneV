using System;
using System.Globalization;
using System.Text;
using System.Xml;

namespace VideoOS.Platform.License.OemResources;

public abstract class OemResourceItemBase
{
	protected string _name = "";

	protected bool _locked;

	protected string _description = "";

	protected byte[] _valueBytes = Array.Empty<byte>();

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
		string innerText = oemResourceItemNode.Attributes["Type"].InnerText;
		if (innerText == new OemResourceItemString().ResourceTypeString)
		{
			result = new OemResourceItemString(oemResourceItemNode);
		}
		else if (innerText == new OemResourceItemRichText().ResourceTypeString)
		{
			result = new OemResourceItemRichText(oemResourceItemNode);
		}
		else if (innerText == new OemResourceItemIcon().ResourceTypeString)
		{
			result = new OemResourceItemIcon(oemResourceItemNode);
		}
		else if (innerText == new OemResourceItemInteger().ResourceTypeString)
		{
			result = new OemResourceItemInteger(oemResourceItemNode);
		}
		return result;
	}
}
