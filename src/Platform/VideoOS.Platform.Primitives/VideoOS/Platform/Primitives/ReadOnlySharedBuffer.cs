#define TRACE
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using NmBuffers;
using msclr;
using std;

namespace VideoOS.Platform.Primitives;

public class ReadOnlySharedBuffer : IReadOnlyBuffer, IDisposable
{
	private class RefCountingSharedBuffer : IDisposable
	{
		private bool m_Disposed;

		private int m_RefCount;

		private unsafe CmReadOnlySharedBuffer* m_Buffer;

		public unsafe CmReadOnlySharedBuffer* Buffer
		{
			get
			{
				if (m_Disposed)
				{
					throw new ObjectDisposedException("RefCountingSharedBuffer");
				}
				GC.KeepAlive(this);
				return m_Buffer;
			}
		}

		public unsafe RefCountingSharedBuffer(CmReadOnlySharedBuffer* buffer)
		{
			m_Disposed = false;
			m_RefCount = 1;
			m_Buffer = buffer;
			base._002Ector();
		}

		private void _007ERefCountingSharedBuffer()
		{
			m_Disposed = true;
			_0021RefCountingSharedBuffer();
		}

		private unsafe void _0021RefCountingSharedBuffer()
		{
			//IL_0060: Expected I, but got I8
			//IL_004f: Expected I, but got I8
			if (!m_Disposed)
			{
				CmReadOnlySharedBuffer* buffer = m_Buffer;
				if (0L != (nint)buffer)
				{
					ValueType arg = *(ulong*)buffer;
					Trace.WriteLine($"ReadOnlySharedBuffer::RefCountingSharedBuffer is being finalized ({arg} bytes)");
				}
			}
			CmReadOnlySharedBuffer* buffer2 = m_Buffer;
			if (0L != (nint)buffer2)
			{
				CmReadOnlySharedBuffer* ptr = buffer2;
				ulong num = *(ulong*)((long)(nint)ptr + 16L + 8);
				if (num != 0L)
				{
					global::_003CModule_003E.std_002E_Ref_count_base_002E_Decref((_Ref_count_base*)num);
				}
				global::_003CModule_003E.delete(ptr, 32uL);
				m_Buffer = null;
			}
		}

		public int IncrementRefCount()
		{
			@lock obj = null;
			if (m_Disposed)
			{
				throw new ObjectDisposedException("RefCountingSharedBuffer");
			}
			@lock obj2 = new @lock(this);
			int refCount;
			try
			{
				obj = obj2;
				m_RefCount++;
				GC.KeepAlive(this);
				refCount = m_RefCount;
			}
			catch
			{
				//try-fault
				((IDisposable)obj).Dispose();
				throw;
			}
			((IDisposable)obj).Dispose();
			return refCount;
		}

		public int DecrementRefCount()
		{
			@lock obj = null;
			if (m_Disposed)
			{
				throw new ObjectDisposedException("RefCountingSharedBuffer");
			}
			@lock obj2 = new @lock(this);
			int refCount;
			try
			{
				obj = obj2;
				if (--m_RefCount == 0)
				{
					((IDisposable)this).Dispose();
				}
				GC.KeepAlive(this);
				refCount = m_RefCount;
			}
			catch
			{
				//try-fault
				((IDisposable)obj).Dispose();
				throw;
			}
			((IDisposable)obj).Dispose();
			return refCount;
		}

		[HandleProcessCorruptedStateExceptions]
		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
		{
			if (A_0)
			{
				m_Disposed = true;
				_0021RefCountingSharedBuffer();
				return;
			}
			try
			{
				_0021RefCountingSharedBuffer();
			}
			finally
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

		~RefCountingSharedBuffer()
		{
			Dispose(A_0: false);
		}
	}

	private bool m_Disposed;

	private RefCountingSharedBuffer m_RefCountedBuffer;

