using System.Collections.Generic;
using System.Xml.Serialization;
using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.SDK.Portable.Management;

[XmlRoot(ElementName = "ConfigurationEntry")]
public class ConfigurationEntry
{
	public const string NameXmlNode = "Name";

	public const string ParametersListXmlNode = "ParametersList";

	public const string ParameterXmlNode = "Parameter";

	private Dictionary<string, string> _params;

	[XmlArray(ElementName = "ParametersList")]
	[XmlArrayItem(ElementName = "Parameter")]
	public List<Parameter> Parameters { get; set; }

	[XmlElement(ElementName = "Name")]
	public string Name { get; set; }

	public ConfigurationEntry()
	{
		Parameters = new List<Parameter>();
	}

	public Dictionary<string, string> ToDictionary()
	{
		if (_params == null)
		{
			_params = new Dictionary<string, string>();
			foreach (Parameter parameter in Parameters)
			{
				_params[parameter.ParamName] = parameter.ParamValue;
			}
		}
		return _params;
	}

	public List<Parameter> FromDictionary(Dictionary<string, string> parameters)
	{
		_params = parameters;
		Parameters = new List<Parameter>();
		if (parameters != null)
		{
			foreach (KeyValuePair<string, string> parameter in parameters)
			{
				Parameters.Add(new Parameter(parameter.Key, parameter.Value));
			}
		}
		return Parameters;
	}
}
