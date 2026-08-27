using System;
using System.Drawing;
using System.Windows;

namespace VideoOS.Platform.Client;

public class ImageDisplayedEventArgs : EventArgs
{
	public DateTime ImageTime { get; set; }

	public DateTime PreviousImageTime { get; set; }

	public DateTime NextImageTime { get; set; }

	public bool IsFlippedHorizontally { get; set; }

	public bool IsFlippedVertically { get; set; }

	public System.Drawing.Size OriginalImageSize { get; set; }

	public System.Drawing.Size WindowSize { get; set; }

	public Rect VideoEffectArea { get; set; }
}
