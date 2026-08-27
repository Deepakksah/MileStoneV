using System;

namespace VideoOS.Platform.Util;

public class Factory<T> : IFactory<T>
{
	private Func<T> _creator;

	public Factory(Func<T> creator)
	{
		_creator = creator;
	}

	T IFactory<T>.Create()
	{
		return _creator();
	}
}
