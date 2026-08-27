using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Definitions;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class RuleParametersValidator : IParametersValidator
{
	private static readonly Guid FromTriggerId = new Guid("00000000-0000-0000-0000-000000000000");

	private static readonly Guid FromMetadataId = new Guid("11111111-1111-1111-1111-111111111111");

	private static readonly HashSet<Guid> ReservedGuids = new HashSet<Guid> { FromMetadataId, FromTriggerId };

	private readonly IValueValidatorFactory _validatorFactory;

	public RuleParametersValidator(IValueValidatorFactory validatorFactory)
	{
		_validatorFactory = validatorFactory;
	}

	public ValidationError[] Validate(IGeminiRequestContext requestContext, IDictionary<string, RuleParameter> parameters, IDictionary<string, RuleParameterDefinition> paramDefs)
	{
		if (parameters == null)
		{
			return new ValidationError[1]
			{
				new ValidationError("Parameters cannot be null!")
			};
		}
		paramDefs.NullCheck("", "Validate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\Backend\\Validation\\Common\\RuleParametersValidator.cs", 33);
		List<ValidationError> list = new List<ValidationError>(parameters.Count);
		foreach (RuleParameterDefinition value in paramDefs.Values)
		{
			if (!IsAbsentParameter(parameters, value, list) && !IsObligatoryValueAbsent(parameters, value, list) && !IsWrongParameterType(parameters, value, list))
			{
				IsValueInvalid(requestContext, parameters, value, list);
			}
		}
		return list.ToArray();
	}

	private static string PropertyName(RuleParameterDefinition paramDef)
	{
		if (!string.IsNullOrEmpty(paramDef.PropertyName))
		{
			return paramDef.PropertyName;
		}
		return paramDef.Name;
	}

	private static bool IsAbsentParameter(IDictionary<string, RuleParameter> parameters, RuleParameterDefinition paramDef, List<ValidationError> errors)
	{
		if (!paramDef.ExcludeFromDatabase && !TryGetParameter(parameters, paramDef, out var _))
		{
			errors.Add(new ValidationError("Missing required parameter: {0}", paramDef.Name)
			{
				PropertyName = PropertyName(paramDef)
			});
			return true;
		}
		return false;
	}

	private static bool TryGetParameter(IDictionary<string, RuleParameter> parameters, RuleParameterDefinition paramDef, out RuleParameter parameter)
	{
		if (!parameters.TryGetValue(paramDef.Name, out parameter))
		{
			if (!string.IsNullOrEmpty(paramDef.XmlName))
			{
				return parameters.TryGetValue(paramDef.XmlName, out parameter);
			}
			return false;
		}
		return true;
	}

	private static bool IsObligatoryValueAbsent(IDictionary<string, RuleParameter> parameters, RuleParameterDefinition paramDef, List<ValidationError> errors)
	{
		if (!TryGetParameter(parameters, paramDef, out var parameter))
		{
			return !paramDef.ExcludeFromDatabase;
		}
		if (!parameter.IsEmpty)
		{
			return false;
		}
		if (!paramDef.EmptyAllowed)
		{
			errors.Add(new ValidationError("At least one value required for parameter {0} of type {1}", paramDef.Name, paramDef.ValueType)
			{
				PropertyName = PropertyName(paramDef)
			});
			return true;
		}
		if (ShouldValidatePairedParameter(paramDef) && parameters[paramDef.PairedParameter].IsEmpty)
		{
			errors.Add(new ValidationError("At least one of parameters {0} and {1} must have at least one value", paramDef.Name, paramDef.PairedParameter)
			{
				PropertyName = PropertyName(paramDef)
			});
			return true;
		}
		return false;
	}

	private static bool ShouldValidatePairedParameter(RuleParameterDefinition paramDef)
	{
		if (string.IsNullOrEmpty(paramDef.PairedParameter))
		{
			return false;
		}
		return string.Equals(paramDef.Name, new string[2] { paramDef.Name, paramDef.PairedParameter }.OrderByDescending((string s) => s).First(), StringComparison.Ordinal);
	}

	private static bool IsWrongParameterType(IDictionary<string, RuleParameter> parameters, RuleParameterDefinition paramDef, List<ValidationError> errors)
	{
		if (!TryGetParameter(parameters, paramDef, out var parameter))
		{
			return !paramDef.ExcludeFromDatabase;
		}
		if (paramDef.EmptyAllowed && parameter.IsEmpty)
		{
			return false;
		}
		if (!paramDef.ValueType.IsArray)
		{
			if (!CheckParameterType(paramDef, parameter, parameter.Value, paramDef.ValueType, isPartOfArray: false, errors))
			{
				return true;
			}
		}
		else
		{
			Type valueType = paramDef.ValueType.GetElementType();
			if (parameter.Values.Any((string value) => !CheckParameterType(paramDef, parameter, value, valueType, isPartOfArray: true, errors)))
			{
				return true;
			}
		}
		return false;
	}

	private bool IsValueInvalid(IGeminiRequestContext requestContext, IDictionary<string, RuleParameter> parameters, RuleParameterDefinition paramDef, List<ValidationError> errors)
	{
		if (!TryGetParameter(parameters, paramDef, out var parameter))
		{
			return !paramDef.ExcludeFromDatabase;
		}
		if (paramDef.MinValue != null && paramDef.MaxValue != null && IsComparableValueType(paramDef.ValueType) && (paramDef.ValueType.IsArray ? CheckArrayValueBounds(parameter, paramDef, errors) : CheckValueBounds(parameter.Value, paramDef, paramDef.ValueType, errors)))
		{
			return false;
		}
		if (paramDef.AllowedKeyValuePairs != null && paramDef.AllowedKeyValuePairs.Any() && parameter.Values.Any((string value) => !paramDef.AllowedKeyValuePairs.Keys.Contains(value)))
		{
			errors.Add(new ValidationError("Invalid content of parameter: {0}", paramDef.Name)
			{
				PropertyName = PropertyName(paramDef)
			});
			return true;
		}
		if (paramDef.ValueType == typeof(Guid) || paramDef.ValueType == typeof(Guid[]))
		{
			return CheckGuidValuesAgainstDb(requestContext, parameter, paramDef, parameters, errors);
		}
		if (paramDef.ValidatorType != null)
		{
			IValueValidator validator = _validatorFactory.GetValidator(paramDef.ValidatorType);
			if (validator != null && parameter.Values.Any((string value) => !validator.Validate(requestContext, value, null, parameters)))
			{
				errors.Add(new ValidationError("Invalid content of parameter: {0}", paramDef.Name)
				{
					PropertyName = PropertyName(paramDef)
				});
				return true;
			}
		}
		return false;
	}

	private static bool IsComparableValueType(Type type)
	{
		Type type2 = (type.IsArray ? type.GetElementType() : type);
		if (!type2.IsValueType)
		{
			return false;
		}
		return typeof(IComparable).IsAssignableFrom(type2);
	}

	private bool CheckGuidValuesAgainstDb(IGeminiRequestContext requestContext, RuleParameter parameter, RuleParameterDefinition paramDef, IDictionary<string, RuleParameter> parameters, List<ValidationError> errors)
	{
		if (paramDef.ValidatorType == null)
		{
			return false;
		}
		IValueValidator validator = _validatorFactory.GetValidator(paramDef.ValidatorType);
		if (validator == null)
		{
			return false;
		}
		if (parameter.IsEmpty)
		{
			return false;
		}
		if (parameter.Values.Any(string.IsNullOrWhiteSpace))
		{
			errors.Add(new ValidationError("Guids cannot be empty string in parameter: {0}", paramDef.Name)
			{
				PropertyName = PropertyName(paramDef)
			});
			return true;
		}
		if (paramDef.AllowedMIPKinds.Any())
		{
			return CheckGuidValuesAgainstMIPKinds(requestContext, parameter, paramDef, parameters, validator, errors);
		}
		if (paramDef.AllowedItemTypes.Any() && CheckGuidValuesAgainstAllowedItemTypes(requestContext, parameter, paramDef, parameters, validator, errors))
		{
			return true;
		}
		ValidationError[] array = (from s in parameter.Values
			select new Guid(s) into guid
			where !ReservedGuids.Contains(guid) && !validator.Validate(requestContext, guid, validator.GetItemType(requestContext, guid.ToString(), parameters), parameters)
			select new ValidationError("Invalid value for {0} parameter. Object with id {1} does not exist in the database", paramDef.Name, guid)
			{
				PropertyName = PropertyName(paramDef)
			}).ToArray();
		errors.AddRange(array);
		return array.Any();
	}

	private static bool CheckGuidValuesAgainstMIPKinds(IGeminiRequestContext requestContext, RuleParameter parameter, RuleParameterDefinition paramDef, IDictionary<string, RuleParameter> parameters, IValueValidator validator, List<ValidationError> errors)
	{
		return CheckInvalidGuids((from s in parameter.Values
			select new Guid(s) into guid
			where !ReservedGuids.Contains(guid) && !paramDef.AllowedMIPKinds.Any((Guid kind) => validator.Validate(requestContext, guid, kind.ToString(), parameters))
			select guid).ToList(), paramDef, errors);
	}

	private static bool CheckGuidValuesAgainstAllowedItemTypes(IGeminiRequestContext requestContext, RuleParameter parameter, RuleParameterDefinition paramDef, IDictionary<string, RuleParameter> parameters, IValueValidator validator, List<ValidationError> errors)
	{
		try
		{
			return CheckInvalidGuids((from s in parameter.Values
				where validator.GetItemType(requestContext, s, parameters) != null && !paramDef.AllowedItemTypes.Contains(validator.GetItemType(requestContext, s, parameters))
				select new Guid(s)).ToList(), paramDef, errors);
		}
		catch (Exception ex)
		{
			errors.Add(new ValidationError(ex.Message)
			{
				PropertyName = PropertyName(paramDef)
			});
			return true;
		}
	}

	private static bool CheckInvalidGuids(List<Guid> invalidGuids, RuleParameterDefinition paramDef, List<ValidationError> errors)
	{
		if (!invalidGuids.Any())
		{
			return false;
		}
		errors.AddRange(invalidGuids.Select((Guid guid) => new ValidationError("Object with id {0} does not match allowed item types of parameter {1}", guid, paramDef.Name)
		{
			PropertyName = PropertyName(paramDef)
		}).ToArray());
		return true;
	}

	private static bool CheckArrayValueBounds(RuleParameter parameter, RuleParameterDefinition paramDef, List<ValidationError> errors)
	{
		return !parameter.Values.All((string s) => !CheckValueBounds(s, paramDef, paramDef.ValueType.GetElementType(), errors));
	}

	private static bool CheckValueBounds(string valueString, RuleParameterDefinition paramDef, Type valueType, List<ValidationError> errors)
	{
		if (string.IsNullOrEmpty(valueString))
		{
			return false;
		}
		return InternalCheckValue((IComparable)TypeDescriptor.GetConverter(valueType).ConvertFromString(null, CultureInfo.InvariantCulture, valueString), paramDef.MinValue, paramDef.MaxValue, paramDef, errors);
	}

	private static bool InternalCheckValue(IComparable value, IComparable minValue, IComparable maxValue, RuleParameterDefinition paramDef, List<ValidationError> errors)
	{
		if (minValue.CompareTo(value) > 0 || maxValue.CompareTo(value) < 0)
		{
			errors.Add(new ValidationError("Value {0} of parameter {1} is not between {2} and {3}", value, paramDef.Name, minValue, maxValue)
			{
				PropertyName = PropertyName(paramDef)
			});
			return true;
		}
		return false;
	}

	private static bool CheckParameterType(RuleParameterDefinition paramDef, RuleParameter parameter, string value, Type expectedType, bool isPartOfArray, List<ValidationError> errors)
	{
		try
		{
			TypeDescriptor.GetConverter(expectedType).ConvertFromString(null, CultureInfo.InvariantCulture, value);
		}
		catch (Exception)
		{
			FormErrorMessage(paramDef, parameter, value, expectedType, isPartOfArray, errors);
			return false;
		}
		return true;
	}

	private static void FormErrorMessage(RuleParameterDefinition paramDef, RuleParameter parameter, string value, Type expectedType, bool isPartOfArray, List<ValidationError> errors)
	{
		string text = (isPartOfArray ? (" of array: " + string.Join(",", parameter.Values)) : string.Empty);
		errors.Add(new ValidationError("Invalid type of parameter {0} => {1}{2}; Expected to be {3}", parameter.Name, value, text, GetExpectedTypeString(expectedType))
		{
			PropertyName = PropertyName(paramDef)
		});
	}

	private static string GetExpectedTypeString(Type expectedType)
	{
		if (!expectedType.IsEnum)
		{
			return string.Format(CultureInfo.InvariantCulture, "of type: {0}", expectedType.Name);
		}
		return string.Format(CultureInfo.InvariantCulture, "of values: {0}", string.Join(",", Enum.GetValues(expectedType).Cast<object>()));
	}
}
