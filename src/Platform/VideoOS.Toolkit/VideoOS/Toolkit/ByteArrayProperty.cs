using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NmToolkit;
using std;

namespace VideoOS.Toolkit;

public class ByteArrayProperty : Property, IByteArrayProperty
{
	public unsafe virtual byte[] Value
	{
		get
		{
			//IL_0008: Expected I8, but got I
			//IL_0132: Expected I, but got I8
			//IL_0107: Expected I, but got I8
			//IL_0184: Expected I, but got I8
			//IL_0159: Expected I, but got I8
			//IL_0030: Expected I, but got I8
			//IL_0085: Expected I, but got I8
			//IL_008f: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
			try
			{
				ImByteArrayProperty* ptr = (ImByteArrayProperty*)global::_003CModule_003E.__RTDynamicCast(m_Property, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImByteArrayProperty_0040NmToolkit_0040_0040_00408), 0);
				System.Runtime.CompilerServices.Unsafe.SkipInit(out vector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E obj);
				global::_003CModule_003E.std_002Evector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E_002E_007Bctor_007D(&obj, ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, vector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E*>)(*(ulong*)(*(long*)ptr + 8)))((nint)ptr));
				byte[] result;
				try
				{
					_Vector_val_003Cstd_003A_003A_Simple_types_003Cunsigned_0020char_003E_0020_003E* ptr2 = (_Vector_val_003Cstd_003A_003A_Simple_types_003Cunsigned_0020char_003E_0020_003E*)(&obj);
					byte b = ((*(long*)(&obj) == System.Runtime.CompilerServices.Unsafe.As<vector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 8))) ? ((byte)1) : ((byte)0));
					byte b2 = b;
					if (b != 0)
					{
						result = null;
						goto IL_0064;
					}
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<vector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Evector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E_002E_007Bdtor_007D), &obj);
					throw;
				}
				byte[] array;
				try
				{
					IntPtr zero = IntPtr.Zero;
					_Vector_val_003Cstd_003A_003A_Simple_types_003Cunsigned_0020char_003E_0020_003E* ptr3 = (_Vector_val_003Cstd_003A_003A_Simple_types_003Cunsigned_0020char_003E_0020_003E*)(&obj);
					byte* ptr4 = (byte*)(*(ulong*)(&obj));
					IntPtr intPtr = new IntPtr((void*)(*(ulong*)(&obj)));
					zero = intPtr;
					_Vector_val_003Cstd_003A_003A_Simple_types_003Cunsigned_0020char_003E_0020_003E* ptr5 = (_Vector_val_003Cstd_003A_003A_Simple_types_003Cunsigned_0020char_003E_0020_003E*)(&obj);
					ulong num2 = (ulong)(System.Runtime.CompilerServices.Unsafe.As<vector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E, long>(ref System.Runtime.CompilerServices.Unsafe.AddByteOffset(ref obj, 8)) - *(long*)(&obj));
					ulong num3 = num2;
					int num4 = (int)num2;
					array = new byte[num4];
					Marshal.Copy(intPtr, array, 0, num4);
					GC.KeepAlive(this);
					byte[] array2 = array;
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<vector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E*, void>)(&global::_003CModule_003E.std_002Evector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E_002E_007Bdtor_007D), &obj);
					throw;
				}
				global::_003CModule_003E.std_002Evector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E_002E_Tidy(&obj);
				return array;
				IL_0064:
				global::_003CModule_003E.std_002Evector_003Cunsigned_0020char_002Cstd_003A_003Aallocator_003Cunsigned_0020char_003E_0020_003E_002E_Tidy(&obj);
				return result;
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImToolkitError_0040NmToolkit_0040_0040_00408), 9, &error) != 0)
			{
				uint num5 = 0u;
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
						num5 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
						return (byte)num5 != 0;
					}).Invoke())
					{
					}
					if (num5 != 0)
					{
						throw;
					}
				}
				finally
				{
					global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num5);
				}
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error2) != 0)
			{
				uint num5 = 0u;
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
						num5 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
						return (byte)num5 != 0;
					}).Invoke())
					{
					}
					if (num5 != 0)
					{
						throw;
					}
				}
				finally
				{
					global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num5);
				}
			}
			return null;
		}
	}

	public unsafe ByteArrayProperty(ImByteArrayProperty* property, object ownerRef)
	{
		//IL_0018: Expected I, but got I8
		//IL_0006: Expected I, but got I8
		ImProperty* property2;
		if (property == null)
		{
			property2 = null;
		}
		else
		{
			property2 = (ImProperty*)((long)(nint)property + (long)(*(int*)(*(long*)((ulong)(nint)property + 8uL) + 4)) + 8);
		}
		base._002Ector(property2, ownerRef);
	}
}
