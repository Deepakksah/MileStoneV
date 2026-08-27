using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace VideoOS.Platform.AccessControl.Internal.Util;

public class ImageToByteDataConverter
{
	public static byte[] ToByteData(Image picture)
	{
		using MemoryStream memoryStream = new MemoryStream();
		picture.Save(memoryStream, ImageFormat.Jpeg);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		return memoryStream.ToArray();
	}

	public static Image FromByteData(byte[] pictureData)
	{
		using MemoryStream stream = new MemoryStream(pictureData);
		using Bitmap original = new Bitmap(stream);
		return new Bitmap(original);
	}
}
