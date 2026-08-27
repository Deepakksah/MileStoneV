using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NmTimeStamp;
using NmToolkit;
using std;

namespace VideoOS.Toolkit;

public class UTCTimeProperty : Property, IUTCTimeProperty
{
	public unsafe virtual DateTime Value
	{
		get
		{
			//IL_0008: Expected I8, but got I
			//IL_00b9: Expected I, but got I8
			//IL_008e: Expected I, but got I8
			//IL_010a: Expected I, but got I8
			//IL_00e0: Expected I, but got I8
			//IL_0027: Expected I, but got I8
			long num = (nint)stackalloc byte[global::_003CModule_003E.__CxxQueryExceptionSize()];
			System.Runtime.CompilerServices.Unsafe.SkipInit(out ImToolkitError* error);
			System.Runtime.CompilerServices.Unsafe.SkipInit(out exception* error2);
			try
			{
				void* intPtr = global::_003CModule_003E.__RTDynamicCast(m_Property, 0, System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImProperty_0040NmToolkit_0040_0040_00408), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImUTCTimeProperty_0040NmToolkit_0040_0040_00408), 0);
				long num2 = ((delegate* unmanaged[Cdecl, Cdecl]<IntPtr, long>)(*(ulong*)(*(ulong*)intPtr)))((nint)intPtr);
				GC.KeepAlive(this);
				System.Runtime.CompilerServices.Unsafe.SkipInit(out NmTimeStamp.CmTimeStamp cmTimeStamp);
				NmTimeStamp.CmTimeStamp* stamp = global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bctor_007D(&cmTimeStamp, num2);
				DateTime dateTime;
				try
				{
					dateTime = global::_003CModule_003E.msclr_002Einterop_002Emarshal_as_003Cclass_0020System_003A_003ADateTime_002Cclass_0020NmTimeStamp_003A_003ACmTimeStamp_003E(stamp);
					DateTime dateTime2 = dateTime;
				}
				catch
				{
					//try-fault
					global::_003CModule_003E.___CxxCallUnwindDtor((delegate*<void*, void>)(delegate*<NmTimeStamp.CmTimeStamp*, void>)(&global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D), &cmTimeStamp);
					throw;
				}
				global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002E_007Bdtor_007D(&cmTimeStamp);
				GC.KeepAlive(this);
				return dateTime;
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVImToolkitError_0040NmToolkit_0040_0040_00408), 9, &error) != 0)
			{
				uint num3 = 0u;
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
						num3 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
						return (byte)num3 != 0;
					}).Invoke())
					{
					}
					if (num3 != 0)
					{
						throw;
					}
				}
				finally
				{
					global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num3);
				}
			}
			catch when (global::_003CModule_003E.__CxxExceptionFilter((void*)Marshal.GetExceptionPointers(), System.Runtime.CompilerServices.Unsafe.AsPointer(ref global::_003CModule_003E._003F_003F_R0_003FAVexception_0040std_0040_0040_00408), 9, &error2) != 0)
			{
				uint num3 = 0u;
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
						num3 = (uint)global::_003CModule_003E.__CxxDetectRethrow((void*)Marshal.GetExceptionPointers());
						return (byte)num3 != 0;
					}).Invoke())
					{
					}
					if (num3 != 0)
					{
						throw;
					}
				}
				finally
				{
					global::_003CModule_003E.__CxxUnregisterExceptionObject((void*)num, (int)num3);
				}
			}
			System.Runtime.CompilerServices.Unsafe.SkipInit(out DateTime result);
			return result;
		}
	}

	public unsafe UTCTimeProperty(ImUTCTimeProperty* property, object ownerRef)
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