	public unsafe virtual byte* Data
	{
		get
		{
			//IL_001f: Expected I, but got I8
			CmReadOnlySharedBuffer* buffer = m_RefCountedBuffer.Buffer;
			long num = *(long*)((ulong)(nint)buffer + 16uL) + *(long*)((ulong)(nint)buffer + 8uL);
			GC.KeepAlive(this);
			return (byte*)num;
		}
	}

	public unsafe virtual ulong Length
	{
		get
		{
			long buffer = *(long*)m_RefCountedBuffer.Buffer;
			GC.KeepAlive(this);
			return (ulong)buffer;
		}
	}

	public unsafe ReadOnlySharedBuffer(IReadOnlyBuffer other)
	{
		m_Disposed = false;
		m_RefCountedBuffer = null;
		base._002Ector();
		ulong length = other.Length;
		byte* data = other.Data;
		InitializeBuffer(data, length);
		GC.KeepAlive(this);
	}

	public unsafe ReadOnlySharedBuffer(IntPtr data, ulong length)
	{
		m_Disposed = false;
		m_RefCountedBuffer = null;
		base._002Ector();
		InitializeBuffer((byte*)(void*)data, length);
		GC.KeepAlive(this);
	}

	public unsafe ReadOnlySharedBuffer(IntPtr data, long length)
	{
		m_Disposed = false;
		m_RefCountedBuffer = null;
		base._002Ector();
		InitializeBuffer((byte*)(void*)data, (ulong)length);
		GC.KeepAlive(this);
	}

	public ReadOnlySharedBuffer(byte[] data, ulong length)
	{
		m_Disposed = false;
		m_RefCountedBuffer = null;
		base._002Ector();
		InitializeBuffer(data, length);
	}

	public ReadOnlySharedBuffer(byte[] data, long length)
	{
		m_Disposed = false;
		m_RefCountedBuffer = null;
		base._002Ector();
		InitializeBuffer(data, (ulong)length);
	}

	public ReadOnlySharedBuffer(byte[] data)
	{
		m_Disposed = false;
		m_RefCountedBuffer = null;
		base._002Ector();
		if (null == data)
		{
			throw new ArgumentNullException("data");
		}
		InitializeBuffer(data, (ulong)data.LongLength);
	}

	public unsafe ReadOnlySharedBuffer(CmUniqueBuffer* buffer)
	{
		//IL_0038: Expected I, but got I8
		//IL_0030: Expected I, but got I8
		//IL_0030: Expected I, but got I8
		m_Disposed = false;
		CmReadOnlySharedBuffer* ptr = (CmReadOnlySharedBuffer*)global::_003CModule_003E.@new(32uL);
		CmReadOnlySharedBuffer* buffer2;
		try
		{
			if (ptr != null)
			{
				*(long*)ptr = *(long*)((ulong)(nint)buffer + 8uL);
				*(long*)((ulong)(nint)ptr + 8uL) = 0L;
				global::_003CModule_003E.std_002Eshared_ptr_003Cunsigned_0020char_0020const_0020_003E_002E_007Bctor_007D_003Cunsigned_0020char_002Cclass_0020std_003A_003Afunction_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E_002C0_003E((shared_ptr_003Cunsigned_0020char_0020const_0020_003E*)((ulong)(nint)ptr + 16uL), (unique_ptr_003Cunsigned_0020char_002Cstd_003A_003Afunction_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E_0020_003E*)((ulong)(nint)buffer + 16uL));
				buffer2 = ptr;
			}
			else
			{
				buffer2 = null;
			}
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.delete(ptr, 32uL);
			throw;
		}
		m_RefCountedBuffer = new RefCountingSharedBuffer(buffer2);
		base._002Ector();
	}

