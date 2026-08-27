using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NmToolkit;
using std;

namespace VideoOS.Toolkit;

public unsafe class MediaDataVector(ImMediaDataVector* data) : MediaData((ImMediaData*)data), IMediaDataVector
{
	private unsafe ImMediaDataVector* m_MediaDataVector = data;

	private IMediaData[] m_ManagedMediaDataVector;

	private bool m_HasCreatedManagedVector = false;

	public unsafe virtual IMediaData[] GetVector()
	{
		//IL_0008: Expected I8, but got I
		//IL_0176: Expected I, but got I8
		//IL_014b: Expected I, but got I8
		//IL_01c8: Expected I, but got I8
		//IL_019d: Expected I, but got I8
		//IL_0048: Expected I, but got I8
		//IL_00b5: Expected I, but got I8
		//IL_00c5: Expected I, but got I8
		//IL_00c5: Expected I, but got I8
		long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
		if (m_MediaDataVector == null)
		{
			throw new ArgumentNullException("Internal native media data vector is uninitialized.");
		}
		if (m_HasCreatedManagedVector)
		{
			return m_ManagedMediaDataVector;
		}
		System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
		System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
		try
		{
			ImMediaDataVector* mediaDataVector = m_MediaDataVector;
			long num2 = *(long*)(*(long*)mediaDataVector + 72);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out vector_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_003E_0020_003E obj);
			((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, vector_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_003E_0020_003E*, vector_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_003E_0020_003E*>)num2)((nint)mediaDataVector, &obj);
			IMediaData[] managedMediaDataVector;
			try
			{
				_Vector_val_003Cstd_003A_003A_Simple_types_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_003E_0020_003E* ptr = (_Vector_val_003Cstd_003A_003A_Simple_types_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_003E_0020_003E*)(&obj);
				ulong num3 = (ulong)(System.Runtime.CompilerServices.Unsafe.As<vector_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_003E_0020_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 8)) - *(long*)(&obj) >> 3);
				ulong num4 = num3;
				m_ManagedMediaDataVector = new IMediaData[(uint)num3];
				System.Runtime.CompilerServices.Unsafe.SkipInit(out _Vector_const_iterator_003Cstd_003A_003A_Vector_val_003Cstd_003A_003A_Simple_types_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_003E_0020_003E_0020_003E obj2);
				*(long*)(&obj2) = 0L;
				ulong num5 = 0uL;
				_Vector_val_003Cstd_003A_003A_Simple_types_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_003E_0020_003E* ptr2 = (_Vector_val_003Cstd_003A_003A_Simple_types_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_003E_0020_003E*)(&obj);
				_Vector_val_003Cstd_003A_003A_Simple_types_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_003E_0020_003E* ptr3 = (_Vector_val_003Cstd_003A_003A_Simple_types_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_003E_0020_003E*)(&obj);
				long num6 = *(long*)(&obj);
				*(long*)(&obj2) = *(long*)(&obj);
				System.Runtime.CompilerServices.Unsafe.SkipInit(out _Vector_iterator_003Cstd_003A_003A_Vector_val_003Cstd_003A_003A_Simple_types_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_003E_0020_003E_0020_003E obj3);
				while (true)
				{
					_Vector_val_003Cstd_003A_003A_Simple_types_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_003E_0020_003E* ptr4 = (_Vector_val_003Cstd_003A_003A_Simple_types_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_003E_0020_003E*)(&obj);
					_Vector_val_003Cstd_003A_003A_Simple_types_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_003E_0020_003E* ptr5 = (_Vector_val_003Cstd_003A_003A_Simple_types_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_003E_0020_003E*)(&obj);
					long num7 = System.Runtime.CompilerServices.Unsafe.As<vector_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_003E_0020_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 8));
					*(long*)(&obj3) = System.Runtime.CompilerServices.Unsafe.As<vector_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_003E_0020_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 8));
					byte b = ((*(long*)(&obj2) == System.Runtime.CompilerServices.Unsafe.As<vector_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_003E_0020_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 8))) ? ((byte)1) : ((byte)0));
					byte b2 = b;
					if (b != 0)
					{
						break;
					}
					ImMediaData** ptr6 = (ImMediaData**)(*(ulong*)(&obj2));
					long num8 = *(long*)(*(ulong*)(&obj2));
					MediaData mediaData = new MediaData((ImMediaData*)global::_003CModule_003E.__RTDynamicCast(((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImData*>)(*(ulong*)(*(long*)num8 + 48)))((nint)num8), 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImData_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImMediaData_0040NmToolkit_0040_0040_00408), 0));
					m_ManagedMediaDataVector[(uint)num5] = mediaData;
					*(long*)(&obj2) += 8L;
					num5++;
				}
				m_HasCreatedManagedVector = true;
				managedMediaDataVector = m_ManagedMediaDataVector;
			}
			catch
			{
				//try-fault
				global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<vector_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Evector_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_003E_0020_003E_002E_007Bdtor_007D), &obj);
				throw;
			}
			global::_003CModule_003E.std_002Evector_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_002Cstd_003A_003Aallocator_003CNmToolkit_003A_003AImMediaData_0020const_0020_002A_003E_0020_003E_002E_Tidy(&obj);
			return managedMediaDataVector;
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImToolkitError_0040NmToolkit_0040_0040_00408), 9, &error) != 0)
		{
			uint num9 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new ToolkitException(error);
				}
				catch when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					num9 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num9 != 0;
				}).Invoke())
				{
				}
				if (num9 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num9);
			}
		}
		catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error2) != 0)
		{
			uint num9 = 0u;
			global::_003CModule_003E.__CxxRegisterExceptionObject((void*)Marshal.GetExceptionPointers(), (void*)num);
			try
			{
				try
				{
					throw new StdCppException(error2);
				}
				catch when (((Func<bool>)delegate
				{
					// Could not convert BlockContainer to single expression
					num9 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
					return (byte)num9 != 0;
				}).Invoke())
				{
				}
				if (num9 != 0)
				{
					throw;
				}
			}
			finally
			{
				global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num9);
			}
		}
		return null;
	}
}
