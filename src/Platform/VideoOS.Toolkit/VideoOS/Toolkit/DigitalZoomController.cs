using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using NmToolkit;
using msclr.interop;
using std;

namespace VideoOS.Toolkit;

public class DigitalZoomController : IDigitalZoomController, IDisposable
{
	private unsafe shared_ptr_003CNmToolkit_003A_003AImDigitalZoomController_003E* m_pDigitalZoomController;

	public unsafe DigitalZoomController(shared_ptr_003CNmToolkit_003A_003AImDigitalZoomController_003E* pDigitalZoomController)
	{
		//IL_0041: Expected I, but got I8
		//IL_001d: Expected I, but got I8
		//IL_006c: Expected I, but got I8
		try
		{
			shared_ptr_003CNmToolkit_003A_003AImDigitalZoomController_003E* ptr = (shared_ptr_003CNmToolkit_003A_003AImDigitalZoomController_003E*)global::_003CModule_003E.@new(16uL);
			shared_ptr_003CNmToolkit_003A_003AImDigitalZoomController_003E* pDigitalZoomController2;
			if (ptr != null)
			{
				*(long*)ptr = 0L;
				*(long*)((ulong)(nint)ptr + 8uL) = 0L;
				shared_ptr_003CNmToolkit_003A_003AImDigitalZoomController_003E* ptr2 = (shared_ptr_003CNmToolkit_003A_003AImDigitalZoomController_003E*)((ulong)(nint)pDigitalZoomController + 8uL);
				ulong num = *(ulong*)ptr2;
				if (num != 0L)
				{
					Interlocked.Increment(ref *(int*)(num + 8));
				}
				*(long*)ptr = *(long*)pDigitalZoomController;
				*(long*)((ulong)(nint)ptr + 8uL) = *(long*)ptr2;
				pDigitalZoomController2 = ptr;
			}
			else
			{
				pDigitalZoomController2 = null;
			}
			m_pDigitalZoomController = pDigitalZoomController2;
			base._002Ector();
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<shared_ptr_003CNmToolkit_003A_003AImDigitalZoomController_003E*, void>)(&global::_003CModule_003E.std_002Eshared_ptr_003CNmToolkit_003A_003AImDigitalZoomController_003E_002E_007Bdtor_007D), pDigitalZoomController);
			throw;
		}
		ulong num2 = *(ulong*)((ulong)(nint)pDigitalZoomController + 8uL);
		if (num2 != 0L)
		{
			global::_003CModule_003E.std_002E_Ref_count_base_002E_Decref((std._Ref_count_base*)num2);
		}
	}

	private void _007EDigitalZoomController()
	{
		_0021DigitalZoomController();
	}

	private unsafe void _0021DigitalZoomController()
	{
		//IL_0019: Expected I, but got I8
		shared_ptr_003CNmToolkit_003A_003AImDigitalZoomController_003E* pDigitalZoomController = m_pDigitalZoomController;
		if (pDigitalZoomController != null)
		{
			ulong num = *(ulong*)((ulong)(nint)pDigitalZoomController + 8uL);
			if (num != 0L)
			{
				global::_003CModule_003E.std_002E_Ref_count_base_002E_Decref((std._Ref_count_base*)num);
			}
			global::_003CModule_003E.delete(pDigitalZoomController, 16uL);
		}
		GC.KeepAlive(this);
	}

	public unsafe virtual void SetAbsolutePosition(string position)
	{
		//IL_0023: Expected I, but got I8
		//IL_0023: Expected I, but got I8
		uint num = 0u;
		long pDigitalZoomController = *(long*)m_pDigitalZoomController;
		long num2 = pDigitalZoomController;
		long num3 = *(long*)(*(ulong*)pDigitalZoomController);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out utf8string_t utf8string_t2);
		utf8string_t* ptr = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020msclr_003A_003Ainterop_003A_003Autf8string_t_002Cclass_0020System_003A_003AString_0020_005E_003E(&utf8string_t2, &position);
		try
		{
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, void>)num3)((nint)num2, (std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)ptr);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<utf8string_t*, void>)(&global::_003CModule_003E.msclr_002Einterop_002Eutf8string_t_002E_007Bdtor_007D), &utf8string_t2);
			throw;
		}
		try
		{
			global::_003CModule_003E.std_002Ebasic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E_002E_Tidy_deallocate((std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*)(&utf8string_t2));
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<std._Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E*, void>)(&global::_003CModule_003E.std_002E_Compressed_pair_003Cstd_003A_003Aallocator_003Cchar_003E_002Cstd_003A_003A_String_val_003Cstd_003A_003A_Simple_types_003Cchar_003E_0020_003E_002C1_003E_002E_007Bdtor_007D), &utf8string_t2);
			throw;
		}
		GC.KeepAlive(this);
	}

	public unsafe virtual string GetAbsolutePosition()
	{
		//IL_0018: Expected I, but got I8
		//IL_0018: Expected I, but got I8
		//IL_0019: Expected I8, but got I
		//IL_001f: Expected I, but got I8
		long pDigitalZoomController = *(long*)m_pDigitalZoomController;
		long num = *(long*)(*(long*)pDigitalZoomController + 8);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E obj);
		long num2 = (nint)((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*, std.basic_string_003Cchar_002Cstd_003A_003Achar_traits_003Cchar_003E_002Cstd_003A_003Aallocator_003Cchar_003E_0020_003E*>)num)((nint)pDigitalZoomController, &obj);
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

	public unsafe virtual void MoveRelative(double x, double y)
	{
		//IL_0019: Expected I, but got I8
		//IL_0019: Expected I, but got I8
		long pDigitalZoomController = *(long*)m_pDigitalZoomController;
		long num = *(long*)(*(long*)pDigitalZoomController + 16);
		((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, double, double, void>)num)((nint)pDigitalZoomController, x, y);
		GC.KeepAlive(this);
	}

	public unsafe virtual void MoveRelative(double left, double top, double right, double bottom)
	{
		//IL_001c: Expected I, but got I8
		//IL_001c: Expected I, but got I8
		long pDigitalZoomController = *(long*)m_pDigitalZoomController;
		long num = *(long*)(*(long*)pDigitalZoomController + 24);
		((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, double, double, double, double, void>)num)((nint)pDigitalZoomController, left, top, right, bottom);
		GC.KeepAlive(this);
	}

	public unsafe virtual void Move(IDigitalZoomController.MoveCommand command)
	{
		//IL_0018: Expected I, but got I8
		//IL_0018: Expected I, but got I8
		long pDigitalZoomController = *(long*)m_pDigitalZoomController;
		long num = *(long*)(*(long*)pDigitalZoomController + 32);
		((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImDigitalZoomController.eMoveCommand, void>)num)((nint)pDigitalZoomController, (ImDigitalZoomController.eMoveCommand)command);
		GC.KeepAlive(this);
	}

	public unsafe virtual void ConvertDewarpedCoordinates(double xCoordinate, double yCoordinate, ref double outXCoordinate, ref double outYCoordinate)
	{
		//IL_001d: Expected I, but got I8
		//IL_001d: Expected I, but got I8
		long pDigitalZoomController = *(long*)m_pDigitalZoomController;
		long num = *(long*)(*(long*)pDigitalZoomController + 48);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out double num2);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out double num3);
		((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, double, double, double*, double*, void>)num)((nint)pDigitalZoomController, xCoordinate, yCoordinate, &num2, &num3);
		outXCoordinate = num2;
		outYCoordinate = num3;
		GC.KeepAlive(this);
	}

	public unsafe virtual IDigitalZoomController.Point[] ConvertDewarpedPoints(IDigitalZoomController.Point[] points)
	{
		//IL_0098: Expected I, but got I8
		//IL_0098: Expected I, but got I8
		//IL_00c6: Expected I, but got I8
		ulong count = (ulong)points.LongLength;
		System.Runtime.CompilerServices.Unsafe.SkipInit(out vector_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_003E_0020_003E obj);
		*(long*)(&obj) = 0L;
		System.Runtime.CompilerServices.Unsafe.As<vector_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_003E_0020_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 8)) = 0L;
		System.Runtime.CompilerServices.Unsafe.As<vector_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_003E_0020_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 16)) = 0L;
		global::_003CModule_003E.std_002Evector_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_003E_0020_003E_002E_Construct_n_003C_003E(&obj, count);
		IDigitalZoomController.Point[] array;
		try
		{
			int num = 0;
			if (0 < (nint)points.LongLength)
			{
				long num2 = 0L;
				System.Runtime.CompilerServices.Unsafe.SkipInit(out ImDigitalZoomController.Point point2);
				do
				{
					IDigitalZoomController.Point point = points[num];
					double y = point.y;
					*(double*)(&point2) = point.x;
					System.Runtime.CompilerServices.Unsafe.As<ImDigitalZoomController.Point, double>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref point2, 8)) = y;
					// IL cpblk instruction
					System.Runtime.CompilerServices.Unsafe.CopyBlock(num2 + *(long*)(&obj), ref point2, 16);
					num++;
					num2 += 16;
				}
				while (num < (nint)points.LongLength);
			}
			long pDigitalZoomController = *(long*)m_pDigitalZoomController;
			long num3 = pDigitalZoomController;
			long num4 = *(long*)(*(long*)pDigitalZoomController + 56);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out vector_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_003E_0020_003E obj2);
			vector_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_003E_0020_003E* ptr = &obj2;
			vector_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_003E_0020_003E* ptr2 = global::_003CModule_003E.std_002Evector_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_003E_0020_003E_002E_007Bctor_007D(&obj2, &obj);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out vector_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_003E_0020_003E obj3);
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, vector_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_003E_0020_003E*, vector_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_003E_0020_003E*, vector_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_003E_0020_003E*>)num4)((nint)num3, &obj3, ptr2);
			try
			{
				long num5 = System.Runtime.CompilerServices.Unsafe.As<vector_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_003E_0020_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj3, 8)) - *(long*)(&obj3) >> 4;
				array = new IDigitalZoomController.Point[(int)num5];
				int num6 = 0;
				if (0 < (int)num5)
				{
					long num7 = 0L;
					do
					{
						long num8 = num7 + *(long*)(&obj3);
						ImDigitalZoomController.Point* ptr3 = (ImDigitalZoomController.Point*)num8;
						IDigitalZoomController.Point point3 = new IDigitalZoomController.Point(*(double*)num8, *(double*)((ulong)(nint)ptr3 + 8uL));
						array[num6] = point3;
						num6++;
						num7 += 16;
					}
					while (num6 < (int)(System.Runtime.CompilerServices.Unsafe.As<vector_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_003E_0020_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj3, 8)) - *(long*)(&obj3) >> 4));
				}
				GC.KeepAlive(this);
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<vector_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Evector_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_003E_0020_003E_002E_007Bdtor_007D), &obj3);
				throw;
			}
			global::_003CModule_003E.std_002Evector_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_003E_0020_003E_002E_Tidy(&obj3);
		}
		catch
		{
			//try-fault
			global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<vector_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Evector_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_003E_0020_003E_002E_007Bdtor_007D), &obj);
			throw;
		}
		global::_003CModule_003E.std_002Evector_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImDigitalZoomController_003A_003APoint_003E_0020_003E_002E_Tidy(&obj);
		return array;
	}

	[HandleProcessCorruptedStateExceptions]
	protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
	{
		if (A_0)
		{
			_0021DigitalZoomController();
			return;
		}
		try
		{
			_0021DigitalZoomController();
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

	~DigitalZoomController()
	{
		Dispose(A_0: false);
	}
}
