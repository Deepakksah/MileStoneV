using System;
using System.Globalization;
using System.Text;
using System.Xml;

namespace VideoOS.Utilities.OemResource;

public class OemResourceItemInteger : OemResourceItemBase
{
	public const string ResourceTypeStringConst = "System.Int32";

	public int ResourceValue
	{
		get
		{
			return Convert.ToInt32(Encoding.UTF8.GetString(_valueBytes), CultureInfo.InvariantCulture);
		}
		set
		{
			string s = Convert.ToString(value, CultureInfo.InvariantCulture);
			_valueBytes = Encoding.UTF8.GetBytes(s);
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
			ResourceValue = Convert.ToInt32(value, CultureInfo.InvariantCulture);
		}
	}

	public override string ResourceTypeString => "System.Int32";

	public OemResourceItemInteger()
	{
	}

	public OemResourceItemInteger(XmlNode oemResourceItemNode)
		: base(oemResourceItemNode)
	{
	}
}
