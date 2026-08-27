using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace VideoOS.ServerConfigurator.Interfaces;

public class RegistrationResult
{
	private static Dictionary<Type, IExtendedResult> _knownResultsTypes = new Dictionary<Type, IExtendedResult>
	{
		{
			typeof(ConsistencyStatus),
			new ExtendedResult<ConsistencyStatus>((ConsistencyStatus x) => x.ResultType())
		},
		{
			typeof(RegisterServiceStatus),
			new ExtendedResult<RegisterServiceStatus>((RegisterServiceStatus x) => x.ResultType())
		},
		{
			typeof(RegisterClientStatus),
			new ExtendedResult<RegisterClientStatus>((RegisterClientStatus x) => x.ResultType())
		},
		{
			typeof(SetServerAddressStatus),
			new ExtendedResult<SetServerAddressStatus>((SetServerAddressStatus x) => x.ResultType())
		},
		{
			typeof(ApplyStatus),
			new ExtendedResult<ApplyStatus>((ApplyStatus x) => x.ResultType())
		},
		{
			typeof(SystemConfigurationStatus),
			new ExtendedResult<SystemConfigurationStatus>((SystemConfigurationStatus x) => x.ResultType())
		},
		{
			typeof(SetSqlServerStatus),
			new ExtendedResult<SetSqlServerStatus>((SetSqlServerStatus x) => x.ResultType())
		}
	};

	public RegistrationResultType ResultType
	{
		get
		{
			if (Results.Any((KeyValuePair<Type, object> x) => _knownResultsTypes[x.Key].ResultType(x.Value) == RegistrationResultType.Error))
			{
				return RegistrationResultType.Error;
			}
			if (Results.Any((KeyValuePair<Type, object> x) => _knownResultsTypes[x.Key].ResultType(x.Value) == RegistrationResultType.Warning))
			{
				return RegistrationResultType.Warning;
			}
			return RegistrationResultType.Success;
		}
	}

	public Dictionary<Type, object> Results { get; }

	public RegistrationResult(Dictionary<Type, object> results)
	{
		Results = results;
	}

	public RegistrationResult(object result)
	{
		if (result == null)
		{
			throw new ArgumentNullException("result");
		}
		Results = new Dictionary<Type, object> { 
		{
			result.GetType(),
			result
		} };
	}

	public T? GetResult<T>() where T : struct
	{
		if (!Results.ContainsKey(typeof(T)))
		{
			return null;
		}
		return (T?)Results[typeof(T)];
	}

	public int GetNumericResult()
	{
		int num = 0;
		int num2 = 1;
		Dictionary<Type, object> dictionary = new Dictionary<Type, object>(Results);
		foreach (IExtendedResult value in _knownResultsTypes.Values)
		{
			Type type = value.Type;
			int num3 = 0;
			if (dictionary.ContainsKey(type))
			{
				num3 = (int)dictionary[type];
				dictionary.Remove(type);
			}
			num += num3 * num2;
			num2 *= GetFactor(type);
		}
		if (dictionary.Any())
		{
			throw new InvalidOperationException(FormattableString.Invariant(FormattableStringFactory.Create("One or more results are not in the list '{0}'", "_knownResultsTypes")));
		}
		return num;
	}

	private static int GetFactor(Type type)
	{
		if (Enum.GetValues(type).Length >= 10)
		{
			return 100;
		}
		return 10;
	}

	public static RegistrationResult CreateRegistrationResult(int numericResult)
	{
		RegistrationResult registrationResult = new RegistrationResult(new Dictionary<Type, object>());
		int num = numericResult;
		foreach (IExtendedResult value2 in _knownResultsTypes.Values)
		{
			Type type = value2.Type;
			int factor = GetFactor(type);
			int value = num % factor;
			registrationResult.Results[type] = Enum.ToObject(type, value);
			num /= factor;
		}
		return registrationResult;
	}

	public override string ToString()
	{
		if (ResultType == RegistrationResultType.Success)
		{
			return "Success";
		}
		if (Results != null)
		{
			return FormattableString.Invariant(FormattableStringFactory.Create("{{{0}}}", string.Join(", ", Results.Select((KeyValuePair<Type, object> x) => FormattableString.Invariant($"{x.Key.Name}.{x.Value}")))));
		}
		return "Not Initialized";
	}

	public void Concat(RegistrationResult otherResult)
	{
		if (otherResult == null || otherResult.Results.Count == 0)
		{
			return;
		}
		foreach (KeyValuePair<Type, object> result in otherResult.Results)
		{
			Results.Add(result.Key, result.Value);
		}
	}
}
