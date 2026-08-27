using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.Utilities.Rect;

namespace VideoOS.Mobile.VideoSource;

public class ImageUtils
{
	public delegate Bitmap Tranform(Bitmap original);

	public static ImageCodecInfo GetCodecInfo(ImageFormat format)
	{
		ImageCodecInfo[] imageDecoders = ImageCodecInfo.GetImageDecoders();
		foreach (ImageCodecInfo imageCodecInfo in imageDecoders)
		{
			if (imageCodecInfo.FormatID == format.Guid)
			{
				return imageCodecInfo;
			}
		}
		return null;
	}

	public static EncoderParameters GetEncoderParams(int compression)
	{
		Encoder quality = Encoder.Quality;
		EncoderParameters encoderParameters = new EncoderParameters(1);
		EncoderParameter encoderParameter = new EncoderParameter(quality, compression);
		encoderParameters.Param[0] = encoderParameter;
		return encoderParameters;
	}

	public static byte[] Transcode(byte[] original, VideoParams videoParams)
	{
		if (original != null && original.Length > 1)
		{
			byte[] result = original;
			MemoryStream memoryStream = new MemoryStream(original);
			Bitmap bitmap = new Bitmap(memoryStream);
			if (bitmap.Width != videoParams.DestWidth || bitmap.Height != videoParams.DestHeight)
			{
				if (videoParams.CheckForMask(StreamParamsHelper.VideoParamMask.SrcRect))
				{
					Bitmap bitmap2 = bitmap.Clone(videoParams.SrcRect.ToRectnagleF(), bitmap.PixelFormat);
					bitmap.Dispose();
					bitmap = bitmap2;
				}
				ImageCodecInfo codecInfo = GetCodecInfo(ImageFormat.Jpeg);
				EncoderParameters encoderParams = GetEncoderParams((int)videoParams.CompressionLvl);
				Bitmap bitmap3 = new Bitmap(bitmap, new Size(videoParams.DestWidth, videoParams.DestHeight));
				MemoryStream memoryStream2 = new MemoryStream();
				bitmap3.Save(memoryStream2, codecInfo, encoderParams);
				memoryStream2.Seek(0L, SeekOrigin.Begin);
				result = memoryStream2.ToArray();
				memoryStream2.Close();
				bitmap3.Dispose();
				memoryStream2.Dispose();
			}
			bitmap.Dispose();
			memoryStream.Dispose();
			return result;
		}
		return new byte[0];
	}

	public static byte[] Convert(byte[] original, ImageFormat format, Tranform transformer = null)
	{
		MemoryStream memoryStream = new MemoryStream(original);
		Bitmap bitmap = new Bitmap(memoryStream);
		if (transformer != null)
		{
			bitmap = transformer(bitmap);
		}
		MemoryStream memoryStream2 = new MemoryStream();
		bitmap.Save(memoryStream2, format);
		byte[] buffer = memoryStream2.GetBuffer();
		memoryStream2.Close();
		bitmap.Dispose();
		memoryStream.Close();
		return buffer;
	}

	public static Bitmap Resize(Bitmap original, int destWidth, int destHeight)
	{
		return new Bitmap(original, destWidth, destHeight);
	}

	public static Bitmap ResizeAspect(Bitmap original, int destWidth, int destHeight)
	{
		double num = (double)original.Width / (double)original.Height;
		double num2 = (double)destWidth / (double)destHeight;
		if (num > num2)
		{
			destHeight = (int)Math.Round((double)destWidth / num);
		}
		else
		{
			destWidth = (int)Math.Round((double)destHeight * num);
		}
		return Resize(original, destWidth, destHeight);
	}

	public static byte[] ImageToByteArr(Image image, ImageFormat format)
	{
		using MemoryStream memoryStream = new MemoryStream();
		image.Save(memoryStream, format);
		return memoryStream.ToArray();
	}
}
