using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using NmTimeStamp;
using NmToolkit;
using std;

namespace VideoOS.Toolkit;

public class ImageRenderingInformation : IRendererToolkit.IImageRenderingInformation
{
	private unsafe shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImImageRenderingInformation_0020const_0020_003E* m_pImageRenderingInformation;

	public unsafe ImageRenderingInformation(shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImImageRenderingInformation_0020const_0020_003E* pImageRenderingInformation)
	{
		//IL_0041: Expected I, but got I8
		//IL_001d: Expected I, but got I8
		//IL_006c: Expected I, but got I8
		try
		{
			shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImImageRenderingInformation_0020const_0020_003E* ptr = (shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImImageRenderingInformation_0020const_0020_003E*)global::_003CModule_003E.@new(16uL);
			shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImImageRenderingInformation_0020const_0020_003E* pImageRenderingInformation2;
			if (ptr != null)
			{
				*(long*)ptr = 0L;
				*(long*)((ulong)(nint)ptr + 8uL) = 0L;
				shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImImageRenderingInformation_0020const_0020_003E* ptr2 = (shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImImageRenderingInformation_0020const_0020_003E*)((ulong)(nint)pImageRenderingInformation + 8uL);
				ulong num = *(ulong*)ptr2;
				if (num != 0L)
				{
					Interlocked.Increment(ref *(int*)(num + 8));
				}
				*(long*)ptr = *(long*)pImageRenderingInformation;
				*(long*)((ulong)(nint)ptr + 8uL) = *(long*)ptr2;
				pImageRenderingInformation2 = ptr;
			}
			else
			{
				pImageRenderingInformation2 = null;
			}
			m_pImageRenderingInformation = pImageRenderingInformation2;
			base._002Ector();
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImImageRenderingInformation_0020const_0020_003E*, void>)(&global::_003CModule_003E.std_002Eshared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImImageRenderingInformation_0020const_0020_003E_002E_007Bdtor_007D), pImageRenderingInformation);
			throw;
		}
		ulong num2 = *(ulong*)((ulong)(nint)pImageRenderingInformation + 8uL);
		if (num2 != 0L)
		{
			global::_003CModule_003E.std_002E_Ref_count_base_002E_Decref((std._Ref_count_base*)num2);
		}
	}

	private void _007EImageRenderingInformation()
	{
		_0021ImageRenderingInformation();
	}

	private unsafe void _0021ImageRenderingInformation()
	{
		//IL_002a: Expected I, but got I8
		//IL_0019: Expected I, but got I8
		shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImImageRenderingInformation_0020const_0020_003E* pImageRenderingInformation = m_pImageRenderingInformation;
		if (pImageRenderingInformation != null)
		{
			ulong num = *(ulong*)((ulong)(nint)pImageRenderingInformation + 8uL);
			if (num != 0L)
			{
				global::_003CModule_003E.std_002E_Ref_count_base_002E_Decref((std._Ref_count_base*)num);
			}
			global::_003CModule_003E.delete(pImageRenderingInformation, 16uL);
		}
		m_pImageRenderingInformation = null;
	}

	public unsafe virtual int GetImageSynchronizationTag()
	{
		//IL_0012: Expected I, but got I8
		//IL_0012: Expected I, but got I8
		long pImageRenderingInformation = *(long*)m_pImageRenderingInformation;
		int result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pImageRenderingInformation + 8)))((nint)pImageRenderingInformation);
		GC.KeepAlive(this);
		return result;
	}

	public unsafe virtual IRendererToolkit.ImageModel GetImageModel()
	{
		//IL_0013: Expected I, but got I8
		//IL_0013: Expected I, but got I8
		long pImageRenderingInformation = *(long*)m_pImageRenderingInformation;
		ImRendererToolkit.ImImageRenderingInformation.image_model_t result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImRendererToolkit.ImImageRenderingInformation.image_model_t>)(*(ulong*)(*(long*)pImageRenderingInformation + 16)))((nint)pImageRenderingInformation);
		GC.KeepAlive(this);
		return (IRendererToolkit.ImageModel)result;
	}

	public unsafe virtual IRendererToolkit.HardwareAcceleration GetHardwareAccelerationType()
	{
		//IL_0013: Expected I, but got I8
		//IL_0013: Expected I, but got I8
		long pImageRenderingInformation = *(long*)m_pImageRenderingInformation;
		ImRendererToolkit.hardware_acceleration_t result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImRendererToolkit.hardware_acceleration_t>)(*(ulong*)(*(long*)pImageRenderingInformation + 24)))((nint)pImageRenderingInformation);
		GC.KeepAlive(this);
		return (IRendererToolkit.HardwareAcceleration)result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe virtual bool IsLive()
	{
		//IL_0013: Expected I, but got I8
		//IL_0013: Expected I, but got I8
		long pImageRenderingInformation = *(long*)m_pImageRenderingInformation;
		bool result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte>)(*(ulong*)(*(long*)pImageRenderingInformation + 32)))((nint)pImageRenderingInformation) != 0;
		GC.KeepAlive(this);
		return result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe virtual bool IsFlippedHorizontally()
	{
		//IL_0013: Expected I, but got I8
		//IL_0013: Expected I, but got I8
		long pImageRenderingInformation = *(long*)m_pImageRenderingInformation;
		bool result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte>)(*(ulong*)(*(long*)pImageRenderingInformation + 40)))((nint)pImageRenderingInformation) != 0;
		GC.KeepAlive(this);
		return result;
	}

	[return: MarshalAs(UnmanagedType.U1)]
	public unsafe virtual bool IsFlippedVertically()
	{
		//IL_0013: Expected I, but got I8
		//IL_0013: Expected I, but got I8
		long pImageRenderingInformation = *(long*)m_pImageRenderingInformation;
		bool result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte>)(*(ulong*)(*(long*)pImageRenderingInformation + 48)))((nint)pImageRenderingInformation) != 0;
		GC.KeepAlive(this);
		return result;
	}

	public unsafe virtual DateTime GetImageTime()
	{
		//IL_0017: Expected I, but got I8
		//IL_0017: Expected I, but got I8
		long pImageRenderingInformation = *(long*)m_pImageRenderingInformation;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out NmTimeStamp.CmTimeStamp cmTimeStamp);
		NmTimeStamp.CmTimeStamp* ptr = global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bctor_007D(&cmTimeStamp, ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, long>)(*(ulong*)(*(long*)pImageRenderingInformation + 56)))((nint)pImageRenderingInformation));
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

	public unsafe virtual DateTime GetNextImageTime()
	{
		//IL_0017: Expected I, but got I8
		//IL_0017: Expected I, but got I8
		long pImageRenderingInformation = *(long*)m_pImageRenderingInformation;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out NmTimeStamp.CmTimeStamp cmTimeStamp);
		NmTimeStamp.CmTimeStamp* ptr = global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bctor_007D(&cmTimeStamp, ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, long>)(*(ulong*)(*(long*)pImageRenderingInformation + 64)))((nint)pImageRenderingInformation));
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

	public unsafe virtual DateTime GetPreviousImageTime()
	{
		//IL_0017: Expected I, but got I8
		//IL_0017: Expected I, but got I8
		long pImageRenderingInformation = *(long*)m_pImageRenderingInformation;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out NmTimeStamp.CmTimeStamp cmTimeStamp);
		NmTimeStamp.CmTimeStamp* ptr = global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bctor_007D(&cmTimeStamp, ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, long>)(*(ulong*)(*(long*)pImageRenderingInformation + 72)))((nint)pImageRenderingInformation));
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

	public unsafe virtual void GetOriginalImageSize(ref int width, ref int height)
	{
		//IL_0021: Expected I, but got I8
		//IL_0021: Expected I, but got I8
		ulong num = 0uL;
		ulong num2 = 0uL;
		long pImageRenderingInformation = *(long*)m_pImageRenderingInformation;
		long num3 = *(long*)(*(long*)pImageRenderingInformation + 80);
		((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong*, ulong*, void>)num3)((nint)pImageRenderingInformation, &num, &num2);
		width = (int)num;
		height = (int)num2;
		GC.KeepAlive(this);
	}

	public unsafe virtual IRendererToolkit.DataAvailabilityAtRequestedTime GetDataAvailabilityAtRequestedTime()
	{
		//IL_0013: Expected I, but got I8
		//IL_0013: Expected I, but got I8
		long pImageRenderingInformation = *(long*)m_pImageRenderingInformation;
		ImRendererToolkit.ImImageRenderingInformation.eDataAvailabilityAtRequestedTime result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImRendererToolkit.ImImageRenderingInformation.eDataAvailabilityAtRequestedTime>)(*(ulong*)(*(long*)pImageRenderingInformation + 88)))((nint)pImageRenderingInformation);
		GC.KeepAlive(this);
		return (IRendererToolkit.DataAvailabilityAtRequestedTime)result;
	}

	public unsafe virtual string GetVideoCodec()
	{
		//IL_0019: Expected I, but got I8
		//IL_0019: Expected I, but got I8
		//IL_001a: Expected I8, but got I
		//IL_0020: Expected I, but got I8
		long pImageRenderingInformation = *(long*)m_pImageRenderingInformation;
		long num = *(long*)(*(long*)pImageRenderingInformation + 96);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj);
		long num2 = (nint)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*>)num)((nint)pImageRenderingInformation, &obj);
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

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_0021ImageRenderingInformation();
			return;
		}
		try
		{
			_0021ImageRenderingInformation();
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

	~ImageRenderingInformation()
	{
		Dispose(A_0: false);
	}
}
