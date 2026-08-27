using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Mobile.AudioSource;

public static class IntArrayExtensions
{
	public static void Mix(this int[] result, int[] first, int[] second)
	{
		int num = Math.Min(result.Length, Math.Min(first.Length, second.Length));
		for (int i = 0; i < num; i++)
		{
			result[i] = first[i] + second[i];
		}
	}

	public static void Mix(this int[] result, int[] first, int[] second, int[] third)
	{
		int num = Math.Min(result.Length, Math.Min(first.Length, Math.Min(second.Length, third.Length)));
		for (int i = 0; i < num; i++)
		{
			result[i] = first[i] + second[i] + third[i];
		}
	}

	public static void Mix(this int[] result, IEnumerable<int[]> inputs)
	{
		int num = result.Length;
		foreach (int[] input in inputs)
		{
			num = Math.Min(num, input.Length);
		}
		List<int[]> list = new List<int[]>(inputs);
		for (int i = 0; i < num; i++)
		{
			result[i] = 0;
			for (int j = 0; j < list.Count(); j++)
			{
				result[i] += list[j][i];
			}
		}
	}
}
