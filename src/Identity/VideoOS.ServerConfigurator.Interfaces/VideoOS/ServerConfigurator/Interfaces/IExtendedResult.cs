using System;

namespace VideoOS.ServerConfigurator.Interfaces;

public interface IExtendedResult
{
	Type Type { get; }

	RegistrationResultType ResultType(object value);
}
