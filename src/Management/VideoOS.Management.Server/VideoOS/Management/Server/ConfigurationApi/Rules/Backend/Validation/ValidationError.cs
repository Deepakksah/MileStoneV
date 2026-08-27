using System.Globalization;
using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class ValidationError
{
	public string Message { get; private set; }

	public string PropertyName { get; set; }

	public Property ValidProperty { get; set; }

	public ValidationError(string message, params object[] args)
	{
		Message = string.Format(CultureInfo.InvariantCulture, message, args);
	}

	public static ValidationError WithPrefix(string parentMessage, ValidationError error, string propertyName = null, Property validProperty = null)
	{
		return new ValidationError("{0}: {1}", parentMessage, error.Message)
		{
			PropertyName = propertyName,
			ValidProperty = validProperty
		};
	}
}
