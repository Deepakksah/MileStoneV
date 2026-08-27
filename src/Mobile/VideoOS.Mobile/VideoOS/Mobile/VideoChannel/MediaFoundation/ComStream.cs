using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

public class ComStream : Stream, IStream, IWritable
{
	private Stream _stream;

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

	public ComStream(Stream stream)
		: this(stream, sync: true)
	{
	}

	public ComStream(Stream stream, bool sync)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		if (sync)
		{
			stream = Stream.Synchronized(stream);
		}
		_stream = stream;
	}

	HResult IStream.Clone(out IStream ppstm)
	{
		ppstm = null;
		return HResult.E_NOTIMPL;
	}

	HResult IStream.Commit(int grfCommitFlags)
	{
		_stream.Flush();
		return HResult.S_OK;
	}

	HResult IStream.CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
	{
		return HResult.E_NOTIMPL;
	}

	HResult IStream.LockRegion(long libOffset, long cb, int dwLockType)
	{
		return HResult.E_NOTIMPL;
	}

	HResult IStream.Read(byte[] pv, int cb, IntPtr pcbRead)
	{
		if (!CanRead)
		{
			throw new InvalidOperationException("Stream not readable");
		}
		int num = Read(pv, 0, cb);
		if (pcbRead != IntPtr.Zero)
		{
			Marshal.WriteInt64(pcbRead, num);
		}
		return HResult.S_OK;
	}

	HResult IStream.Revert()
	{
		return HResult.E_NOTIMPL;
	}

	HResult IStream.Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition)
	{
		long val = Seek(dlibMove, (SeekOrigin)dwOrigin);
		if (plibNewPosition != IntPtr.Zero)
		{
			Marshal.WriteInt64(plibNewPosition, val);
		}
		return HResult.S_OK;
	}

	HResult IStream.SetSize(long libNewSize)
	{
		SetLength(libNewSize);
		return HResult.S_OK;
	}

	HResult IStream.Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag)
	{
		System.Runtime.InteropServices.ComTypes.STATSTG sTATSTG = new System.Runtime.InteropServices.ComTypes.STATSTG
		{
			type = 2,
			cbSize = Length
		};
		if (CanWrite && CanRead)
		{
			sTATSTG.grfMode = 2;
		}
		else if (CanRead)
		{
			sTATSTG.grfMode = 0;
		}
		else
		{
			if (!CanWrite)
			{
				throw new ObjectDisposedException("Stream");
			}
			sTATSTG.grfMode = 1;
		}
		pstatstg = sTATSTG;
		return HResult.S_OK;
	}

	HResult IStream.UnlockRegion(long libOffset, long cb, int dwLockType)
	{
		return HResult.E_NOTIMPL;
	}

	HResult IStream.Write(byte[] pv, int cb, IntPtr pcbWritten)
	{
		if (!CanWrite)
		{
			throw new InvalidOperationException("Stream is not writeable.");
		}
		Write(pv, 0, cb);
		Marshal.WriteInt32(pcbWritten, cb);
		return HResult.S_OK;
	}

	public override void Flush()
	{
		_stream.Flush();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		return _stream.Read(buffer, offset, count);
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

	protected override void Dispose(bool disposing)
	{
		if (_stream != null)
		{
			_stream.Dispose();
			_stream = null;
		}
	}
}
