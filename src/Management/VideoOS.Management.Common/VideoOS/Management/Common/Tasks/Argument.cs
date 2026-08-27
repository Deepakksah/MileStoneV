using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace VideoOS.Management.Common.Tasks;

public class Argument
{
	[XmlAttribute("Index", DataType = "int")]
	public int Index { get; set; }

	[DefaultValue("")]
	[XmlAttribute("ResourceId", DataType = "string")]
	public string ResourceId { get; set; }

	[DefaultValue("")]
	[XmlElement("Value", IsNullable = false, DataType = "string")]
	public string Value { get; set; }

	public Argument()
	{
		Value = string.Empty;
	}

	public Argument(int index, string value)
		: this(index, value, null)
	{
	}

	public Argument(int index, string value, string resourceId)
	{
		if (index < 0)
		{
			throw new ArgumentException("The argument must be 0 or greater.", "index");
		}
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		Index = index;
		Value = value;
		ResourceId = resourceId;
	}
}
