using System.Text.RegularExpressions;

namespace VideoOS.WebService.Common.SecurityService;

public static class StringExtensions
{
	private static Regex re;

	private static Regex RegexInstance
	{
		get
		{
			if (re == null)
			{
				re = new Regex("^[A-Fa-f0-9]{32}$|^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$", RegexOptions.Compiled);
			}
			return re;
		}
	}

	public static bool IsGuid(this string input)
	{
		if (string.IsNullOrEmpty(input) || !RegexInstance.IsMatch(input))
		{
			return false;
		}
		return true;
	}
}
