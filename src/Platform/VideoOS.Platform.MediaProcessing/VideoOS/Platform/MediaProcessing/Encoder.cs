using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using NmBuffers;
using NmCountedPtr;
using NmGenericByteData;
using NmMediaProcessing;
using VideoOS.Platform.Primitives;
using std;

namespace VideoOS.Platform.MediaProcessing;

public class Encoder : IDisposable
{
	protected unsafe CmSimpleJPEGEncoderWrapper* m_pEncoder;

	public unsafe Encoder()
	{
		//IL_0008: Expected I8, but got I
		//IL_0010: Expected I, but got I8
		//IL_0095: Expected I, but got I8
		//IL_006a: Expected I, but got I8
		//IL_002e: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		m_pEncoder = null;
		base._002Ector();
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
		try
		{
			CmSimpleJPEGEncoderWrapper* ptr = (CmSimpleJPEGEncoderWrapper*)global::_003CModule_003E.@new(16uL);
			CmSimpleJPEGEncoderWrapper* ptr2;
			try
			{
				ptr2 = ((ptr == null) ? null : global::_003CModule_003E.NmMediaProcessing_002ECmSimpleJPEGEncoderWrapper_002E_007Bctor_007D(ptr));
				CmSimpleJPEGEncoderWrapper* ptr3 = ptr2;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.delete(ptr, 16uL);
				throw;
			}
			m_pEncoder = ptr2;
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
		{
			uint num2 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new Error((exception*)error);
				}
				catch when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num2 != 0;
				}).Invoke())
				{
				}
				if (num2 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
			}
		}
	}

	private void _007EEncoder()
	{
		_0021Encoder();
	}

	private unsafe void _0021Encoder()
	{
		//IL_001f: Expected I, but got I8
		//IL_0016: Expected I, but got I8
		CmSimpleJPEGEncoderWrapper* pEncoder = m_pEncoder;
		if (pEncoder != null)
		{
			long num = *(long*)(*(ulong*)pEncoder);
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, uint, void*>)num)((nint)pEncoder, 1u);
		}
		m_pEncoder = null;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe bool EncodeImageToJpeg(Image image, int qualityLevel, [MarshalAs(UnmanagedType.U1)] bool bPreservePicFormat, out GenericByteData result)
	{
		//Discarded unreachable code: IL_0129
		//IL_0009: Expected I8, but got I
		//IL_00f6: Expected I, but got I8
		//IL_00ca: Expected I, but got I8
		//IL_001b: Expected I, but got I8
		//IL_0056: Expected I, but got I8
		//IL_0064: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (image == null)
		{
			throw new ArgumentNullException();
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out CmCountedPtrTSFast_003CNmGenericByteData_003A_003ACmGenericVideoByteDataBlock_003E cmCountedPtrTSFast_003CNmGenericByteData_003A_003ACmGenericVideoByteDataBlock_003E);
		global::_003CModule_003E.NmCountedPtr_002ECmCountedPtrTSFast_003CNmGenericByteData_003A_003ACmGenericVideoByteDataBlock_003E_002E_007Bctor_007D(&cmCountedPtrTSFast_003CNmGenericByteData_003A_003ACmGenericVideoByteDataBlock_003E, null);
		byte b = 0;
		try
		{
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error);
			try
			{
				CmSimpleJPEGEncoderWrapper* native = GetNative();
				CmImage* native2 = image.GetNative();
				if (!global::_003CModule_003E.NmMediaProcessing_002ECmSimpleJPEGEncoderWrapper_002EEncode(native, native2, &cmCountedPtrTSFast_003CNmGenericByteData_003A_003ACmGenericVideoByteDataBlock_003E, (ulong)qualityLevel, bPreservePicFormat, false))
				{
					goto IL_008c;
				}
				byte b2 = ((System.Runtime.CompilerServices.Unsafe.As<CmCountedPtrTSFast_003CNmGenericByteData_003A_003ACmGenericVideoByteDataBlock_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmCountedPtrTSFast_003CNmGenericByteData_003A_003ACmGenericVideoByteDataBlock_003E, 8)) == 0) ? ((byte)1) : ((byte)0));
				byte b3 = b2;
				if (b2 != 0 || !global::_003CModule_003E.NmGenericByteData_002ECmGenericByteData_002EGetIsByteDataOwnedByObject((NmGenericByteData.CmGenericByteData*)System.Runtime.CompilerServices.Unsafe.As<CmCountedPtrTSFast_003CNmGenericByteData_003A_003ACmGenericVideoByteDataBlock_003E, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmCountedPtrTSFast_003CNmGenericByteData_003A_003ACmGenericVideoByteDataBlock_003E, 8))))
				{
					goto IL_008c;
				}
				System.Runtime.CompilerServices.Unsafe.SkipInit(out CmUniqueBuffer cmUniqueBuffer);
				CmUniqueBuffer* buffer = global::_003CModule_003E.NmGenericByteData_002ECmGenericByteData_002ETakeMemoryOwnership((NmGenericByteData.CmGenericByteData*)System.Runtime.CompilerServices.Unsafe.As<CmCountedPtrTSFast_003CNmGenericByteData_003A_003ACmGenericVideoByteDataBlock_003E, ulong>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmCountedPtrTSFast_003CNmGenericByteData_003A_003ACmGenericVideoByteDataBlock_003E, 8)), &cmUniqueBuffer);
				try
				{
					result = new GenericByteData(buffer);
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmUniqueBuffer*, void>)(&global::_003CModule_003E.NmBuffers_002ECmUniqueBuffer_002E_007Bdtor_007D), &cmUniqueBuffer);
					throw;
				}
				global::_003CModule_003E.std_002Eunique_ptr_003Cunsigned_0020char_002Cstd_003A_003Afunction_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E_0020_003E_002E_007Bdtor_007D((std.unique_ptr_003Cunsigned_0020char_002Cstd_003A_003Afunction_003Cvoid_0020__cdecl_0028unsigned_0020char_0020_002A_0029_003E_0020_003E*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref cmUniqueBuffer, 16)));
				goto end_IL_001e;
				IL_008c:
				GC.KeepAlive(this);
				bool flag = false;
				b = 1;
				global::_003CModule_003E.NmCountedPtr_002ECmCountedPtr_003CNmGenericByteData_003A_003ACmGenericVideoByteDataBlock_002CNmCountedPtr_003A_003ACmInterlockedReferenceCount_003E_002EDispose((CmCountedPtr_003CNmGenericByteData_003A_003ACmGenericVideoByteDataBlock_002CNmCountedPtr_003A_003ACmInterlockedReferenceCount_003E*)(&cmCountedPtrTSFast_003CNmGenericByteData_003A_003ACmGenericVideoByteDataBlock_003E));
				return false;
				end_IL_001e:;
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error) != 0)
			{
				uint num2 = 0u;
				global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
				try
				{
					try
					{
						throw new Error((exception*)error);
					}
					catch when (((Func<bool>)delegate
					{
						// Could not convert BlockContainer to single expression
						num2 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
						return (byte)num2 != 0;
					}).Invoke())
					{
					}
					if (num2 != 0)
					{
						throw;
					}
				}
				finally
				{
					global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num2);
				}
			}
			GC.KeepAlive(result);
			GC.KeepAlive(image);
			GC.KeepAlive(this);
		}
		catch
		{
			//try-fault
			if (b == 0)
			{
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<CmCountedPtrTSFast_003CNmGenericByteData_003A_003ACmGenericVideoByteDataBlock_003E*, void>)(&global::_003CModule_003E.NmCountedPtr_002ECmCountedPtrTSFast_003CNmGenericByteData_003A_003ACmGenericVideoByteDataBlock_003E_002E_007Bdtor_007D), &cmCountedPtrTSFast_003CNmGenericByteData_003A_003ACmGenericVideoByteDataBlock_003E);
			}
			throw;
		}
		global::_003CModule_003E.NmCountedPtr_002ECmCountedPtr_003CNmGenericByteData_003A_003ACmGenericVideoByteDataBlock_002CNmCountedPtr_003A_003ACmInterlockedReferenceCount_003E_002EDispose((CmCountedPtr_003CNmGenericByteData_003A_003ACmGenericVideoByteDataBlock_002CNmCountedPtr_003A_003ACmInterlockedReferenceCount_003E*)(&cmCountedPtrTSFast_003CNmGenericByteData_003A_003ACmGenericVideoByteDataBlock_003E));
		return true;
	}

	public unsafe CmSimpleJPEGEncoderWrapper* GetNative()
	{
		CmSimpleJPEGEncoderWrapper* pEncoder = m_pEncoder;
		if (pEncoder == null)
		{
			throw new ObjectDisposedException("VideoOS.Platform.MediaProcessing.Encoder");
		}
		return pEncoder;
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_0021Encoder();
			return;
		}
		try
		{
			_0021Encoder();
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

	~Encoder()
	{
		Dispose(A_0: false);
	}
}
