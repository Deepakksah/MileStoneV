using System.Text;
using System.Xml;

namespace VideoOS.Utilities.OemResource;

public class OemResourceItemString : OemResourceItemBase
{
	public const string ResourceTypeStringConst = "System.String";

	public string ResourceValue
	{
		get
		{
			return Encoding.UTF8.GetString(_valueBytes);
		}
		set
		{
			_valueBytes = Encoding.UTF8.GetBytes(value);
		}
	}

	public override object ResourceValueAsObject
	{
		get
		{
			return ResourceValue;
		}
		set
		{
			ResourceValue = (string)value;
		}
	}

	public override string ResourceTypeString => "System.String";

	public OemResourceItemString()
	{
	}

	public OemResourceItemString(XmlNode oemResourceItemNode)
		: base(oemResourceItemNode)
	{
	}
}