	public unsafe ReadOnlySharedBuffer(CmReadOnlySharedBuffer* buffer)
	{
		//IL_0020: Expected I, but got I8
		m_Disposed = false;
		CmReadOnlySharedBuffer* ptr = (CmReadOnlySharedBuffer*)global::_003CModule_003E.@new(32uL);
		CmReadOnlySharedBuffer* buffer2;
		try
		{
			buffer2 = ((ptr == null) ? null : global::_003CModule_003E.NmBuffers_002ECmReadOnlySharedBuffer_002E_007Bctor_007D(ptr, buffer));
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.delete(ptr, 32uL);
			throw;
		}
		m_RefCountedBuffer = new RefCountingSharedBuffer(buffer2);
		base._002Ector();
	}

	public unsafe ReadOnlySharedBuffer(ReadOnlySharedBuffer buffer, byte* offset, ulong length)
	{
		//IL_0067: Expected I, but got I8
		//IL_0098: Expected I8, but got I
		//IL_00be: Expected I, but got I8
		m_Disposed = false;
		m_RefCountedBuffer = null;
		base._002Ector();
		if (null == buffer)
		{
			throw new ArgumentNullException("buffer");
		}
		if (0L == (nint)offset)
		{
			throw new ArgumentNullException("offset");
		}
		if (buffer.m_Disposed)
		{
			throw new ArgumentException("Buffer is disposed", "buffer");
		}
		CmReadOnlySharedBuffer* buffer2 = buffer.m_RefCountedBuffer.Buffer;
		byte* ptr = (byte*)(*(long*)((ulong)(nint)buffer2 + 16uL) + *(long*)((ulong)(nint)buffer2 + 8uL));
		ulong buffer3 = *(ulong*)buffer.m_RefCountedBuffer.Buffer;
		if (offset >= ptr)
		{
			ulong num = buffer3 + (ulong)(nint)ptr;
			if ((ulong)(nint)offset <= num)
			{
				if ((ulong)((long)(nint)offset + (long)length) > num)
				{
					throw new ArgumentOutOfRangeException("length");
				}
				ulong offset2 = (ulong)(nint)(offset - (nuint)ptr);
				CmReadOnlySharedBuffer* ptr2 = (CmReadOnlySharedBuffer*)global::_003CModule_003E.@new(32uL);
				CmReadOnlySharedBuffer* buffer4;
				try
				{
					buffer4 = ((ptr2 == null) ? null : global::_003CModule_003E.NmBuffers_002ECmReadOnlySharedBuffer_002E_007Bctor_007D(ptr2, buffer.m_RefCountedBuffer.Buffer, length, offset2));
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.delete(ptr2, 32uL);
					throw;
				}
				m_RefCountedBuffer = new RefCountingSharedBuffer(buffer4);
				return;
			}
		}
		throw new ArgumentException("The offset address is pointing outside of the buffers address range.", "offset");
	}

	public ReadOnlySharedBuffer(ReadOnlySharedBuffer other)
	{
		m_Disposed = false;
		m_RefCountedBuffer = other.m_RefCountedBuffer;
		base._002Ector();
		m_RefCountedBuffer.IncrementRefCount();
		GC.KeepAlive(this);
	}

	private void _007EReadOnlySharedBuffer()
	{
		m_Disposed = true;
		RefCountingSharedBuffer refCountedBuffer = m_RefCountedBuffer;
		if (null != refCountedBuffer)
		{
			if (refCountedBuffer.DecrementRefCount() == 0)
			{
				((IDisposable)m_RefCountedBuffer)?.Dispose();
			}
			m_RefCountedBuffer = null;
		}
	}

