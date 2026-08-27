using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using NmTimeStamp;
using std;

namespace VideoOS.Toolkit;

public class AudioInformation : IAudioToolkit.IAudioInformation
{
	private unsafe shared_ptr_003CNmToolkit_003A_003AImAudioToolkit_003A_003AImAudioInformation_0020const_0020_003E* m_pAudioInformation;

	public unsafe AudioInformation(shared_ptr_003CNmToolkit_003A_003AImAudioToolkit_003A_003AImAudioInformation_0020const_0020_003E* pAudioInformation)
	{
		//IL_0041: Expected I, but got I8
		//IL_001d: Expected I, but got I8
		//IL_006c: Expected I, but got I8
		try
		{
			shared_ptr_003CNmToolkit_003A_003AImAudioToolkit_003A_003AImAudioInformation_0020const_0020_003E* ptr = (shared_ptr_003CNmToolkit_003A_003AImAudioToolkit_003A_003AImAudioInformation_0020const_0020_003E*)global::_003CModule_003E.@new(16uL);
			shared_ptr_003CNmToolkit_003A_003AImAudioToolkit_003A_003AImAudioInformation_0020const_0020_003E* pAudioInformation2;
			if (ptr != null)
			{
				*(long*)ptr = 0L;
				*(long*)((ulong)(nint)ptr + 8uL) = 0L;
				shared_ptr_003CNmToolkit_003A_003AImAudioToolkit_003A_003AImAudioInformation_0020const_0020_003E* ptr2 = (shared_ptr_003CNmToolkit_003A_003AImAudioToolkit_003A_003AImAudioInformation_0020const_0020_003E*)((ulong)(nint)pAudioInformation + 8uL);
				ulong num = *(ulong*)ptr2;
				if (num != 0L)
				{
					Interlocked.Increment(ref *(int*)(num + 8));
				}
				*(long*)ptr = *(long*)pAudioInformation;
				*(long*)((ulong)(nint)ptr + 8uL) = *(long*)ptr2;
				pAudioInformation2 = ptr;
			}
			else
			{
				pAudioInformation2 = null;
			}
			m_pAudioInformation = pAudioInformation2;
			base._002Ector();
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<shared_ptr_003CNmToolkit_003A_003AImAudioToolkit_003A_003AImAudioInformation_0020const_0020_003E*, void>)(&global::_003CModule_003E.std_002Eshared_ptr_003CNmToolkit_003A_003AImAudioToolkit_003A_003AImAudioInformation_0020const_0020_003E_002E_007Bdtor_007D), pAudioInformation);
			throw;
		}
		ulong num2 = *(ulong*)((ulong)(nint)pAudioInformation + 8uL);
		if (num2 != 0L)
		{
			global::_003CModule_003E.std_002E_Ref_count_base_002E_Decref((std._Ref_count_base*)num2);
		}
	}

	private void _007EAudioInformation()
	{
		_0021AudioInformation();
	}

	private unsafe void _0021AudioInformation()
	{
		//IL_002a: Expected I, but got I8
		//IL_0019: Expected I, but got I8
		shared_ptr_003CNmToolkit_003A_003AImAudioToolkit_003A_003AImAudioInformation_0020const_0020_003E* pAudioInformation = m_pAudioInformation;
		if (pAudioInformation != null)
		{
			ulong num = *(ulong*)((ulong)(nint)pAudioInformation + 8uL);
			if (num != 0L)
			{
				global::_003CModule_003E.std_002E_Ref_count_base_002E_Decref((std._Ref_count_base*)num);
			}
			global::_003CModule_003E.delete(pAudioInformation, 16uL);
		}
		m_pAudioInformation = null;
	}

	public unsafe virtual IntPtr RawData()
	{
		//IL_0012: Expected I, but got I8
		//IL_0012: Expected I, but got I8
		long pAudioInformation = *(long*)m_pAudioInformation;
		byte* value = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte*>)(*(ulong*)(*(long*)pAudioInformation + 8)))((nint)pAudioInformation);
		IntPtr result = new IntPtr(value);
		GC.KeepAlive(this);
		return result;
	}

	public unsafe virtual string Codec()
	{
		//IL_0019: Expected I, but got I8
		//IL_0019: Expected I, but got I8
		//IL_001a: Expected I8, but got I
		//IL_0020: Expected I, but got I8
		long pAudioInformation = *(long*)m_pAudioInformation;
		long num = *(long*)(*(long*)pAudioInformation + 48);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj);
		long num2 = (nint)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*>)num)((nint)pAudioInformation, &obj);
		string result;
		try
		{
			result = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020System_003A_003AString_0020_005E_002Cclass_0020std_003A_003Abasic_string_003Cchar_002Cstruct_0020std_003A_003Achar_traits_003Cchar_003E_002Cclass_0020std_003A_003Aallocator_003Cchar_003E_0020_003E_0020_003E((std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)num2);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_007Bdtor_007D), &obj);
			throw;
		}
		try
		{
			global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_Tidy_deallocate(&obj);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std._Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E*, void>)(&global::_003CModule_003E.std_002E_Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E_002E_007Bdtor_007D), &obj);
			throw;
		}
		GC.KeepAlive(this);
		return result;
	}

	public unsafe virtual long NumberOfChannels()
	{
		//IL_0013: Expected I, but got I8
		//IL_0013: Expected I, but got I8
		long pAudioInformation = *(long*)m_pAudioInformation;
		ulong result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong>)(*(ulong*)(*(long*)pAudioInformation + 16)))((nint)pAudioInformation);
		GC.KeepAlive(this);
		return (long)result;
	}

	public unsafe virtual long SamplingRate()
	{
		//IL_0013: Expected I, but got I8
		//IL_0013: Expected I, but got I8
		long pAudioInformation = *(long*)m_pAudioInformation;
		ulong result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong>)(*(ulong*)(*(long*)pAudioInformation + 24)))((nint)pAudioInformation);
		GC.KeepAlive(this);
		return (long)result;
	}

	public unsafe virtual long BitDepth()
	{
		//IL_0013: Expected I, but got I8
		//IL_0013: Expected I, but got I8
		long pAudioInformation = *(long*)m_pAudioInformation;
		ulong result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong>)(*(ulong*)(*(long*)pAudioInformation + 32)))((nint)pAudioInformation);
		GC.KeepAlive(this);
		return (long)result;
	}

	public unsafe virtual long DataSize()
	{
		//IL_0013: Expected I, but got I8
		//IL_0013: Expected I, but got I8
		long pAudioInformation = *(long*)m_pAudioInformation;
		ulong result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong>)(*(ulong*)(*(long*)pAudioInformation + 40)))((nint)pAudioInformation);
		GC.KeepAlive(this);
		return (long)result;
	}

	public unsafe virtual DateTime TimeStamp()
	{
		//IL_0017: Expected I, but got I8
		//IL_0017: Expected I, but got I8
		long pAudioInformation = *(long*)m_pAudioInformation;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out NmTimeStamp.CmTimeStamp cmTimeStamp);
		NmTimeStamp.CmTimeStamp* ptr = global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bctor_007D(&cmTimeStamp, ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, long>)(*(ulong*)(*(long*)pAudioInformation + 64)))((nint)pAudioInformation));
		DateTime result;
		try
		{
			result = new DateTime((global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002EGetMillisecondsSinceTheEpoch(ptr) + 62135596800000L) * 10000, DateTimeKind.Utc);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<NmTimeStamp.CmTimeStamp*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D), &cmTimeStamp);
			throw;
		}
		global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D(&cmTimeStamp);
		GC.KeepAlive(this);
		return result;
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_0021AudioInformation();
			return;
		}
		try
		{
			_0021AudioInformation();
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

	~AudioInformation()
	{
		Dispose(A_0: false);
	}
}
