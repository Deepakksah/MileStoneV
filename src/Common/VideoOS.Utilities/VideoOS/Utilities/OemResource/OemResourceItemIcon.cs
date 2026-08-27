using System.Drawing;
using System.IO;
using System.Xml;

namespace VideoOS.Utilities.OemResource;

public class OemResourceItemIcon : OemResourceItemBase
{
	public const string ResourceTypeStringConst = "System.Drawing.Icon";

	public Icon ResourceValue
	{
		get
		{
			return new Icon(new MemoryStream(_valueBytes));
		}
		set
		{
			using MemoryStream memoryStream = new MemoryStream();
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

	public override string ResourceTypeString => "System.Drawing.Icon";

	public OemResourceItemIcon()
	{
	}

	public OemResourceItemIcon(XmlNode oemResourceItemNode)
		: base(oemResourceItemNode)
	{
	}
}
