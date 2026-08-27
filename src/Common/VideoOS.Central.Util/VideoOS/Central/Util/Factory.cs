using System;

namespace VideoOS.Central.Util;

public class Factory<T> : IFactory<T>
{
	private readonly Func<T> _create;

	public Factory(Func<T> create)
	{
		_create = create;
	}

	public T Create()
	{
		return _create();
	}
}
public class Factory<T, TArg1> : IFactory<T, TArg1>
{
	private readonly Func<TArg1, T> _create;

	public Factory(Func<TArg1, T> create)
	{
		_create = create;
	}

	public T Create(TArg1 arg1)
	{
		return _create(arg1);
	}
}
