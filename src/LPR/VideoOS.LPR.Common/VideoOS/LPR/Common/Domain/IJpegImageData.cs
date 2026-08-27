using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace VideoOS.LPR.Common.Domain;

public interface IJpegImageData
{
	IEnumerable<byte> Bytes { get; }

	Size Size { get; }

	BitmapSource ImageSource { get; }
}
