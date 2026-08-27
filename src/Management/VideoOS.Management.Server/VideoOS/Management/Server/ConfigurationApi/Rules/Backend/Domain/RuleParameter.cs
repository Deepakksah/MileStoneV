using System.Linq;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

public class RuleParameter
{
	public string Name { get; set; }

	public string[] Values { get; set; } = new string[0];

	public string Value
	{
		get
		{
			if (Values.Length == 0)
			{
				return null;
			}
			return Values[0];
		}
		set
		{
			Values = ((value == null) ? new string[0] : new string[1] { value });
		}
	}

	public bool IsEmpty
	{
		get
		{
			if (Values.Any())
			{
				if (Values.Length == 1)
				{
					return string.IsNullOrWhiteSpace(Values[0]);
				}
				return false;
			}
			return true;
		}
	}

	public string ParameterToAppend { get; set; }

	public bool ExcludeFromDatabase { get; set; }

	public string XmlName { get; set; }
}
