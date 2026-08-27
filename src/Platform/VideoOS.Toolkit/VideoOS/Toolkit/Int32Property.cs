using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NmToolkit;
using std;

namespace VideoOS.Toolkit;

public class Int32Property : Property, IInt32Property
{
	public unsafe virtual int Value
	{
		get
		{
			//IL_0008: Expected I8, but got I
			//IL_0082: Expected I, but got I8
			//IL_0058: Expected I, but got I8
			//IL_00d3: Expected I, but got I8
			//IL_00a9: Expected I, but got I8
			//IL_0027: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
			try
			{
				void* intPtr = global::_003CModule_003E.__RTDynamicCast(m_Property, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImInt32Property_0040NmToolkit_0040_0040_00408), 0);
				int result = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, int>)(*(ulong*)(*(ulong*)intPtr)))((nint)intPtr);
				GC.KeepAlive(this);
				return result;
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImToolkitError_0040NmToolkit_0040_0040_00408), 9, &error) != 0)
			{
				uint num2 = 0u;
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
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error2) != 0)
			{
				uint num2 = 0u;
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
			return 0;
		}
	}

	public unsafe Int32Property(ImInt32Property* property, object ownerRef)
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
