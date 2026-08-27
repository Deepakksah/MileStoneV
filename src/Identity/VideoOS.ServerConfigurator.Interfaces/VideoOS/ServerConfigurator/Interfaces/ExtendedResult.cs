using System;

namespace VideoOS.ServerConfigurator.Interfaces;

public class ExtendedResult<T> : IExtendedResult where T : struct
{
	private readonly Func<T, RegistrationResultType> _resultType;

	Type IExtendedResult.Type => typeof(T);

	public ExtendedResult(Func<T, RegistrationResultType> resultType)
	{
		_resultType = resultType;
	}

	RegistrationResultType IExtendedResult.ResultType(object value)
	{
		return _resultType((T)value);
	}
}
