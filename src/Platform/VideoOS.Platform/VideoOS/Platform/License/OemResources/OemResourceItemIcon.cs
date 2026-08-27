using System.Drawing;
using System.IO;
using System.Xml;

namespace VideoOS.Platform.License.OemResources;

public class OemResourceItemIcon : OemResourceItemBase
{
	public Icon ResourceValue
	{
		get
		{
			return new Icon(new MemoryStream(_valueBytes));
		}
		set
		{
			MemoryStream memoryStream = new MemoryStream();
			value.Save(memoryStream);
			_valueBytes = memoryStream.ToArray();
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
			ResourceValue = (Icon)value;
		}
	}

	public override string ResourceTypeString => typeof(Icon).ToString();

	public OemResourceItemIcon()
	{
	}

	public OemResourceItemIcon(XmlNode oemResourceItemNode)
		: base(oemResourceItemNode)
	{
	}
}
