using System;

namespace VideoOS.Platform.Admin;

public class VideoInfoArgs : EventArgs
{
	public int Width { get; set; }

	public int Height { get; set; }

	public int DataSize { get; set; }

	public int Count { get; set; }
}
