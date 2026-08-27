using System;
using System.Text;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

public static class BoxStringConvertor
{
	public static string FixedArrayToString(this byte[] array)
	{
		return Encoding.ASCII.GetString(array, 1, Math.Min(array.Length - 1, array[0]));
	}

	public static void WriteToFixedArray(this string text, byte[] array)
	{
		Array.Clear(array, 0, array.Length);
		byte[] bytes = Encoding.ASCII.GetBytes(text);
		int length = Math.Min(bytes.Length, array.Length - 1);
		Array.Copy(bytes, 0, array, 1, length);
	}
}
