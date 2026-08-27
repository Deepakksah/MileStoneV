using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace VideoOS.Server.RecorderCommunication;

public class StreamContentWithProgress : HttpContent
{
	private const int BUFFER_SIZE = 65536;

	private Stream sourceStream;

	private IProgress<int> progress;

	public StreamContentWithProgress(Stream sourceStream, IProgress<int> progress)
	{
		this.sourceStream = sourceStream;
		this.progress = progress;
	}

	protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		byte[] buffer = new byte[65536];
		long size = sourceStream.Length;
		int sent = 0;
		using (sourceStream)
		{
			while (true)
			{
				int num = await sourceStream.ReadAsync(buffer, 0, buffer.Length);
				if (num != 0)
				{
					sent += num;
					progress.Report((int)(100f * ((float)sent / (float)size)));
					await stream.WriteAsync(buffer, 0, num);
					continue;
				}
				break;
			}
		}
	}

	protected override bool TryComputeLength(out long length)
	{
		length = sourceStream.Length;
		return true;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			sourceStream.Dispose();
		}
		((HttpContent)this).Dispose(disposing);
	}
}
