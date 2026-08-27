using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NmToolkit;
using std;

namespace VideoOS.Toolkit;

public unsafe class DiagnosticsKeyInfo(ImRendererToolkit.ImDiagnosticsKeyInfo* pNativeKeyInfo) : IRendererToolkit.IDiagnosticsKeyInfo, IDisposable
{
	private unsafe ImRendererToolkit.ImDiagnosticsKeyInfo* m_pNativeKeyInfo = pNativeKeyInfo;

	private void _007EDiagnosticsKeyInfo()
	{
	}

	public unsafe virtual IRendererToolkit.PerformanceCounterType GetPerformanceCounterType()
	{
		//IL_000e: Expected I, but got I8
		ImRendererToolkit.ImDiagnosticsKeyInfo* pNativeKeyInfo = m_pNativeKeyInfo;
		ImRendererToolkit.ImDiagnosticsKeyInfo.ePerformanceCounterType result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImRendererToolkit.ImDiagnosticsKeyInfo.ePerformanceCounterType>)(*(ulong*)(*(ulong*)pNativeKeyInfo)))((nint)pNativeKeyInfo);
		GC.KeepAlive(this);
		return (IRendererToolkit.PerformanceCounterType)result;
	}

	public unsafe virtual string GetPerformanceCounterDescription()
	{
		//IL_0017: Expected I, but got I8
		//IL_0018: Expected I8, but got I
		//IL_001e: Expected I, but got I8
		ImRendererToolkit.ImDiagnosticsKeyInfo* pNativeKeyInfo = m_pNativeKeyInfo;
		long num = *(long*)(*(long*)pNativeKeyInfo + 8);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj);
		long num2 = (nint)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*>)num)((nint)pNativeKeyInfo, &obj);
		string result;
		try
		{
			result = new string(global::_003CModule_003E.std_002E_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002E_Myptr((std._String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E*)num2));
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

	public unsafe virtual int GetDisplayLevel()
	{
		//IL_0012: Expected I, but got I8
		ImRendererToolkit.ImDiagnosticsKeyInfo* pNativeKeyInfo = m_pNativeKeyInfo;
		int result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(long*)pNativeKeyInfo + 16)))((nint)pNativeKeyInfo);
		GC.KeepAlive(this);
		return result;
	}

	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (!A_0)
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
