namespace VideoOS.Management.VmoClient;

public class GridCell
{
	public int X { get; private set; }

	public int Y { get; private set; }

	public GridCell(int x, int y)
	{
		X = x;
		Y = y;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is GridCell gridCell))
		{
			return false;
		}
		if (X == gridCell.X)
		{
			return Y == gridCell.Y;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (X * 128 + Y).GetHashCode();
	}
}
