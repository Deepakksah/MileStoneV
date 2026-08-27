using System;

namespace VideoOS.Server;

[AttributeUsage(AttributeTargets.Constructor, Inherited = false, AllowMultiple = true)]
public sealed class SqlErrorAttribute : Attribute
{
	private int _errorCode;

	public int ErrorCode => _errorCode;

	public SqlErrorAttribute(int errorCode)
	{
		_errorCode = errorCode;
	}
}
