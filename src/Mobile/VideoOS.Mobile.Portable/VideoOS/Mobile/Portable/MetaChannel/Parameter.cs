using System.Xml.Serialization;
using Newtonsoft.Json;

namespace VideoOS.Mobile.Portable.MetaChannel;

public class Parameter
{
	private const string ParamNameXmlNode = "Name";

	private const string ParamValueXmlNode = "Value";

	[JsonProperty("Name")]
	[XmlAttribute(AttributeName = "Name", DataType = "string")]
	public string ParamName { get; set; }

	[JsonProperty("Value")]
	[XmlAttribute(AttributeName = "Value", DataType = "string")]
	public string ParamValue { get; set; }

	[XmlText]
	public string Optional { get; set; }

	public Parameter()
	{
	}

	public Parameter(string name, string value)
	{
		ParamName = name;
		ParamValue = value;
	}
}
