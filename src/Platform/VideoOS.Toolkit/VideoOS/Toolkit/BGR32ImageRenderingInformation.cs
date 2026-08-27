using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using std;

namespace VideoOS.Toolkit;

public class BGR32ImageRenderingInformation : ImageRenderingInformation, IRendererToolkit.IBGR32ImageRenderingInformation
{
	private unsafe shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImBGR32ImageRenderingInformation_0020const_0020_003E* m_pBGR32ImageRenderingInformation;

	public unsafe BGR32ImageRenderingInformation(shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImBGR32ImageRenderingInformation_0020const_0020_003E* pBGR32ImageRenderingInformation)
	{
		//IL_0044: Expected I, but got I8
		//IL_0020: Expected I, but got I8
		//IL_007f: Expected I, but got I8
		uint num = 0u;
		try
		{
			shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImBGR32ImageRenderingInformation_0020const_0020_003E* ptr = (shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImBGR32ImageRenderingInformation_0020const_0020_003E*)global::_003CModule_003E.@new(16uL);
			shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImBGR32ImageRenderingInformation_0020const_0020_003E* pBGR32ImageRenderingInformation2;
			if (ptr != null)
			{
				*(long*)ptr = 0L;
				*(long*)((ulong)(nint)ptr + 8uL) = 0L;
				shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImBGR32ImageRenderingInformation_0020const_0020_003E* ptr2 = (shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImBGR32ImageRenderingInformation_0020const_0020_003E*)((ulong)(nint)pBGR32ImageRenderingInformation + 8uL);
				ulong num2 = *(ulong*)ptr2;
				if (num2 != 0L)
				{
					Interlocked.Increment(ref *(int*)(num2 + 8));
				}
				*(long*)ptr = *(long*)pBGR32ImageRenderingInformation;
				*(long*)((ulong)(nint)ptr + 8uL) = *(long*)ptr2;
				pBGR32ImageRenderingInformation2 = ptr;
			}
			else
			{
				pBGR32ImageRenderingInformation2 = null;
			}
			m_pBGR32ImageRenderingInformation = pBGR32ImageRenderingInformation2;
			System.Runtime.CompilerServices.Unsafe.SkipInit(out shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImImageRenderingInformation_0020const_0020_003E obj);
			shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImImageRenderingInformation_0020const_0020_003E* ptr3 = &obj;
			shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImImageRenderingInformation_0020const_0020_003E* pImageRenderingInformation = global::_003CModule_003E.std_002Edynamic_pointer_cast_003Cclass_0020NmToolkit_003A_003AImRendererToolkit_003A_003AImImageRenderingInformation_0020const_0020_002Cclass_0020NmToolkit_003A_003AImRendererToolkit_003A_003AImBGR32ImageRenderingInformation_0020const_0020_003E(&obj, pBGR32ImageRenderingInformation);
			base._002Ector(pImageRenderingInformation);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImBGR32ImageRenderingInformation_0020const_0020_003E*, void>)(&global::_003CModule_003E.std_002Eshared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImBGR32ImageRenderingInformation_0020const_0020_003E_002E_007Bdtor_007D), pBGR32ImageRenderingInformation);
			throw;
		}
		ulong num3 = *(ulong*)((ulong)(nint)pBGR32ImageRenderingInformation + 8uL);
		if (num3 != 0L)
		{
			global::_003CModule_003E.std_002E_Ref_count_base_002E_Decref((std._Ref_count_base*)num3);
		}
	}

	private void _007EBGR32ImageRenderingInformation()
	{
		_0021BGR32ImageRenderingInformation();
	}

	private unsafe void _0021BGR32ImageRenderingInformation()
	{
		//IL_002a: Expected I, but got I8
		//IL_0019: Expected I, but got I8
		shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImBGR32ImageRenderingInformation_0020const_0020_003E* pBGR32ImageRenderingInformation = m_pBGR32ImageRenderingInformation;
		if (pBGR32ImageRenderingInformation != null)
		{
			ulong num = *(ulong*)((ulong)(nint)pBGR32ImageRenderingInformation + 8uL);
			if (num != 0L)
			{
				global::_003CModule_003E.std_002E_Ref_count_base_002E_Decref((std._Ref_count_base*)num);
			}
			global::_003CModule_003E.delete(pBGR32ImageRenderingInformation, 16uL);
		}
		m_pBGR32ImageRenderingInformation = null;
	}

	public unsafe virtual IntPtr GetBuffer()
	{
		//IL_0013: Expected I, but got I8
		//IL_0013: Expected I, but got I8
		long pBGR32ImageRenderingInformation = *(long*)m_pBGR32ImageRenderingInformation;
		IntPtr result = (IntPtr)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, byte*>)(*(ulong*)(*(long*)pBGR32ImageRenderingInformation + 104)))((nint)pBGR32ImageRenderingInformation);
		GC.KeepAlive(this);
		return result;
	}

	public unsafe virtual void GetImageSize(ref int width, ref int height, ref int stride)
	{
		//IL_0027: Expected I, but got I8
		//IL_0027: Expected I, but got I8
		ulong num = 0uL;
		ulong num2 = 0uL;
		ulong num3 = 0uL;
		long pBGR32ImageRenderingInformation = *(long*)m_pBGR32ImageRenderingInformation;
		long num4 = *(long*)(*(long*)pBGR32ImageRenderingInformation + 112);
		((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong*, ulong*, ulong*, void>)num4)((nint)pBGR32ImageRenderingInformation, &num, &num2, &num3);
		width = (int)num;
		height = (int)num2;
		stride = (int)num3;
		GC.KeepAlive(this);
	}

	[HandleProcessCorruptedStateExceptions]
	protected override void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			try
			{
				_0021BGR32ImageRenderingInformation();
				return;
			}
			finally
			{
				base.Dispose(A_0: true);
			}
		}
		try
		{
			_0021BGR32ImageRenderingInformation();
		}
		finally
		{
			base.Dispose(A_0: false);
		}
	}
}
