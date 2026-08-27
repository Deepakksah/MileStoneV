using System;

namespace VideoOS.EventServer.Shared.Aggregation;

public class Result<S, T>
{
	private bool _hasValue;

	private T _value;

	private bool _hasException;

	private Exception _exception;

	public S Source { get; set; }

	public T Value
	{
		get
		{
			return _value;
		}
		set
		{
			if (_hasException)
			{
				throw new ArgumentException("Cannot have value and exception in same Result object");
			}
			_value = value;
			_hasValue = true;
		}
	}

	public bool HasValue => _hasValue;

	public Exception Exception
	{
		get
		{
			return _exception;
		}
		set
		{
			if (_hasValue)
			{
				throw new ArgumentException("Cannot have value and exception in same Result object");
			}
			_exception = value;
			_hasException = true;
		}
	}

	public bool HasException => _hasException;
}
