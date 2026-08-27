using System;
using System.Runtime.Serialization;

namespace VideoOS.GisMapSharedData.CommandExceptions;

[Serializable]
[MapCommandException("Unknown")]
public class MapCommandException : Exception
{
	private string _errorReason;

	public string ErrorDescription => base.Message;

	public string ErrorReason => _errorReason;

	public MapCommandException()
		: base(MapCommandErrorReason.Unknown)
	{
		_errorReason = MapCommandErrorReason.Unknown;
	}

	public MapCommandException(string errorDescription)
		: base(errorDescription)
	{
		_errorReason = MapCommandErrorReason.Unknown;
	}

	public MapCommandException(string errorDescription, Exception innerException)
		: base(errorDescription, innerException)
	{
		_errorReason = MapCommandErrorReason.Unknown;
	}

	public MapCommandException(string errorDescription, string errorReason)
		: base(errorDescription)
	{
		_errorReason = errorReason;
	}

	public MapCommandException(string errorDescription, string errorReason, Exception innerException)
		: base(errorDescription, innerException)
	{
		_errorReason = errorReason;
	}

	protected MapCommandException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		if (info == null)
		{
			throw new ArgumentException("Argument is null", "info");
		}
		_errorReason = info.GetString("ErrorReason");
	}

	protected void AddData(string key, string value)
	{
		Data[key] = value;
	}

	protected bool TryGetData(string key, out string value)
	{
		value = null;
		if (Data.Contains(key))
		{
			value = (string)Data[key];
			return true;
		}
		return false;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentException("Argument is null", "info");
		}
		base.GetObjectData(info, context);
		info.AddValue("ErrorReason", _errorReason);
	}
}
