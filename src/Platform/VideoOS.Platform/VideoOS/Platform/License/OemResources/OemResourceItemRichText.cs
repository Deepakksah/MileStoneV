using System.Text;
using System.Xml;

namespace VideoOS.Platform.License.OemResources;

public class OemResourceItemRichText : OemResourceItemBase
{
	public string ResourceValue
	{
		get
		{
			return Encoding.Unicode.GetString(_valueBytes);
		}
		set
		{
			_valueBytes = Encoding.Unicode.GetBytes(value);
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

	public override string ResourceTypeString => "System.String (RTF)";

	public OemResourceItemRichText()
	{
	}

	public OemResourceItemRichText(XmlNode oemResourceItemNode)
		: base(oemResourceItemNode)
	{
	}
}
