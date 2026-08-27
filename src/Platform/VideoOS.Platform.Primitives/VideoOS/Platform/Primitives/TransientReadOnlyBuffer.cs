using System;
using System.Runtime.InteropServices;

namespace VideoOS.Platform.Primitives;

public class TransientReadOnlyBuffer : IReadOnlyBuffer, IDisposable
{
	private ulong m_length;

	private GCHandle m_gch;

	public unsafe virtual byte* Data
	{
		get
		{
			void* result = (void*)m_gch.AddrOfPinnedObject();
			GC.KeepAlive(this);
			return (byte*)result;
		}
	}

	public virtual ulong Length => m_length;

	public TransientReadOnlyBuffer(byte[] data, long length)
	{
		GCHandle gch = GCHandle.Alloc(data, GCHandleType.Pinned);
		m_gch = gch;
		m_length = (ulong)length;
	}

	private void _007ETransientReadOnlyBuffer()
	{
		m_gch.Free();
		GC.KeepAlive(this);
	}

	public virtual void CopyTo(ulong sourceIndex, byte[] destination, ulong destinationIndex, ulong length)
	{
		if (null == destination)
		{
			throw new ArgumentNullException("destination");
		}
		ulong longLength = (ulong)destination.LongLength;
		ulong length2 = m_length;
		if (sourceIndex > length2)
		{
			throw new ArgumentOutOfRangeException("sourceIndex");
		}
		if (destinationIndex > longLength)
		{
			throw new ArgumentOutOfRangeException("destinationIndex");
		}
		if (sourceIndex + length <= length2 && destinationIndex + length <= longLength)
		{
			Array.Copy((Array)m_gch.Target, (long)sourceIndex, destination, (long)destinationIndex, (long)length);
			GC.KeepAlive(this);
			return;
		}
		throw new ArgumentOutOfRangeException("length");
	}

	public virtual void CopyTo(long sourceIndex, byte[] destination, long destinationIndex, long length)
	{
		if (sourceIndex < 0)
		{
			throw new ArgumentOutOfRangeException("sourceIndex");
		}
		if (destinationIndex < 0)
		{
			throw new ArgumentOutOfRangeException("destinationIndex");
		}
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException("length");
		}
		CopyTo((ulong)sourceIndex, destination, (ulong)destinationIndex, (ulong)length);
	}

	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			m_gch.Free();
			GC.KeepAlive(this);
		}
		else
		{
			base.Finalize();
		}
	}

	public virtual sealed void Dispose()
	{
		Dispose(A_0: true);
		GC.SuppressFinalize(this);
		GC.KeepAlive(this);
	}
}
