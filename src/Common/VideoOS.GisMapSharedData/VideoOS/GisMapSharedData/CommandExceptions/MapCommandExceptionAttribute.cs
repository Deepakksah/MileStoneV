using System;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class MapCommandExceptionAttribute : Attribute
{
	private string _errorReason;

	public string ErrorReason => _errorReason;

	public MapCommandExceptionAttribute(string errorReason)
	{
		_errorReason = errorReason;
	}
}
