using System.Text;
using System.Xml;

namespace VideoOS.Platform.License.OemResources;

public class OemResourceItemString : OemResourceItemBase
{
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

	public override string ResourceTypeString => typeof(string).ToString();

	public OemResourceItemString()
	{
	}

	public OemResourceItemString(XmlNode oemResourceItemNode)
		: base(oemResourceItemNode)
	{
	}
}
