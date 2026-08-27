using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

public class Grid
{
	private HashSet<GridCell> _cells;

	private GridSize.GridSizeType _gridSize;

	public ReadOnlyCollection<GridCell> Cells => new ReadOnlyCollection<GridCell>(_cells.ToList());

	public GridSize.GridSizeType GridSize
	{
		get
		{
			return _gridSize;
		}
		set
		{
			if (!Enum.IsDefined(typeof(GridSize.GridSizeType), value))
			{
				throw new ArgumentOutOfRangeException("value");
			}
			if (value < _gridSize)
			{
				_gridSize = value;
				_cells.Clear();
				RaiseGridChanged();
			}
			else if (value > _gridSize)
			{
				int increaseFactor = (int)value / (int)_gridSize;
				_gridSize = value;
				IncreaseNumberOfCells(increaseFactor);
				RaiseGridChanged();
			}
		}
	}

	public event EventHandler GridChanged;

	private void RaiseGridChanged()
	{
		if (this.GridChanged != null)
		{
			this.GridChanged(this, null);
		}
	}

	public Grid(string grid, GridSize.GridSizeType gridSize)
	{
		_cells = new HashSet<GridCell>();
		_gridSize = gridSize;
		PopulateGridCells(grid);
	}

	public void Initialize(string grid, GridSize.GridSizeType gridSize)
	{
		_gridSize = gridSize;
		PopulateGridCells(grid);
		RaiseGridChanged();
	}

	public string GetGridAsString()
	{
		StringBuilder stringBuilder = new StringBuilder(new string('0', (int)_gridSize * (int)_gridSize));
		foreach (GridCell cell in _cells)
		{
			int num = (int)(cell.Y * (int)_gridSize - (_gridSize - cell.X));
			stringBuilder.Replace('0', '1', num - 1, 1);
		}
		return stringBuilder.ToString();
	}

	private void PopulateGridCells(string grid)
	{
		_cells.Clear();
		int gridSize = (int)_gridSize;
		int num = 0;
		if (grid == null)
		{
			return;
		}
		foreach (char num2 in grid)
		{
			num++;
			int x = (num - 1) % gridSize + 1;
			int y = (num - 1) / gridSize + 1;
			if (num2 == '1')
			{
				_cells.Add(new GridCell(x, y));
			}
		}
	}

	private void IncreaseNumberOfCells(int increaseFactor)
	{
		List<GridCell> list = _cells.ToList();
		_cells.Clear();
		foreach (GridCell item in list)
		{
			SetRegion((item.X - 1) * increaseFactor + 1, (item.Y - 1) * increaseFactor + 1, increaseFactor, increaseFactor);
		}
	}

	public void SetRegion(int x, int y, int width, int height)
	{
		UpdateRegion(x, y, width, height, set: true);
	}

	public void ClearRegion(int x, int y, int width, int height)
	{
		UpdateRegion(x, y, width, height, set: false);
	}

	private void UpdateRegion(int x, int y, int width, int height, bool set)
	{
		if (x < 1 || x > (int)_gridSize)
		{
			throw new ArgumentOutOfRangeException("x");
		}
		if (y < 1 || y > (int)_gridSize)
		{
			throw new ArgumentOutOfRangeException("y");
		}
		if (width < 1 || width + x > (int)(_gridSize + 1))
		{
			throw new ArgumentOutOfRangeException("width");
		}
		if (height < 1 || height + y > (int)(_gridSize + 1))
		{
			throw new ArgumentOutOfRangeException("height");
		}
		int num = x;
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				if (set)
				{
					_cells.Add(new GridCell(x, y));
				}
				else
				{
					_cells.Remove(new GridCell(x, y));
				}
				x++;
			}
			y++;
			x = num;
		}
		RaiseGridChanged();
	}

	public void FillAll()
	{
		SetRegion(1, 1, (int)_gridSize, (int)_gridSize);
	}

	public void ClearAll()
	{
		_cells.Clear();
		RaiseGridChanged();
	}

	public bool Contains(int x, int y)
	{
		return _cells.Contains(new GridCell(x, y));
	}
}
