using System;

namespace VideoOS.Mobile.Portable.Utilities;

public static class ArrayExtensions
{
	public static T[] SubArray<T>(this T[] data, int index, int length)
	{
		T[] array = new T[length];
		Array.Copy(data, index, array, 0, length);
		return array;
	}
}
