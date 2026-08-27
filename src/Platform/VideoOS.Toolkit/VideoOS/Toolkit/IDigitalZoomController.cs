namespace VideoOS.Toolkit;

public interface IDigitalZoomController
{
	public enum MoveCommand
	{
		Up,
		UpRight,
		Right,
		DownRight,
		Down,
		DownLeft,
		Left,
		UpLeft,
		Home,
		ZoomIn,
		ZoomOut
	}

	public class Point
	{
		public double x;

		public double y;

		public Point(double x, double y)
		{
			this.x = x;
			this.y = y;
		}

		public Point()
		{
		}
	}

	void SetAbsolutePosition(string position);

	string GetAbsolutePosition();

	void MoveRelative(double x, double y);

	void MoveRelative(double left, double top, double right, double bottom);

	void Move(MoveCommand command);

	void ConvertDewarpedCoordinates(double xCoordinate, double yCoordinate, ref double outXCoordinate, ref double outYCoordinate);

	Point[] ConvertDewarpedPoints(Point[] points);
}
