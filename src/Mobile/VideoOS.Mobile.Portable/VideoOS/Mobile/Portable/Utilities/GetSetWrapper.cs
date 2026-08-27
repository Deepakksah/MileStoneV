using System;

namespace VideoOS.Mobile.Portable.Utilities;

public class GetSetWrapper<T>
{
	public Action<T> Setter;

	public Func<T> Getter;
}
