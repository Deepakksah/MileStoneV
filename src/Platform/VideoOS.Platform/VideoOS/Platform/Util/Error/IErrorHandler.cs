using System;

namespace VideoOS.Platform.Util.Error;

public interface IErrorHandler<out T> where T : Exception
{
	T LastError { get; }

	event Action<T> OnError;
}
