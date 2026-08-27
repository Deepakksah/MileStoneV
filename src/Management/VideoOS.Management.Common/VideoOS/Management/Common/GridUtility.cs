using System;

namespace VideoOS.Management.Common;

public static class GridUtility
{
	public static int ConvertTwoDimensionalIndexesToOneDimensional(int x, int y, int dimension)
	{
		CheckParam(x, dimension, "x");
		CheckParam(y, dimension, "y");
		return y * dimension + x;
	}

	public static GridCoordinate ConvertOneDimensionalIndexToTwoDimensional(int index, int dimension)
	{
		CheckParam(index, dimension * dimension, "index");
		return new GridCoordinate
		{
			X = index % dimension,
			Y = index / dimension
		};
	}

	private static void CheckParam(int param, int dimension, string paramName)
	{
		if (param < 0 || param >= dimension)
		{
			throw new ArgumentOutOfRangeException(paramName);
		}
	}
}
