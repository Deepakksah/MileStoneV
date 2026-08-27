using System;
using System.Windows;

namespace VideoOS.Platform.Client;

public class ImageOrPaintInfoChangedEventArgs : EventArgs
{
	public Size ImageSize { get; private set; }

	public Size PaintSize { get; private set; }

	public Point PaintLocation { get; private set; }

	public ImageOrPaintInfoChangedEventArgs(Size imageSize, Size paintSize, Point paintLocation)
	{
		ImageSize = imageSize;
		PaintSize = paintSize;
		PaintLocation = paintLocation;
	}
}
