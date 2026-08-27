using System.Collections;

namespace VideoOS.Rules.Adapters.XmlHelper;

public class ActionParameters
{
	private Hashtable parameters;

	public string this[string parameterName]
	{
		get
		{
			string text = (string)parameters[parameterName];
			if (text != null)
			{
				return text;
			}
			return "";
		}
	}

	public ActionParameters(Hashtable parameters)
	{
		this.parameters = parameters;
	}
}
