using System;

namespace VideoOS.Mobile.Portable.Interfaces;

public interface IThumbnail
{
	byte[] JpegData { get; }

	int Width { get; }

	int Height { get; }

	DateTime Time { get; }
}
