using System.Windows.Media;

namespace VideoOS.Platform.Search.Results;

public abstract class BoundingShape
{
	public Color? LineColor { get; set; }

	public Color? FillColor { get; set; }

	public BoundingShape()
	{
	}
}
