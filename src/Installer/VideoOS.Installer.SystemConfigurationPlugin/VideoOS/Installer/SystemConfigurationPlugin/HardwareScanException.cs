using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace VideoOS.Installer.SystemConfigurationPlugin;

[Serializable]
public class HardwareScanException : Exception
{
	private ScanErrorCode _errorCode = ScanErrorCode.Generic;

	public ScanErrorCode ErrorCode => _errorCode;

	public HardwareScanException()
	{
	}

	public HardwareScanException(ScanErrorCode errorCode)
	{
		_errorCode = errorCode;
	}

	public HardwareScanException(string message)
		: base(message)
	{
	}

	public HardwareScanException(ScanErrorCode errorCode, string message)
		: base(message)
	{
		_errorCode = errorCode;
	}

	public HardwareScanException(string message, Exception inner)
		: base(message, inner)
	{
	}

	public HardwareScanException(ScanErrorCode errorCode, string message, Exception inner)
		: base(message, inner)
	{
		_errorCode = errorCode;
	}

	protected HardwareScanException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		base.GetObjectData(info, context);
		info.AddValue("ErrorCode", _errorCode);
	}
}
