using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NmToolkit;
using std;

namespace VideoOS.Toolkit;

public class DataProperty : Property, IDataProperty
{
	public unsafe virtual IData Value
	{
		get
		{
			//IL_0008: Expected I8, but got I
			//IL_009f: Expected I, but got I8
			//IL_0074: Expected I, but got I8
			//IL_00f1: Expected I, but got I8
			//IL_00c6: Expected I, but got I8
			//IL_0027: Expected I, but got I8
			//IL_003f: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
			try
			{
				void* intPtr = global::_003CModule_003E.__RTDynamicCast(m_Property, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImDataProperty_0040NmToolkit_0040_0040_00408), 0);
				ImData* ptr = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImData*>)(*(ulong*)(*(ulong*)intPtr)))((nint)intPtr);
				if (ptr == null)
				{
					return null;
				}
				IData result = DataFactory.CreateFromImData(((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, ImData*>)(*(ulong*)(*(long*)ptr + 48)))((nint)ptr));
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
			return null;
		}
	}

	public unsafe DataProperty(ImDataProperty* property, object ownerRef)
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
