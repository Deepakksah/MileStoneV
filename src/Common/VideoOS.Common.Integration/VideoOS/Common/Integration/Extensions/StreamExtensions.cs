using System;
using System.IO;
using System.Security.Cryptography;

namespace VideoOS.Common.Integration.Extensions;

public static class StreamExtensions
{
	public static byte[] GetSHA256Hash(this Stream stream)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		if (!stream.CanRead)
		{
			throw new ArgumentException("The stream is not readable.", "stream");
		}
		long position = stream.Position;
		stream.Seek(0L, SeekOrigin.Begin);
		using SHA256 sHA = SHA256.Create();
		byte[] result = sHA.ComputeHash(stream);
		if (position < stream.Length)
		{
			stream.Seek(position, SeekOrigin.Begin);
		}
		return result;
	}
}