	public unsafe virtual void CopyTo(ulong sourceIndex, byte[] destination, ulong destinationIndex, ulong length)
	{
		//IL_008b: Expected I, but got I8
		if (null == destination)
		{
			throw new ArgumentNullException("destination");
		}
		ulong longLength = (ulong)destination.LongLength;
		CmReadOnlySharedBuffer* buffer = m_RefCountedBuffer.Buffer;
		if (sourceIndex > (ulong)(*(long*)buffer))
		{
			throw new ArgumentOutOfRangeException("sourceIndex");
		}
		if (destinationIndex > longLength)
		{
			throw new ArgumentOutOfRangeException("destinationIndex");
		}
		CmReadOnlySharedBuffer* buffer2 = m_RefCountedBuffer.Buffer;
		if (sourceIndex + length <= (ulong)(*(long*)buffer2) && destinationIndex + length <= longLength)
		{
			fixed (byte* ptr = &destination[0])
			{
				ref byte reference = ref System.Runtime.CompilerServices.Unsafe.Add(ref *ptr, destinationIndex);
				CmReadOnlySharedBuffer* buffer3 = m_RefCountedBuffer.Buffer;
				global::_003CModule_003E._003FA0xabe7bb48_002Ememcpy_s(System.Runtime.CompilerServices.Unsafe.AsPointer(ref reference), longLength, (void*)((ulong)(*(long*)((ulong)(nint)buffer3 + 16uL) + *(long*)((ulong)(nint)buffer3 + 8uL)) + sourceIndex), length);
				GC.KeepAlive(destination);
				GC.KeepAlive(this);
				return;
			}
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
		GC.KeepAlive(this);
	}

	private unsafe void InitializeBuffer(byte* data, ulong length)
	{
		//IL_001b: Expected I4, but got I8
		//IL_004c: Expected I, but got I8
		//IL_0044: Expected I, but got I8
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CmUniqueBuffer cmUniqueBuffer);
		global::_003CModule_003E.NmBuffers_002ECmUniqueBuffer_002E_007Bctor_007D(&cmUniqueBuffer, length);
		try
		{
			if (length != 0)
			{
				// IL cpblk instruction
				System.Runtime.CompilerServices.Unsafe.CopyBlockUnaligned(System.Runtime.CompilerServices.Unsafe.As<CmUniqueBuffer, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmUniqueBuffer, 80)), data, length);
			}
			CmReadOnlySharedBuffer* ptr = (CmReadOnlySharedBuffer*)global::_003CModule_003E.@new(32uL);
			CmReadOnlySharedBuffer* buffer;
			try
			{
				if (ptr != null)
				{
					*(long*)ptr = System.Runtime.CompilerServices.Unsafe.As<CmUniqueBuffer, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmUniqueBuffer, 8));
					*(long*)((ulong)(nint)ptr + 8uL) = 0L;
					global::_003CModule_003E.std_002Eshared_ptr_003Cunsigned_0020char_0020const_0020_003E_002E_007Bctor_007D_003Cunsigned_0020char_002Cclass_0020std_003A_003Afunction_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E_002C0_003E((shared_ptr_003Cunsigned_0020char_0020const_0020_003E*)((ulong)(nint)ptr + 16uL), (unique_ptr_003Cunsigned_0020char_002Cstd_003A_003Afunction_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmUniqueBuffer, 16)));
					buffer = ptr;
				}
				else
				{
					buffer = null;
				}
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr, 32uL);
				throw;
			}
			m_RefCountedBuffer = new RefCountingSharedBuffer(buffer);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmUniqueBuffer*, void>)(&global::_003CModule_003E.NmBuffers_002ECmUniqueBuffer_002E_007Bdtor_007D), &cmUniqueBuffer);
			throw;
		}
		global::_003CModule_003E.std_002Eunique_ptr_003Cunsigned_0020char_002Cstd_003A_003Afunction_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E_0020_003E_002E_007Bdtor_007D((unique_ptr_003Cunsigned_0020char_002Cstd_003A_003Afunction_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmUniqueBuffer, 16)));
	}

	private unsafe void InitializeBuffer(byte[] data, ulong length)
	{
		//IL_0049: Expected I, but got I8
		//IL_0081: Expected I, but got I8
		//IL_0079: Expected I, but got I8
		//The blocks IL_004a, IL_008d are reachable both inside and outside the pinned region starting at IL_003a. ILSpy has duplicated these blocks in order to place them both within and outside the `fixed` statement.
		if (null == data)
		{
			throw new ArgumentNullException("data");
		}
		ulong longLength = (ulong)data.LongLength;
		if (length > longLength)
		{
			throw new ArgumentOutOfRangeException("length");
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CmUniqueBuffer cmUniqueBuffer);
		global::_003CModule_003E.NmBuffers_002ECmUniqueBuffer_002E_007Bctor_007D(&cmUniqueBuffer, length);
		try
		{
			if ((nint)data.LongLength > 0)
			{
				fixed (byte* source = &data[0])
				{
					global::_003CModule_003E._003FA0xabe7bb48_002Ememcpy_s((void*)System.Runtime.CompilerServices.Unsafe.As<CmUniqueBuffer, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmUniqueBuffer, 80)), length, source, length);
					GC.KeepAlive(data);
					CmReadOnlySharedBuffer* ptr = (CmReadOnlySharedBuffer*)global::_003CModule_003E.@new(32uL);
					CmReadOnlySharedBuffer* buffer;
					try
					{
						if (ptr != null)
						{
							*(long*)ptr = System.Runtime.CompilerServices.Unsafe.As<CmUniqueBuffer, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmUniqueBuffer, 8));
							*(long*)((ulong)(nint)ptr + 8uL) = 0L;
							global::_003CModule_003E.std_002Eshared_ptr_003Cunsigned_0020char_0020const_0020_003E_002E_007Bctor_007D_003Cunsigned_0020char_002Cclass_0020std_003A_003Afunction_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E_002C0_003E((shared_ptr_003Cunsigned_0020char_0020const_0020_003E*)((ulong)(nint)ptr + 16uL), (unique_ptr_003Cunsigned_0020char_002Cstd_003A_003Afunction_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmUniqueBuffer, 16)));
							buffer = ptr;
						}
						else
						{
							buffer = null;
						}
					}
					catch
					{
						//try-fault
						global::_003CModule_003E.delete(ptr, 32uL);
						throw;
					}
					m_RefCountedBuffer = new RefCountingSharedBuffer(buffer);
				}
			}
			else
			{
				GC.KeepAlive(data);
				CmReadOnlySharedBuffer* ptr = (CmReadOnlySharedBuffer*)global::_003CModule_003E.@new(32uL);
				CmReadOnlySharedBuffer* buffer;
				try
				{
					if (ptr != null)
					{
						*(long*)ptr = System.Runtime.CompilerServices.Unsafe.As<CmUniqueBuffer, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmUniqueBuffer, 8));
						*(long*)((ulong)(nint)ptr + 8uL) = 0L;
						global::_003CModule_003E.std_002Eshared_ptr_003Cunsigned_0020char_0020const_0020_003E_002E_007Bctor_007D_003Cunsigned_0020char_002Cclass_0020std_003A_003Afunction_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E_002C0_003E((shared_ptr_003Cunsigned_0020char_0020const_0020_003E*)((ulong)(nint)ptr + 16uL), (unique_ptr_003Cunsigned_0020char_002Cstd_003A_003Afunction_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmUniqueBuffer, 16)));
						buffer = ptr;
					}
					else
					{
						buffer = null;
					}
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.delete(ptr, 32uL);
					throw;
				}
				m_RefCountedBuffer = new RefCountingSharedBuffer(buffer);
			}
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmUniqueBuffer*, void>)(&global::_003CModule_003E.NmBuffers_002ECmUniqueBuffer_002E_007Bdtor_007D), &cmUniqueBuffer);
			throw;
		}
		global::_003CModule_003E.std_002Eunique_ptr_003Cunsigned_0020char_002Cstd_003A_003Afunction_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E_0020_003E_002E_007Bdtor_007D((unique_ptr_003Cunsigned_0020char_002Cstd_003A_003Afunction_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmUniqueBuffer, 16)));
	}

	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_007EReadOnlySharedBuffer();
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
