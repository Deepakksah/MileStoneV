using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using std;

namespace VideoOS.Toolkit;

public class D3D9SurfaceRenderingInformation : ImageRenderingInformation, IRendererToolkit.ID3D9SurfaceRenderingInformation
{
	private unsafe shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImD3D9SurfaceRenderingInformation_0020const_0020_003E* m_pD3D9SurfaceRenderingInformation;

	public unsafe D3D9SurfaceRenderingInformation(shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImD3D9SurfaceRenderingInformation_0020const_0020_003E* pD3D9SurfaceRenderingInformation)
	{
		//IL_0044: Expected I, but got I8
		//IL_0020: Expected I, but got I8
		//IL_007f: Expected I, but got I8
		uint num = 0u;
		try
		{
			shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImD3D9SurfaceRenderingInformation_0020const_0020_003E* ptr = (shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImD3D9SurfaceRenderingInformation_0020const_0020_003E*)global::_003CModule_003E.@new(16uL);
			shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImD3D9SurfaceRenderingInformation_0020const_0020_003E* pD3D9SurfaceRenderingInformation2;
			if (ptr != null)
			{
				*(long*)ptr = 0L;
				*(long*)((ulong)(nint)ptr + 8uL) = 0L;
				shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImD3D9SurfaceRenderingInformation_0020const_0020_003E* ptr2 = (shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImD3D9SurfaceRenderingInformation_0020const_0020_003E*)((ulong)(nint)pD3D9SurfaceRenderingInformation + 8uL);
				ulong num2 = *(ulong*)ptr2;
				if (num2 != 0L)
				{
					Interlocked.Increment(ref *(int*)(num2 + 8));
				}
				*(long*)ptr = *(long*)pD3D9SurfaceRenderingInformation;
				*(long*)((ulong)(nint)ptr + 8uL) = *(long*)ptr2;
				pD3D9SurfaceRenderingInformation2 = ptr;
			}
			else
			{
				pD3D9SurfaceRenderingInformation2 = null;
			}
			m_pD3D9SurfaceRenderingInformation = pD3D9SurfaceRenderingInformation2;
			System.Runtime.CompilerServices.Unsafe.SkipInit(out shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImImageRenderingInformation_0020const_0020_003E obj);
			shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImImageRenderingInformation_0020const_0020_003E* ptr3 = &obj;
			shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImImageRenderingInformation_0020const_0020_003E* pImageRenderingInformation = global::_003CModule_003E.std_002Edynamic_pointer_cast_003Cclass_0020NmToolkit_003A_003AImRendererToolkit_003A_003AImImageRenderingInformation_0020const_0020_002Cclass_0020NmToolkit_003A_003AImRendererToolkit_003A_003AImD3D9SurfaceRenderingInformation_0020const_0020_003E(&obj, pD3D9SurfaceRenderingInformation);
			base._002Ector(pImageRenderingInformation);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImD3D9SurfaceRenderingInformation_0020const_0020_003E*, void>)(&global::_003CModule_003E.std_002Eshared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImD3D9SurfaceRenderingInformation_0020const_0020_003E_002E_007Bdtor_007D), pD3D9SurfaceRenderingInformation);
			throw;
		}
		ulong num3 = *(ulong*)((ulong)(nint)pD3D9SurfaceRenderingInformation + 8uL);
		if (num3 != 0L)
		{
			global::_003CModule_003E.std_002E_Ref_count_base_002E_Decref((std._Ref_count_base*)num3);
		}
	}

	private void _007ED3D9SurfaceRenderingInformation()
	{
		_0021D3D9SurfaceRenderingInformation();
	}

	private unsafe void _0021D3D9SurfaceRenderingInformation()
	{
		//IL_002a: Expected I, but got I8
		//IL_0019: Expected I, but got I8
		shared_ptr_003CNmToolkit_003A_003AImRendererToolkit_003A_003AImD3D9SurfaceRenderingInformation_0020const_0020_003E* pD3D9SurfaceRenderingInformation = m_pD3D9SurfaceRenderingInformation;
		if (pD3D9SurfaceRenderingInformation != null)
		{
			ulong num = *(ulong*)((ulong)(nint)pD3D9SurfaceRenderingInformation + 8uL);
			if (num != 0L)
			{
				global::_003CModule_003E.std_002E_Ref_count_base_002E_Decref((std._Ref_count_base*)num);
			}
			global::_003CModule_003E.delete(pD3D9SurfaceRenderingInformation, 16uL);
		}
		m_pD3D9SurfaceRenderingInformation = null;
	}

	public unsafe virtual IntPtr GetSurface()
	{
		//IL_0013: Expected I, but got I8
		//IL_0013: Expected I, but got I8
		long pD3D9SurfaceRenderingInformation = *(long*)m_pD3D9SurfaceRenderingInformation;
		IntPtr result = (IntPtr)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, void*>)(*(ulong*)(*(long*)pD3D9SurfaceRenderingInformation + 104)))((nint)pD3D9SurfaceRenderingInformation);
		GC.KeepAlive(this);
		return result;
	}

	public unsafe virtual IntPtr GetDx11Surface()
	{
		//IL_0013: Expected I, but got I8
		//IL_0013: Expected I, but got I8
		long pD3D9SurfaceRenderingInformation = *(long*)m_pD3D9SurfaceRenderingInformation;
		IntPtr result = (IntPtr)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, void*>)(*(ulong*)(*(long*)pD3D9SurfaceRenderingInformation + 112)))((nint)pD3D9SurfaceRenderingInformation);
		GC.KeepAlive(this);
		return result;
	}

	public unsafe virtual void GetImageViewport(ref int left, ref int top, ref int right, ref int bottom)
	{
		//IL_002d: Expected I, but got I8
		//IL_002d: Expected I, but got I8
		ulong num = 0uL;
		ulong num2 = 0uL;
		ulong num3 = 0uL;
		ulong num4 = 0uL;
		long pD3D9SurfaceRenderingInformation = *(long*)m_pD3D9SurfaceRenderingInformation;
		long num5 = *(long*)(*(long*)pD3D9SurfaceRenderingInformation + 120);
		((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong*, ulong*, ulong*, ulong*, void>)num5)((nint)pD3D9SurfaceRenderingInformation, &num, &num2, &num3, &num4);
		left = (int)num;
		top = (int)num2;
		right = (int)num3;
		bottom = (int)num4;
		GC.KeepAlive(this);
	}

	public unsafe virtual void GetWindowViewport(ref int left, ref int top, ref int right, ref int bottom)
	{
		//IL_0030: Expected I, but got I8
		//IL_0030: Expected I, but got I8
		ulong num = 0uL;
		ulong num2 = 0uL;
		ulong num3 = 0uL;
		ulong num4 = 0uL;
		long pD3D9SurfaceRenderingInformation = *(long*)m_pD3D9SurfaceRenderingInformation;
		long num5 = *(long*)(*(long*)pD3D9SurfaceRenderingInformation + 128);
		((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ulong*, ulong*, ulong*, ulong*, void>)num5)((nint)pD3D9SurfaceRenderingInformation, &num, &num2, &num3, &num4);
		left = (int)num;
		top = (int)num2;
		right = (int)num3;
		bottom = (int)num4;
		GC.KeepAlive(this);
	}

	[HandleProcessCorruptedStateExceptions]
	protected override void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			try
			{
				_0021D3D9SurfaceRenderingInformation();
				return;
			}
			finally
			{
				base.Dispose(A_0: true);
			}
		}
		try
		{
			_0021D3D9SurfaceRenderingInformation();
		}
		finally
		{
			base.Dispose(A_0: false);
		}
	}
}
