using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace VideoOS.Platform.AccessControl.Plugin;

public sealed class ACIconInfo
{
	public string Key { get; private set; }

	public Icon Icon { get; private set; }

	public ACIconInfo(string key, Bitmap bitmap)
	{
		Key = key;
		Icon = Create(new List<Bitmap> { bitmap });
	}

	public ACIconInfo(string key, IEnumerable<Bitmap> bitmaps)
	{
		bitmaps = new ReadOnlyCollection<Bitmap>((bitmaps != null) ? bitmaps.ToList() : new List<Bitmap>());
		Key = key;
		Icon = Create(bitmaps);
	}

	public ACIconInfo(string key, Icon icon)
	{
		Key = key;
		Icon = Create(icon);
	}

	private static Icon Create(Icon icon)
	{
		if (IconToBytes(icon)[4] != 6)
		{
			try
			{
				Bitmap bitmap = Bitmap.FromHicon(icon.Handle);
				Icon result = Create(new List<Bitmap> { bitmap });
				bitmap.Dispose();
				return result;
			}
			catch (Exception)
			{
			}
		}
		return icon;
	}

	private static Icon Create(IEnumerable<Bitmap> imagesInput)
	{
		List<Bitmap> list = CheckAndFix(imagesInput).ToList();
		MemoryStream memoryStream = new MemoryStream();
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		List<byte[]> list2 = new List<byte[]>();
		binaryWriter.Write((short)0);
		binaryWriter.Write((short)1);
		binaryWriter.Write((short)list.Count);
		int num = 16;
		int num2 = 6 + list.Count * num;
		for (int i = 0; i < list.Count; i++)
		{
			Bitmap bitmap = list[i];
			byte[] array = BitmapToBytes(bitmap);
			binaryWriter.Write((byte)bitmap.Width);
			binaryWriter.Write((byte)bitmap.Height);
			binaryWriter.Write((byte)0);
			binaryWriter.Write((byte)0);
			binaryWriter.Write((short)1);
			binaryWriter.Write(BitmapDepth(array));
			binaryWriter.Write((uint)array.Length);
			binaryWriter.Write((uint)num2);
			list2.Add(array);
			num2 += array.Length;
		}
		foreach (byte[] item in list2)
		{
			binaryWriter.Write(item);
		}
		binaryWriter.Flush();
		memoryStream.Seek(0L, SeekOrigin.Begin);
		Icon result = new Icon(memoryStream);
		binaryWriter.Close();
		memoryStream.Close();
		return result;
	}

	private static IEnumerable<Bitmap> CheckAndFix(IEnumerable<Bitmap> bitmaps)
	{
		return new List<Bitmap>
		{
			FixOneImage(8, bitmaps),
			FixOneImage(16, bitmaps),
			FixOneImage(24, bitmaps),
			FixOneImage(32, bitmaps),
			FixOneImage(48, bitmaps),
			FixOneImage(64, bitmaps)
		};
	}

	private static short BitmapDepth(byte[] data)
	{
		if (data[0] == 137 && data[1] == 80)
		{
			return data[24];
		}
		return 32;
	}

	private static Bitmap FixOneImage(int pixels, IEnumerable<Bitmap> bitmaps)
	{
		Bitmap bitmap = null;
		Bitmap bitmap2 = null;
		foreach (Bitmap bitmap4 in bitmaps)
		{
			if (bitmap4.Width == pixels)
			{
				bitmap2 = bitmap4;
				break;
			}
			if (bitmap == null)
			{
				bitmap = bitmap4;
			}
			else if (bitmap.Width < bitmap4.Width && bitmap.Width < pixels)
			{
				bitmap = bitmap4;
			}
		}
		if (bitmap2 == null)
		{
			bitmap2 = bitmap;
		}
		Bitmap bitmap3 = new Bitmap(pixels, pixels, PixelFormat.Format32bppArgb);
		Graphics graphics = Graphics.FromImage(bitmap3);
		graphics.FillRectangle(Brushes.Transparent, 0, 0, pixels, pixels);
		graphics.DrawImage(bitmap2, 0, 0, pixels, pixels);
		graphics.Dispose();
		return bitmap3;
	}

	private static byte[] BitmapToBytes(Bitmap bitmap)
	{
		if (bitmap == null)
		{
			return null;
		}
		try
		{
			MemoryStream memoryStream = new MemoryStream();
			bitmap.Save(memoryStream, ImageFormat.Png);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			byte[] result = memoryStream.ToArray();
			memoryStream.Close();
			return result;
		}
		catch (ExternalException)
		{
			return null;
		}
	}

	private static byte[] IconToBytes(Icon icon)
	{
		if (icon == null)
		{
			return null;
		}
		MemoryStream memoryStream = new MemoryStream();
		icon.Save(memoryStream);
		memoryStream.Seek(0L, SeekOrigin.Begin);
		byte[] result = memoryStream.ToArray();
		memoryStream.Close();
		return result;
	}
}
