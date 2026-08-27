using System;

namespace VideoOS.Mobile.Portable.Utilities;

public class Disposing
{
	public delegate void OnDispose<in I>(I obj);

	public static void DisposeMember<T>(ref T obj) where T : class
	{
		DisposeMemberObject(ref obj, delegate(IDisposable iDisposable)
		{
			iDisposable.Dispose();
		});
	}

	public static void DisposeMemberQuiet<T>(ref T obj) where T : class
	{
		DisposeMemberObject(ref obj, delegate(IDisposableQuiet iDisposable)
		{
			iDisposable.DisposeQuiet();
		});
	}

	public static void DisposeMemberObject<T, I>(ref T obj, OnDispose<I> disposer) where T : class where I : class
	{
		if (obj != null)
		{
			if (obj is I obj2)
			{
				disposer(obj2);
			}
			obj = null;
		}
	}

	public static void DisposeMembers(object obj)
	{
	}
}
