using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VideoOS.IdentityServer.Model.Result;

public record Result : IResult
{
	public bool IsError { get; }

	public IReadOnlyList<Error> Errors
	{
		get
		{
			if (!IsError)
			{
				return new List<Error>().AsReadOnly();
			}
			return _errors.AsReadOnly();
		}
	}

	private readonly List<Error> _errors;

	protected Result(List<Error> errors, bool isError)
	{
		_errors = (isError ? (errors ?? new List<Error>()) : null);
		IsError = isError;
	}

	public static Result From(List<Error> errors)
	{
		return errors;
	}

	public static Result Success()
	{
		return new Result(null, isError: false);
	}

	public static Result Failure(Error error)
	{
		int num = 1;
		List<Error> list = new List<Error>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<Error> span = CollectionsMarshal.AsSpan(list);
		int num2 = 0;
		span[num2] = error;
		num2++;
		return new Result(list, isError: true);
	}

	public static Result Failure(List<Error> errors)
	{
		if (errors == null)
		{
			throw new ArgumentNullException("errors");
		}
		return new Result(errors, isError: true);
	}

	public static implicit operator Result(Error error)
	{
		return Failure(error);
	}

	public static implicit operator Result(List<Error> errors)
	{
		return Failure(errors);
	}
}
public record Result<TValue> : Result
{
	public TValue Value { get; }

	public Result(TValue value, List<Error> errors, bool isError)
		: base(errors, isError)
	{
		Value = value;
	}

	public static Result<TValue> Success(TValue value)
	{
		return new Result<TValue>(value, null, isError: false);
	}

	public new static Result<TValue> Failure(Error error)
	{
		int num = 1;
		List<Error> list = new List<Error>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<Error> span = CollectionsMarshal.AsSpan(list);
		int num2 = 0;
		span[num2] = error;
		num2++;
		return new Result<TValue>(default(TValue), list, isError: true);
	}

	public new static Result<TValue> From(List<Error> errors)
	{
		return errors;
	}

	public new static Result<TValue> Success()
	{
		return new Result<TValue>(default(TValue), null, isError: false);
	}

	public new static Result<TValue> Failure(List<Error> errors)
	{
		if (errors == null)
		{
			throw new ArgumentNullException("errors");
		}
		return new Result<TValue>(default(TValue), errors, isError: true);
	}

	public static implicit operator Result<TValue>(TValue value)
	{
		return Success(value);
	}

	public static implicit operator Result<TValue>(Error error)
	{
		return Failure(error);
	}

	public static implicit operator Result<TValue>(List<Error> errors)
	{
		return Failure(errors);
	}
}
