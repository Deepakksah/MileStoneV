using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace VideoOS.Management.Common;

public static class NestedPropertyHelper
{
	private const string NestedPropertyRegularExpression = "^\\$\\((?<PropertyName>\\S.*)\\)$";

	private const string PropertyNameRegularExpression = "^(?<PropertyName>\\S.*)$";

	public static string FormatNestedPropertyName(string propertyName)
	{
		if (propertyName == null)
		{
			throw new ArgumentNullException("propertyName");
		}
		if (string.IsNullOrWhiteSpace(propertyName))
		{
			throw new ArgumentException("Argument 'propertyName' cannot be empty", "propertyName");
		}
		if (!Regex.IsMatch(propertyName, "^(?<PropertyName>\\S.*)$", RegexOptions.Compiled))
		{
			throw new ArgumentException("Argument 'propertyName' cannot is not valid", "propertyName");
		}
		if (char.IsWhiteSpace(propertyName, propertyName.Length - 1))
		{
			throw new ArgumentException("Argument 'propertyName' cannot end with a white-space char", "propertyName");
		}
		if (propertyName.StartsWith("$", StringComparison.Ordinal))
		{
			throw new ArgumentException("Argument 'propertyName' cannot start with a '$'", "propertyName");
		}
		return string.Format(CultureInfo.InvariantCulture, "$({0})", propertyName);
	}

	public static bool TryGetNestedPropertyName(string formattedPropertyName, out string propertyName)
	{
		if (formattedPropertyName == null)
		{
			throw new ArgumentNullException("formattedPropertyName");
		}
		if (string.IsNullOrWhiteSpace(formattedPropertyName))
		{
			throw new ArgumentException("Argument 'formattedPropertyName' cannot be empty", "formattedPropertyName");
		}
		propertyName = null;
		Match match = Regex.Match(formattedPropertyName, "^\\$\\((?<PropertyName>\\S.*)\\)$", RegexOptions.Compiled);
		if (match.Success)
		{
			propertyName = match.Groups["PropertyName"].Value;
			return true;
		}
		return false;
	}

	public static bool IsNestedPropertyName(string propertyName)
	{
		if (propertyName == null)
		{
			throw new ArgumentNullException("propertyName");
		}
		if (string.IsNullOrWhiteSpace(propertyName))
		{
			throw new ArgumentException("Argument 'propertyName' cannot be empty", "propertyName");
		}
		return Regex.IsMatch(propertyName, "^\\$\\((?<PropertyName>\\S.*)\\)$", RegexOptions.Compiled);
	}
}
