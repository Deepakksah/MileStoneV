using System.Text;
using System.Xml;

namespace VideoOS.Utilities.OemResource;

public class OemResourceItemVectorImage : OemResourceItemBase
{
	public const string ResourceTypeStringConst = "VectorGraphicsResource";

	public VectorGraphicsResource ResourceValue
	{
		get
		{
			return new VectorGraphicsResource(Encoding.UTF8.GetString(_valueBytes));
		}
		set
		{
			_valueBytes = Encoding.UTF8.GetBytes(value?.Value);
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
			ResourceValue = (VectorGraphicsResource)value;
		}
	}

	public override string ResourceTypeString => "VectorGraphicsResource";

	public OemResourceItemVectorImage()
	{
	}

	public OemResourceItemVectorImage(XmlNode oemResourceItemNode)
		: base(oemResourceItemNode)
	{
	}
}
