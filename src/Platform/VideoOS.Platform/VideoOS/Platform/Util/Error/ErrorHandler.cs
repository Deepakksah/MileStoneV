using System;

namespace VideoOS.Platform.Util.Error;

public class ErrorHandler<T> : IErrorHandler<T> where T : Exception
{
	private T _lastError;

	public T LastError
	{
		get
		{
			return _lastError;
		}
		set
		{
			_lastError = value;
			if (_lastError != null)
			{
				this.OnError?.Invoke(_lastError);
			}
		}
	}

	public event Action<T> OnError;
}
