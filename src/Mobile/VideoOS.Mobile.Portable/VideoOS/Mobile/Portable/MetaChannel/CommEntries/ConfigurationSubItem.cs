using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class ConfigurationSubItem
{
	public const string IdName = "ID";

	public const string NameName = "Name";

	private Guid _id = Guid.Empty;

	[JsonProperty("ID")]
	[XmlAttribute("ID")]
	public string ID
	{
		get
		{
			if (_id == Guid.Empty)
			{
				return null;
			}
			return _id.ToString();
		}
		set
		{
			try
			{
				_id = new Guid(value);
			}
			catch (Exception)
			{
				_id = Guid.Empty;
			}
		}
	}

	[JsonProperty("Name")]
	[XmlAttribute("Name")]
	public string Name { get; set; }

	public void SetID(Guid id)
	{
		_id = id;
	}

	public Guid GetId()
	{
		return _id;
	}
}
