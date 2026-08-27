using System;
using System.IO;

namespace VideoOS.Installer.Setup;

public class ObservableStream : Stream
{
	private const int _reportPercentInterval = 1;

	private readonly Stream _stream;

	private readonly long _streamLength;

	private readonly IProgress<int> _progress;

	private int _nextReportThreshold;

	private long _bytesRead;

	public override bool CanRead => _stream.CanRead;

	public override bool CanSeek => _stream.CanSeek;

	public override bool CanWrite => _stream.CanWrite;

	public override long Length => _stream.Length;

	public override long Position
	{
		get
		{
			return _stream.Position;
		}
		set
		{
			_stream.Position = value;
		}
	}

	public ObservableStream(Stream stream, IProgress<int> progress)
	{
		_stream = stream;
		_streamLength = _stream.Length;
		_progress = progress;
		_bytesRead = 0L;
		_nextReportThreshold = 1;
	}

	public void Begin()
	{
		_bytesRead = 0L;
		_stream.Position = 0L;
		_nextReportThreshold = 1;
		_progress.Report(0);
	}

	public void End()
	{
		_progress.Report(100);
	}

	public void UpdateReadCount(int bytesRead)
	{
		if (_streamLength != 0L)
		{
			_bytesRead += bytesRead;
			int num = Math.Min(100, (int)(100 * _bytesRead / _streamLength));
			if (num >= _nextReportThreshold)
			{
				_nextReportThreshold++;
				_progress.Report(num);
			}
		}
	}

	public override void Flush()
	{
		_stream.Flush();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		int num = _stream.Read(buffer, offset, count);
		UpdateReadCount(num);
		return num;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		return _stream.Seek(offset, origin);
	}

	public override void SetLength(long value)
	{
		_stream.SetLength(value);
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		_stream.Write(buffer, offset, count);
	}
}
