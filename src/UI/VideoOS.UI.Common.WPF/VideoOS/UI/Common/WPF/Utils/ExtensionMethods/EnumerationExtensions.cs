using System;

namespace VideoOS.UI.Common.WPF.Utils.ExtensionMethods;

public static class EnumerationExtensions
{
	public static T Next<T>(this T src) where T : struct
	{
		if (!typeof(T).IsEnum)
		{
			throw new ArgumentException("Argument '" + typeof(T).FullName + "' is not an Enum");
		}
		T[] array = (T[])Enum.GetValues(src.GetType());
		int num = Array.IndexOf(array, src) + 1;
		if (array.Length != num)
		{
			return array[num];
		}
		return array[0];
	}
}
