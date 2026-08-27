using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VideoOS.DataCollector.Service.Common.ResultResponse;

public readonly record struct Result<TValue> : IResult
{
	public bool IsError { get; }

	public TValue Value { get; }

	public List<Error>? Errors
	{
		get
		{
			if (!IsError)
			{
				return new List<Error> { Error.Failure("No errors was set") };
			}
			return _errors;
		}
	}

	private readonly List<Error>? _errors;

	public static Result<TValue> From(List<Error> errors)
	{
		return errors;
	}

	public Result(TValue value)
	{
		_errors = null;
		Value = default(TValue);
		Value = value;
		IsError = false;
	}

	private Result(Error error)
	{
		_errors = null;
		Value = default(TValue);
		_errors = new List<Error> { error };
		IsError = true;
	}

	private Result(List<Error> errors)
	{
		_errors = null;
		Value = default(TValue);
		_errors = errors;
		IsError = true;
	}

	public static implicit operator Result<TValue>(TValue value)
	{
		return new Result<TValue>(value);
	}

	public static implicit operator Result<TValue>(Error error)
	{
		return new Result<TValue>(error);
	}

	public static implicit operator Result<TValue>(List<Error> errors)
	{
		return new Result<TValue>(errors);
	}

	[CompilerGenerated]
	public override int GetHashCode()
	{
		return (EqualityComparer<List<Error>>.Default.GetHashCode(_errors) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(IsError)) * -1521134295 + EqualityComparer<TValue>.Default.GetHashCode(Value);
	}

	[CompilerGenerated]
	public bool Equals(Result<TValue> other)
	{
		if (EqualityComparer<List<Error>>.Default.Equals(_errors, other._errors) && EqualityComparer<bool>.Default.Equals(IsError, other.IsError))
		{
			return EqualityComparer<TValue>.Default.Equals(Value, other.Value);
		}
		return false;
	}
}
