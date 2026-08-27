using System;

namespace VideoOS.Management.Common;

public static class GridSize
{
	public enum GridSizeType
	{
		Grid8X8 = 8,
		Grid16X16 = 0x10,
		Grid32X32 = 0x20,
		Grid64X64 = 0x40
	}

	public static GridSizeType ConvertInt32ToGridSize(int gridSize)
	{
		return ConvertInt32ToGridSize(gridSize, GridSizeType.Grid16X16);
	}

	public static GridSizeType ConvertInt32ToGridSize(int gridSize, GridSizeType defaultValue)
	{
		if (Enum.IsDefined(typeof(GridSizeType), gridSize))
		{
			return (GridSizeType)gridSize;
		}
		return defaultValue;
	}

	public static int ConvertGridSizeToInt32(GridSizeType gridSize)
	{
		return ConvertGridSizeToInt32(gridSize, 16);
	}

	public static int ConvertGridSizeToInt32(GridSizeType gridSize, int defaultValue)
	{
		if (Enum.IsDefined(typeof(GridSizeType), gridSize))
		{
			return (int)gridSize;
		}
		return defaultValue;
	}

	public static void SetGridSize(GridSizeType newGridSize, int currentNumberOfCells, Action<int> increaseNumberOfCells, Action<int, int> initialize)
	{
		ValidateDelegates(increaseNumberOfCells, initialize);
		if (currentNumberOfCells != (int)newGridSize)
		{
			if (currentNumberOfCells > (int)newGridSize || currentNumberOfCells < 8)
			{
				initialize((int)newGridSize, (int)newGridSize);
				return;
			}
			if ((int)newGridSize % currentNumberOfCells != 0)
			{
				initialize((int)newGridSize, (int)newGridSize);
				return;
			}
			int obj = (int)newGridSize / currentNumberOfCells;
			increaseNumberOfCells(obj);
		}
	}

	private static void ValidateDelegates(Action<int> increaseNumberOfCells, Action<int, int> initialize)
	{
		if (increaseNumberOfCells == null)
		{
			throw new ArgumentNullException("increaseNumberOfCells");
		}
		if (initialize == null)
		{
			throw new ArgumentNullException("initialize");
		}
	}

	public static int CalculateGridLength(GridSizeType gridSize)
	{
		return (int)gridSize * (int)gridSize;
	}
}
